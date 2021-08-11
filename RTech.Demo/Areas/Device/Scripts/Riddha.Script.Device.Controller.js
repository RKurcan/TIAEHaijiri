/// <reference path="Riddha.Script.Device.Model.js" />
/// <reference path="Riddha.Script.Device.Controller.js" />

function modelController() {
    var self = this;
    var url = "/Api/ModelApi";
    self.Model = ko.observable(new ModelModel());
    self.Models = ko.observableArray([]);
    self.SelectedModel = ko.observable();
    self.ModeOfButton = ko.observable('Create');

    //Calling getFunctions
    //getModels();
    //enum wala
    self.ManufactureList = ko.observableArray([
        { Id: 1, Name: 'ZKT' }
    ]);

    self.GetManufactureName = function (id) {
        var mapped = ko.utils.arrayFirst(self.ManufactureList(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    }

    self.Brands = ko.observableArray([
        { Id: 1, Name: 'ZKT' }
    ]);

    self.GetBrandName = function (id) {
        var mapped = ko.utils.arrayFirst(self.Brands(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    }

    function getModels() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ModelModel);
                self.Models(data);
            });
    };

    self.KendoGridOptions = {
        title: "Model",
        target: "#modelKendoGrid",
        url: "/Api/ModelApi/GetKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        group: true,
        selectable: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Name', title: "Device Name", filterable: false, width: 100 },
            { field: 'IsAccessDevice', title: "Is Access Device ?", filterable: false, width: 100, template: "#=GetBoolName(IsAccessDevice)#" },
            { field: 'IsFaceDevice', title: "Is Face Device ?", filterable: false, width: 100, template: "#=GetBoolName(IsFaceDevice)#" },
            { field: 'ImageURL', title: "User", filterable: false, template: '<img width="150" height="100" src="#= ImageURL #" alt="image" />', width: 100, height: 50 },
        ],
        SelectedItem: function (item) {
            self.SelectedModel(new ModelModel(item));
        },
        SelectedItems: function (items) {
        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedModel(new ModelModel());
        $("#modelKendoGrid").getKendoGrid().dataSource.read();
    };

    self.CreateUpdate = function () {
        if (self.Model().Name.hasError()) {
            return Riddha.UI.Toast(self.Model().Name.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Model()))
                .done(function (result) {
                    self.RefreshKendoGrid();
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Model()))
                .done(function (result) {
                    self.RefreshKendoGrid();
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                });
        }

    };

    self.Reset = function () {
        self.Model(new ModelModel({ Id: self.Model().Id() }));
        //self.ModeOfButton('Create');
    };

    self.Select = function () {
        if (self.SelectedModel() == undefined) {
            Riddha.UI.Toast("Please select model to edit.", 0);
            return;
        };
        self.Model(new ModelModel(ko.toJS(self.SelectedModel())));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (model) {
        if (self.SelectedModel() == undefined) {
            Riddha.UI.Toast("Please select model to delete.", 0);
            return;
        };
        Riddha.UI.Confirm("Confirm to Delete this Model?", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedModel().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.ModeOfButton("Create");
                        self.Reset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#modelCreationModel").modal('show');
    };

    $("#modelCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
        self.ModeOfButton("Create");
    });

    self.CloseModal = function () {
        $("#modelCreationModel").modal('hide');
        self.Reset();
        self.ModeOfButton("Create");

    };
}

function deviceController() {
    var self = this;
    var url = "/Api/DeviceApi";
    self.Device = ko.observable(new DeviceModel());
    self.Devices = ko.observableArray([]);
    self.Quantity = ko.observable(0);
    self.ModeOfButton = ko.observable('Create');
    self.SelectedDevice = ko.observable(new DeviceModel());

    self.DeviceTypeList = ko.observableArray([
        { Id: 0, Name: 'Normal' },
        { Id: 1, Name: 'ADMS' },
    ]);

    self.GetDeviceTypeName = function (id) {
        var mapped = ko.utils.arrayFirst(self.DeviceTypeList(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    }

    self.StatusList = ko.observableArray([
        { Id: 0, Name: 'New' },
        { Id: 1, Name: 'Reseller' },
        { Id: 2, Name: 'Customer' },
        { Id: 3, Name: 'Damage' }
    ]);

    self.GetStatusName = function (id) {
        var mapped = ko.utils.arrayFirst(self.StatusList(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    }

    self.Models = ko.observableArray([]);
    getModels();
    self.DeviceList = ko.observableArray([]);


    self.KendoGridOptions = {
        title: "Device",
        target: "#deviceKendoGrid",
        url: "/Api/DeviceApi/GetOwnerDeviceKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        group: true,
        selectable: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Model', title: "Model", filterable: false, width: 100 },
            { field: 'SerialNumber', title: "Serial Number", filterable: false, width: 100 },
            { field: 'DeviceTypeName', title: "Device Type", filterable: false, width: 100 },
            { field: 'StatusName', title: "Status", filterable: false, width: 100 },
        ],
        SelectedItem: function (item) {
            self.SelectedDevice(new DeviceGridViewModel(item));
        },
        SelectedItems: function (items) {
        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedDevice(new DeviceGridViewModel());
        $("#deviceKendoGrid").getKendoGrid().dataSource.read();
    };

    //function getDevices() {
    //    Riddha.ajax.get(url)
    //    .done(function (result) {
    //        var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DeviceGridViewModel);
    //        self.DeviceList(data);
    //    });
    //};

    function getModels() {
        Riddha.ajax.get("/Api/ModelApi")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ModelModel);
                self.Models(data);
            });
    };
    //self.filterText = ko.observable("");

    //self.gridOptions =
    //    riddhaKoGrid(
    //        {
    //            data: self.DeviceList,
    //            columnDefs: [{ field: 'Id', visible: false }, { field: 'Model', displayName: 'Model' }, { field: 'SerialNumber', displayName: 'Device SNo' }, { field: 'DeviceType', displayName: 'DeviceType' }, { field: 'Status', displayName: 'Status' }],
    //            filterText: self.filterText,
    //            pageSize: 10,
    //            enableServerPaging: true,
    //            jsonUrl: url,
    //            getSelectedItem: function (data) {
    //                self.SelectedDevice(data);
    //            },
    //            getSelectedItems: function (data) {
    //                //self.selectedManagementCommittees(data);
    //            }
    //        });

    self.DeviceRows = ko.observableArray([]);
    self.ShowGrid = function () {
        self.DeviceRows(ko.utils.range(1, self.Quantity()));
        self.Devices([]);
        ko.utils.arrayForEach(self.DeviceRows(), function (item) {
            self.Devices.push(new DeviceModel({ ModelId: self.Device().ModelId() }));
        })

    };

    self.CheckDuplicateSNo = function (item, event) {
        var index = ko.contextFor(event.target).$index();
        for (var i = 1; i <= index; i++) {
            if (item.SerialNumber() == self.Devices()[i - 1].SerialNumber()) {
                Riddha.UI.Toast("This Serial Number is already exist!!!", Riddha.CRM.Global.ResultStatus.processError);
                return self.Devices()[index].SerialNumber("");
            }
        }
        if (item.SerialNumber() == "") {
            return false;
        }
        Riddha.ajax.get(url + "/CheckDuplicateSNo/?SerialNumber=" + item.SerialNumber())
            .done(function (result) {
                if (result == true) {
                    //user toast using process   
                    Riddha.UI.Toast("This Serial Number is already exist!!!", Riddha.CRM.Global.ResultStatus.processError);
                    return self.Devices()[index].SerialNumber("");
                }
            })
    }

    self.CreateUpdate = function () {
        if (self.Device().ModelId() == undefined) {
            return Riddha.UI.Toast("!!!Please select Device Model", Riddha.CRM.Global.ResultStatus.processError);
        }
        //if (self.Quantity() == 0) {
        //    return Riddha.UI.Toast("!!!Please Enter Quantity of Devices ", Riddha.CRM.Global.ResultStatus.processError);
        //}
        //if (self.Devices().length==0) {
        //    return Riddha.UI.Toast("!!!Please Select Details", Riddha.CRM.Global.ResultStatus.processError);
        //}
        var isValid = false;
        ko.utils.arrayForEach(self.Devices(), function (item) {
            if (item.SerialNumber() == "") {
                isValid = true;
                return;
            }
            else {
                isValid = false;
                return;
            }
        })
        if (isValid) {
            return Riddha.UI.Toast("Please enter serial number of all devices!!!", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.ModeOfButton() == 'Create') {
            var data = { Devices: self.Devices() };
            Riddha.ajax.post(url, ko.toJS(data))
                .done(function (result) {
                    //var data = Riddha.ko.global.arrayMap(result.Data, DeviceModel);
                    //self.Devices(data);
                    //self.DeviceList.push(new DeviceGridViewModel(result.Data));
                    //obj.gridOptions.refresh();
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.RefreshKendoGrid();
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Device()))
                .done(function (result) {
                    //self.Devices.replace(self.SelectedDevice(), new DeviceModel(ko.toJS(self.Device())));
                    //self.ModeOfButton("Create");
                    //self.Reset();
                    //var DeviceType = find(self.DeviceTypeList(), 'Id', self.Device().DeviceType());
                    //var Model = find(self.Models(), 'Id', self.Device().ModelId())
                    //var selectedModel = self.SelectedDevice();
                    //selectedModel.Model = Model.Name();
                    //selectedModel.DeviceType = DeviceType.Name;
                    //self.DeviceList.replace(self.SelectedDevice(), selectedModel);
                    self.RefreshKendoGrid();
                    Riddha.UI.Toast("Updated Succesfully", 4, "Updated");
                    self.ModeOfButton("Create");
                    self.EditCloseModal();
                    self.Reset();
                });
        }
    };
    self.Reset = function () {
        self.Devices([]);
        self.Device(new DeviceModel());
        self.Quantity(0);
    };

    self.Select = function (model) {
        if (self.SelectedDevice() == undefined) {
            return Riddha.UI.Toast("Please Select Row to Edit", Riddha.CRM.Global.ResultStatus.processError);
        }
        self.Device(new DeviceModel(ko.toJS(self.SelectedDevice())));
        //var model = find(self.Models(), 'Name', self.SelectedDevice().Model);
        //if (model)
        //    self.Device().ModelId(model.Id());
        //var DeviceType = find(self.DeviceTypeList(), 'Name', self.SelectedDevice().DeviceType);

        //self.Device().DeviceType(DeviceType.Id);
        self.ModeOfButton('Update');
        self.EditShowModal();
    };

    function find(array, filterBy, value) {
        return ko.utils.arrayFirst(array, function (item) {
            if (typeof (item[filterBy]) == 'function')
                return item[filterBy]() == value
            else
                return item[filterBy] == value
        });
    }

    self.Delete = function (device) {
        if (self.SelectedDevice() == undefined) {
            return Riddha.UI.Toast("Please Select Row to Delete", Riddha.CRM.Global.ResultStatus.processError);
        }
        Riddha.UI.Confirm("Confirm to delete this message?", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedDevice().Id, null)
                .done(function (result) {
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.DeviceList.remove(self.SelectedDevice());
                    self.SelectedDevice(undefined);
                });
        });
    };

    self.Delete = function (model) {
        if (self.SelectedDevice() == undefined) {
            Riddha.UI.Toast("Please select device to delete.", 0);
            return;
        };
        if (self.SelectedDevice().Status() != 0) {
            Riddha.UI.Toast("Only new device can be deleted.", 0);
            return;
        };
        Riddha.UI.Confirm("Confirm to Delete this Model?", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedDevice().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.ModeOfButton("Create");
                        self.Reset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };


    self.EditShowModal = function () {
        $("#deviceEditModel").modal('show');
    };

    $("#deviceEditModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.EditCloseModal = function () {
        $("#deviceEditModel").modal('hide');
        self.Reset();
        self.ModeOfButton("Create");
    };

    self.ShowModal = function () {
        $("#deviceCreationModel").modal('show');
    };

    $("#deviceCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#deviceCreationModel").modal('hide');
        self.Reset();
        self.ModeOfButton("Create");
    };

    self.trigerFileBrowse = function () {
        $("#UploadedFile").click();
    }
    self.UploadClick = function () {
        var xhr = new XMLHttpRequest();
        var file = document.getElementById('UploadedFile').files[0];
        if (file == undefined) {
            return Riddha.UI.Toast("Please Select Excel File to Upload", Riddha.CRM.Global.ResultStatus.processError);
        }

        Riddha.UI.Confirm("Confirm to upload Device?", function () {
            xhr.open("POST", "/api/DeviceApi/Upload?modelId=" + self.Device().ModelId() + "&quantity=" + self.Quantity());
            xhr.setRequestHeader("filename", file.name);
            xhr.onreadystatechange = function (data) {
                if (xhr.readyState == 4) {
                    var response = JSON.parse(xhr.responseText);
                    if (response["Status"] == 4) {
                        var data = Riddha.ko.global.arrayMap(response["Data"], DeviceModel);
                        self.Devices(data);
                    }
                    return Riddha.UI.Toast(response["Message"], response["Status"]);
                }
            };
            xhr.send(file);
        });
    };
}

function companyDeviceController() {
    var self = this;
    var url = "/Api/DeviceApi";
    var admsUrl = "/Api/ADMSApi";
    self.Device = ko.observable(new DeviceModel());
    self.Devices = ko.observableArray([]);
    self.SelectedDevice = ko.observable(new DeviceModel());
    self.DeviceArray = ko.observableArray([]);
    self.SelectedItems = ko.observableArray([]);
    self.ButtonVisible = ko.observable(false);
    self.KendoGridOptions = {
        title: "Company Device",
        target: "#companyDeviceKendoGrid",
        url: "/Api/DeviceApi/GetKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        selectable: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Name', title: "Device Name", filterable: false, width: 100 },
            { field: 'IpAddress', title: "IpAddress", filterable: false, width: 100 },
            { field: 'SerialNumber', title: "Device Serial Number", filterable: false, width: 130 },
            //{ field: 'Type', title: "Device Type", filterable: false, template: "#=GetDeviceType(Type)#"  },
            { field: 'ModelName', title: "Model Name", filterable: false },
            { field: 'Status', title: "Status", filterable: false, template: "#=GetDeviceStatus(Status)#", width: 150 },
            { field: 'LastActivity', title: "Last Activity", filterable: false, width: 130 },
            { field: 'DevFuns', title: "Funs", filterable: false },
            { field: 'FaceCount', title: "Face ", filterable: false, width: 50 },
            { field: 'FPCount', title: "FP", filterable: false, width: 50 },
            { field: 'TransCount', title: "Trans", filterable: false, width: 50 },
            { field: 'UserCount', title: "User", filterable: false, width: 50 },
            { field: 'FwVersion', title: "Fw", filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedDevice(new DeviceModel(item));

            self.ButtonVisible(item.Type == 'ADMS');

        },
        SelectedItems: function (items) {
            self.SelectedItems(Riddha.ko.global.arrayMap(items, DeviceModel));

        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedDevice(new DeviceModel());
        $("#companyDeviceKendoGrid").getKendoGrid().dataSource.read();
    };

    self.Update = function () {
        Riddha.ajax.put(url + "/UpdateDevice", ko.toJS(self.Device()))
            .done(function (result) {
                if (result.Status == 4) {
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.CloseModal();
                }
            });
    };

    self.Select = function (model) {
        if (self.SelectedDevice().SerialNumber() == "") {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        self.Device(new DeviceModel(ko.toJS(self.SelectedDevice())));
        self.ShowModal();
    };

    self.ShowModal = function () {
        $("#deviceUpdateModal").modal('show');
    };

    $("#deviceUpdateModal").on('hidden.bs.modal', function () {
        self.RefreshKendoGrid();
        self.Reset();
    });

    self.CloseModal = function () {
        $("#deviceUpdateModal").modal('hide');
        self.RefreshKendoGrid();
        self.Reset();
    };

    self.Reset = function () {
        self.Device(new DeviceModel());
    };

    //Region ADMS functions
    self.DownloadAttendanceLog = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to download attendance log ?", function () {
            Riddha.ajax.get(admsUrl + "/checkDeviceData?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Download attendance log command executed.", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.DeleteAttendanceLog = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to delete attendance log ?", function () {
            Riddha.ajax.get(admsUrl + "/ClearAttLogFromDevice?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Device data deleted command executed.", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.DeleteDeviceData = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to delete device data ?", function () {
            Riddha.ajax.get(admsUrl + "/ClearAllData?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Device data deleted command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.DeleteServerData = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to delete server data ?", function () {
            Riddha.ajax.get(admsUrl + "/deleteDevice?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Server data deleted command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.DownloadUserInfofromDevice = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });

        Riddha.UI.Confirm("Confirm to download user info from device ?", function () {
            Riddha.ajax.get(admsUrl + "/checkDeviceData?sn=" + array.toString() + "&checkNew=no", null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Downloaduser info command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.RefreshDevice = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to refresh device ?", function () {
            Riddha.ajax.get(admsUrl + "/checkDeviceInfo?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Device refresh command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.RebootDevice = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to refresh device ?", function () {
            Riddha.ajax.get(admsUrl + "/RebootDevice?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Device reboot command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.CopyDataFromBranch = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to copy data from branch ?", function () {
            Riddha.ajax.get(admsUrl + "/ToNewDevice?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Copy data from branch command executed.", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.CopyDataFromDepartment = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to copy data from department ?", function () {
            Riddha.ajax.get(admsUrl + "/ToNewDevice?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Copy data from department command executed.", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.UpdateUserNameToDevice = function () {
        if (self.ButtonVisible() == false) {
            return;
        }
        var array = new Array();
        ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            array.push(item.SerialNumber());
        });
        Riddha.UI.Confirm("Confirm to update user name to device ?", function () {
            Riddha.ajax.get(admsUrl + "/RestoreUserInfo?sn=" + array.toString(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("Update user name to device command executed.", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };
    //End Region
}

function GetDeviceStatus(data) {
    if (data == 'Not Authorized') {
        return "<span class='badge bg-orange'>" + "Not Authorized" + "</span>";
    }
    if (data == 'Online') {
        return "<span class='badge bg-green'>" + "Online" + "</span>";
    }
    if (data == 'Offline') {
        return "<span class='badge bg-red'>" + "Offline" + "</span>";
    }
    if (data == 'Non Adms Devices') {
        return "<span class='badge bg-red'>" + "Non Adms Devices" + "</span>";
    }
    else {
        return "<span class='badge bg-blue'>" + data + "</span>";
    }
};

function GetDeviceType(data) {
    if (data == 'ADMS') {
        return "<span class='badge bg-green'>" + "ADMS" + "</span>";
    }

    else {
        return "<span class='badge bg-aqua'>" + "Normal" + "</span>";
    }
};

function GetBoolName(data) {
    if (data == false) {
        return "<span class='badge bg-orange'>" + "NO" + "</span>";
    }

    else {
        return "<span class='badge bg-green'>" + "YES" + "</span>";
    }
};

