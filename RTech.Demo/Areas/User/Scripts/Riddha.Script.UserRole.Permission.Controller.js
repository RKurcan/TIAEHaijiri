/// <reference path="../../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.User.Model.js" />
/// <reference path="Riddha.Script.UserRole.Permission.Model.js" />

function modulePermissionController() {
    var self = this;
    var url = "/api/ModulePermissionApi";
    self.ModulePermissions = ko.observableArray([]);
    self.ModulePermission = ko.observable(new ModulePermissionViewModel());
    self.Modules = ko.observableArray();
    self.IsChecked = ko.observable(true);
    getModulePermissions();
    self.CheckAllModules = ko.observable(false);

    self.CheckAllModules = function (item) {
        ko.utils.arrayForEach(item.Modules(), function (module) {
            module.Checked(self.IsChecked());
        });
    }
    function getModulePermissions() {
        Riddha.ajax.get(url)
          .done(function (result) {
              var data = Riddha.ko.global.arrayMap(result.Data, ModulePermissionViewModel);
              self.ModulePermissions(data);
          })
    };
    self.message = ko.observable('');
    self.Save = function () {
        var model = { viewModel: ko.toJS(self.ModulePermissions()) };
        Riddha.ajax.post(url, model)
        .done(function (result) {
            //success after save
            self.message("Module Permission Updated Successfully...");
            Riddha.util.delayExecute(resetMessage, 2000);
        });
    }
    function resetMessage() {
        self.message('');
    }
}

function controllerPermissionController() {
    var self = this;
    var url = "/api/ControllerPermissionApi";
    self.Roles = ko.observableArray(getRoles());
    self.Modules = ko.observableArray([]);
    self.ModuleId = ko.observable();
    self.ControllerPermission = ko.observable(new ControllerPermissionViewModel());
    function getRoles() {
        Riddha.ajax.get("/api/UserRoleApi")
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DropDownViewModel);
            self.Roles(data);

        });
    };
    self.GetModulesByRole = function () {
        Riddha.ajax.get("/api/ModulePermissionApi/GetModulesByRole/" + self.ControllerPermission().RoleId())
       .done(function (result) {
           var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DropDownViewModel);
           self.Modules(data);

       });
    }
    self.GetControllerByModule = function () {
        Riddha.ajax.get("/api/ControllerPermissionApi/GetControllerByModule/" + self.ModuleId() + "?roleId=" + self.ControllerPermission().RoleId())
       .done(function (result) {
           var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ControllerViewModel);
           self.ControllerPermission().Controllers(data);
       });
    };
    self.message = ko.observable('');
    self.Save = function () {
        if (self.ControllerPermission().RoleId() == undefined ||self.ModuleId()==undefined) {
            return Riddha.util.localize.Required("LeaveMaster");
        }
        Riddha.ajax.post(url, ko.toJS(self.ControllerPermission()))
        .done(function (result) {
            //success after save
            self.message("Controller Permission Updated Successfully...");
            Riddha.util.delayExecute(resetMessage, 2000);
        });
    }
    function resetMessage() {
        self.message('');
    }
}

function actionPermissionController() {
    var self = this;
    var url = "/api/ActionPermissionApi";
    self.Roles = ko.observableArray(getRoles());
    self.Modules = ko.observableArray([]);
    self.Controllers = ko.observableArray([]);
    self.ModuleId = ko.observable();
    self.ControllerId = ko.observable();
    self.ActionPermission = ko.observable(new ActionPermissionViewModel());

    function getRoles() {
        Riddha.ajax.get("/api/UserRoleApi")
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DropDownViewModel);
            self.Roles(data);
        });

    };
    self.GetModulesByRole = function () {
        Riddha.ajax.get("/api/ModulePermissionApi/GetModulesByRole/" + self.ActionPermission().RoleId())
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DropDownViewModel);
            self.Modules(data);
        });
    }

    self.GetControllerByModule = function () {
        Riddha.ajax.get("/api/ControllerPermissionApi/GetControllerByModule/" + self.ModuleId() + "?roleId=" + self.ActionPermission().RoleId())
          .done(function (result) {
              var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DropDownViewModel);
              self.Controllers(data);

          });

    }
    self.GetActionByController = function () {
        Riddha.ajax.get("/api/ActionPermissionApi/GetActionByController/" + self.ControllerId() + "?roleId=" + self.ActionPermission().RoleId())
       .done(function (result) {
           var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ActionViewModel);
           self.ActionPermission().Actions(data);
       });
    }

    self.message = ko.observable('');
    self.Save = function () {
        Riddha.ajax.post(url, ko.toJS(self.ActionPermission()))
        .done(function (result) {
            //success after save
            //self.Actions.push(new ActionPermissionViewModel(result.Data));
            self.message("Action Permission Updated Successfully...");
            Riddha.util.delayExecute(resetMessage, 2000);
        });
    }
    function resetMessage() {
        self.message('');
    }
}

function ownerPermissionController() {
    var self = this;
    var url = "/api/ModulePermissionApi";
    self.ModulePermissions = ko.observableArray([]);
    self.ModulePermission = ko.observable(new OwnerPermissionViewModel());
    self.Modules = ko.observableArray();
    self.IsChecked = ko.observable(true);
    getModulePermissions();
    self.CheckAllModules = ko.observable(false);

    self.CheckAllModules = function (item) {
        ko.utils.arrayForEach(item.Modules(), function (module) {
            module.Checked(self.IsChecked());
        });
    }
    function getModulePermissions() {
        Riddha.ajax.get(url + "/GetOwnerPermissions")
          .done(function (result) {
              var data = Riddha.ko.global.arrayMap(result.Data, OwnerPermissionViewModel);
              self.ModulePermissions(data);
          })
    };
    self.message = ko.observable('');
    self.Save = function () {
        var model = { viewModel: ko.toJS(self.ModulePermissions()) };
        Riddha.ajax.post(url + "/SaveOwnerPermission", model)
        .done(function (result) {
            //success after save
            self.message("Owner Permission Updated Successfully...");
            Riddha.UI.Toast("Owner Permission Updated Successfully", 4);
            Riddha.util.delayExecute(resetMessage, 2000);
        });
    }
    function resetMessage() {
        self.message('');
    }
}