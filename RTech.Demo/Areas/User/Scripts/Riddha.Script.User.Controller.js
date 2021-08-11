/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.User.Model.js" />
function userRoleController() {
    var self = this;
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    var url = "/Api/UserRoleApi";
    self.UserRole = ko.observable(new UserRoleModel());
    self.UserRoles = ko.observableArray([]);
    self.SelectedUserRole = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.CheckedNodes = ko.observableArray([]);
    self.CheckAllNodes = ko.observable(false);
    self.MenusAndActions = ko.observableArray([]);
    self.DataVisibilityLevels = ko.observableArray([
        { Id: 0, Name: lang == "ne" ? "आफैं " : "Self" },
        { Id: 1, Name: lang == "ne" ? "फांट" : "Section" },
        { Id: 2, Name: lang == "ne" ? "बिभाग" : "Department" },
        { Id: 3, Name: lang == "ne" ? "शाखा" : "Branch" },
        { Id: 4, Name: lang == "ne" ? "सबै" : "All" },
    ]);

    setSoftwarePackagewiseDataVisibilityLevels();
    function setSoftwarePackagewiseDataVisibilityLevels() {
        var package = parseInt(config.PackageId, 10);
        if (package >= 2) {
            self.DataVisibilityLevels.push({ Id: 5, Name: lang == "ne" ? "पदानुक्रम रिपोर्टिंग" : "Reporting Hierarchy" })
        }
    }
    self.DataVisibilityLevel = ko.observable(0);
    self.Branches = ko.observableArray([]);
    self.BranchId = ko.observable(Riddha.BranchId);

    getBranches();
    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Branches(data);
            });
    };

    self.KendoTree = $("#treeview").kendoTreeView({
        checkboxes: {
            checkChildren: true
        },
        dataSource: rolePermissionsData,
        check: onCheck,
        //expand: onExpand
    }).data("kendoTreeView");


    self.Search = function () {
        GetUserRoles();
    }


    self.dialog = $("#treeViewWindow").kendoDialog({
        width: "500px",
        title: lang == "ne" ? "अनुमती ब्यवस्थापन" : "Permission Management",
        visible: false,
        actions: [
            {
                text: lang == "ne" ? "बन्द गर्नुहोस" : 'Close',
                primary: false,
                action: onCancelClick
            },
            {
                text: lang == "ne" ? "अनुमती बनाउनुस" : 'Create',
                primary: true,
                action: onOkClick
            }
        ],
        close: onClose
    }).data("kendoDialog");

    self.ShowKendoDialog = function (item) {
        self.CheckedNodes([]);
        self.SelectedUserRole(new UserRoleModel(item));
        var treeView = self.KendoTree;
        treeView.dataSource.read();
        Riddha.ajax.get(url + "/GetRolePermissions?roleId=" + item.Id())
            .done(function (result) {
                if (result.Status == 4) {
                    for (var i = 0; i < result.Data.length; i++) {
                        bindCheckbox(treeView, result.Data[i], true);
                    }
                }
            });

        self.dialog.open();
    }

    function bindCheckbox(treeView, id, checked) {
        var dataItem = treeView.dataSource.get(id);
        if (dataItem) {
            dataItem.set("checked", checked);
            self.CheckedNodes.push(new TreeViewModel({ text: dataItem.text, id: dataItem.id, checked: checked, hasChildren: dataItem.hasChildren, parentId: dataItem.parentNode() == undefined ? "" : dataItem.parentNode().id }));
        }
    }


    function GetUserRoles() {
        if (self.BranchId() == undefined) {
            return;
        }
        Riddha.ajax.get(url + "/?branchId=" + self.BranchId())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), UserRoleModel);
                self.UserRoles(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.UserRole().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        }

        if (self.BranchId() != undefined) {
            self.UserRole().BranchId(self.BranchId())
        };

        if (self.UserRole().BranchId() == undefined) {
            Riddha.UI.Toast("Please select branch.", 0);
            return;
        };

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.UserRole()))
                .done(function (result) {
                    self.UserRoles.push(new UserRoleModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.UserRole()))
                .done(function (result) {
                    self.UserRoles.replace(self.SelectedUserRole(), new UserRoleModel(ko.toJS(self.UserRole())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                });
        }
    };
    self.CreateDataVisibilityLevel = function () {
        Riddha.ajax.post(url + "/CreateDataVisibility", { RoleId: self.SelectedUserRole().Id(), DataVisibilityLevel: self.DataVisibilityLevel() }).done(function (data) {
            if (data.Status === 4) {
                self.SelectedUserRole(new UserRoleModel());
                $("#dataVisibilityModal").modal('hide');
            }
            Riddha.UI.Toast(data.Message, data.Status);
        });
    }
    self.GetDataVisibilityLevel = function (model) {
        self.SelectedUserRole(model);
        Riddha.ajax.get(url + "/GetDataVisibilityLevel?roleId=" + model.Id()).done(function (data) {
            self.DataVisibilityLevel(data.Data);
        });
    }

    self.Reset = function () {
        self.UserRole(new UserRoleModel({ Id: self.UserRole().Id() }));
    };
    self.CloseDataVisibilityModal = function () {
        $("#dataVisibilityModal").modal('hide');
    }
    self.Select = function (model) {
        self.SelectedUserRole(model);
        self.UserRole(new UserRoleModel(ko.toJS(model)));
        self.ShowModal();
        self.ModeOfButton('Update');
    };

    self.Delete = function (UserRole) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + UserRole.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UserRoles.remove(UserRole);
                    }
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#userRoleCreationModel").modal('show');
    }
    self.ShowDataVisibilityModal = function (model) {
        $("#dataVisibilityModal").modal('show');
        self.GetDataVisibilityLevel(model);
    }
    $("#userRoleCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
        self.ModeOfButton("Create");
    });

    self.CloseModal = function () {
        $("#userRoleCreationModel").modal('hide');
        self.Reset();
        self.ModeOfButton("Create");
    }

    /*tree view functions*/
    function onCancelClick(e) {
        e.sender.close();
    }

    function onOkClick(e) {
        var checkedNodes = [];
        var treeView = self.KendoTree;
        getCheckedNodes(treeView.dataSource.view(), checkedNodes);
        // populateMultiSelect(checkedNodes);
        var data = { TreeViewLst: self.CheckedNodes(), RoleId: self.SelectedUserRole().Id()() };
        Riddha.ajax.post(url + "/PermitRoles", ko.toJS(data))
            .done(function (result) {
                if (result.Status == 4) {
                    Riddha.UI.Toast(result.Message, result.Status);
                    e.sender.close();
                }
            });
    }

    function onClose() {
        $("#openWindow").fadeIn();
    }
    self.CheckAllNodes.subscribe(function (newValue) {
        debugger;
    });
    function checkUncheckAllNodes(nodes, checked, treeView) {
        for (var i = 0; i < nodes.length; i++) {
            bindCheckbox(treeView, nodes[i].id, checked);
            if (checked) {
                self.CheckedNodes.push(new TreeViewModel({ text: nodes[i].text, id: nodes[i].id, checked: checked, hasChildren: nodes[i].hasChildren, parentId: nodes[i].parentNode() == undefined ? "" : nodes[i].parentNode().id }));
            }
        }
    }
    function chbAllOnChange(checked) {
        self.CheckedNodes([]);
        var checkedNodes = [];
        var treeView = self.KendoTree;
        //var isAllChecked = $('#chbAll').prop("checked");
        checkUncheckAllNodes(treeView.dataSource.view(), checked, treeView)//setMessage(0);
    }

    function getCheckedNodes(nodes, checkedNodes) {
        var node;

        for (var i = 0; i < nodes.length; i++) {
            node = nodes[i];
            if (node.checked && node.hasChildren == false) {
                checkedNodes.push({ text: node.text, id: node.id, checked: node.checked, hasChildren: node.hasChildren, parentId: node.parentNode() == undefined ? "" : node.parentNode().id });
            }

            if (node.hasChildren) {
                getCheckedNodes(node.children.view(), checkedNodes);
            }
        }
    }

    function onCheck() {
        var checkedNodes = [];
        var treeView = self.KendoTree;
        getCheckedNodes(treeView.dataSource.view(), checkedNodes);
        self.CheckedNodes([]);
        var data = Riddha.ko.global.arrayMap(checkedNodes, TreeViewModel);
        self.CheckedNodes(data);
        //setMessage(checkedNodes.length);
    }
}

function userController() {
    var self = this;
    var url = "/Api/UserApi";
    var config = new Riddha.config();
    var userType = config.UserType;
    self.Users = ko.observableArray([]);
    self.User = ko.observable(new UserModel());
    self.Branches = ko.observableArray(getBranches());
    self.SelectedUser = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.Roles = ko.observableArray([]);
    self.IsValid = ko.observable(false);
    self.EmpId = ko.observable();
    self.IsAdmin = ko.observable(false);
    self.EnableRoleSelect = ko.observable(true);
    self.EnableEmpName = ko.observable(true);
    self.owner = ko.observable((userType == 0 || userType == 3) ? true : false);
    self.BranchId = ko.observable(Riddha.BranchId);
    self.GridIsOpen = ko.observable(false);
    self.PasswordInfo = ko.observable('');
    self.PasswordInfoStyle = ko.observable('');
    self.PasswordInfo = ko.computed(function () {
        var language = Riddha.config().CurrentLanguage;
        var password = self.User().Password();
        self.PasswordInfoStyle('form-control text-center bg-red-gradient');
        if (/^([0|\+[0-9]{1,5})?([7-9][0-9]{9})$/.test(password) == false) {
            if (password == "") {
                self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
                return language == "ne" ? "उदाहरण: Hamro@hajiri123 " : "eg:(valid password):Hamro@hajiri123";
            }
            else if (password.length < 6) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डको लम्मबाइ ६ भन्दा बढी चाहिन्छ " : "Password should be of more than 6 chars";
            } else if (password.search(/\d/) == -1) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा नम्बर अनिवार्य छ" : "Password should contain a number";
            } else if (password.search(/[a-zA-Z]/) == -1) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा अल्फाबेट अनिवार्य छ" : "Password should contain a alphabet";
            } else if (/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/.test(password) == false) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा बिषेस स्ंकेत अनिवार्य छ" : "Password should contain a special symbol";
            } else if ((/[A-Z]/.test(password) && /[a-z]/.test(password)) == false) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा " : "Password should contain a upper & lower case";
            }
            else {
                self.IsValid(true);
                self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
                return language == "ne" ? "पास्वर्ड ठीक छ" : "Valid Password";
            }
        }
        else {
            self.IsValid(true);
            self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
            return language == "ne" ? "पास्वर्ड ठीक छ" : "Valid Password";

        }

    }, self);


    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Branches(data);
            });
    };
    self.GetBranchName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Branches()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetUserRoles = function (branchId) {
        GetRoles(branchId);
    };


    function GetRoles(branchId) {
        Riddha.ajax.get(url + "/GetRolesForDropdown?branchId=" + self.User().BranchId())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Roles(data);
            })
    };


    self.GetRoleName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Roles()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };



    self.CreateUpdate = function () {
        //if (!self.IsAdmin()) {
        //    if (self.User().RoleId() == undefined) {
        //        return Riddha.util.localize.Required("Role");
        //    }
        //}
        if (self.User().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        }
        if (!self.owner()) {
            if (self.User().BranchId() == undefined) {
                return Riddha.util.localize.Required("Branch");
            }
        }
        if (self.User().Password.hasError()) {
            return Riddha.util.localize.Required("Password");
        }
        if (self.IsValid() == false) {
            return Riddha.UI.Toast("Invalid Password, Please Check u'r Password Valid or Not!!", 2);
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, { User: self.User(), EmpId: self.EmpId() })
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Users.push(new UserModel(result.Data));
                        self.Reset();
                        self.RefreshKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            if (self.EnableRoleSelect()) {
                if (self.User().RoleId() == undefined) {
                    return Riddha.util.localize.Required("Role");
                }
            }
            Riddha.ajax.put(url, { User: self.User(), EmpId: self.EmpId() })
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Users.replace(self.SelectedUser(), new UserModel(ko.toJS(self.User())));
                        self.ModeOfButton("Create");
                        self.RefreshKendoGrid();
                        self.CloseModal();
                        self.Reset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Reset = function () {
        if (self.User().RoleId() == undefined) {
            self.EnableRoleSelect(false);
        }
        else {
            self.EnableRoleSelect(true);
        }
        self.User(new UserModel());

    };

    self.Delete = function () {
        if (self.SelectedUser() == null) {
            Riddha.UI.Toast("Please select an Employee to Delete", 0);
        }
        var id = self.SelectedUser().Id()
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + id, null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Users.remove(self.SelectedUser());
                        self.ModeOfButton("Create");
                        self.Reset();
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };



    self.Select = function (user) {
        self.GetUserRoles();
        self.SelectedUser(new UserModel(user));
        if (user.RoleId == undefined) {
            self.EnableRoleSelect(false);
            self.EnableEmpName(false);
            self.IsAdmin(true);
        }
        else {
            self.EnableRoleSelect(true);
            self.EnableEmpName(true);
            self.IsAdmin(false);
            self.EmpId(user.EmpId);
        }
    }

    self.ShowModal = function (mode) {
        GetRoles();
        if (mode == 'create') {
            self.ModeOfButton('Create');
            self.User(new UserModel());
            self.EmpId(0);
            self.EnableRoleSelect(true);
            self.EnableEmpName(true);
            self.IsAdmin(false);
        }
        else {
            if (self.SelectedUser() == null) {
                Riddha.UI.Toast("Please select an emloyee to update", 0)
            }
            self.ModeOfButton('Update');
            self.User(self.SelectedUser());
        }
        $("#userCreationModel").modal('show');
    };

    $("#userCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
        self.ModeOfButton("Create");
    });

    self.CloseModal = function () {
        $("#userCreationModel").modal('hide');
        self.Reset();
        self.ModeOfButton("Create");
    };

    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/UserApi/GetEmpLstForAutoComplete",
        paramData: function () { return { BranchId: self.User().BranchId() } },
        select: function (item) {
            self.User().Email(item.Email);
            self.User().PhotoURL(item.Photo);
            self.User().FullName(item.EmployeeName);
            self.EmpId(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    }

    function GetBranchName(id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Branches()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.Search = function () {
        if (self.GridIsOpen())
            self.DestroyGrid();
        self.GridIsOpen(true);
        if (self.checkCallBack == "") {
            return;
        }
        self.checkCallBack();
    }
    self.checkCallBack = "";

    self.KendoGridOptions = {
        title: "User",
        target: "#userKendoGrid",
        url: url + "/GetUserKendoGrid",
        height: 500,
        paramData: function () { return { BranchId: self.BranchId() } },
        multiSelect: false,
        selectable: true,
        group: true,
        columns: [
            { field: '#', title: lang == "ne" ? "S.No" : "S.No", width: 80, template: "#= ++record #", filterable: false },
            { field: 'BranchName', hidden: self.owner(), title: lang == "ne" ? "" : "Branch", width: 225, filterable: false  },
            { field: 'Name', title: lang == "ne" ? "Name" : "Name" },
            { field: 'FullName', title: lang == "ne" ? "FullName" : "FullName" },
            { field: 'RoleName', title: lang == "ne" ? "Role" : "Role", filterable: false },
        ],
        SelectedItem: function (item) {
            self.Select(item)
        },
        SelectedItems: function (items) {
        },
        open: function (callback) {
            self.checkCallBack = callback;
        },
    };

    self.RefreshKendoGrid = function () {
        $("#userKendoGrid").getKendoGrid().dataSource.read();
    };

    self.DestroyGrid = function () {
        var grid = $("#userKendoGrid").getKendoGrid()
        grid.destroy();
    };
}


