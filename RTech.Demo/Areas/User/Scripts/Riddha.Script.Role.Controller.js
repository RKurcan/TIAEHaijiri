/// <reference path="../../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.User.Model.js" />

function roleController() {
    var self = this;
    self.Controllers = ko.observableArray([{ Id: 0, Name: "Country", Checked: false }, { Id: 1, Name: "State", Checked: false }, { Id: 2, Name: "District", Checked: false }, { Id: 3, Name: "Territory", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }, { Id: 0, Name: "Country", Checked: false }]);
    self.Roles = ko.observableArray(getRoles());
    self.Role = ko.observable(new RoleOnControllerModel());
    self.SelectedRole = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    var url = "/api/RoleOnApi";

    self.GetControllerName = function (id) {
        var mapped = ko.utils.arrayFirst(self.Controllers(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    }
    //function getRoles() {
    //    Riddha.ajax.get("/api/UserRoleApi")
    //      .done(function (result) {
    //          var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), UserRoleModel);
    //          self.Roles(data);
    //      })
    //};

    function getRoles() {
        Riddha.ajax.get(url)
          .done(function (result) {
              var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), UserRoleGridVm);
              self.Roles(data);
          })
    };



    self.CheckAllRoles = ko.observable(false);
    self.CheckAllRoles.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Controllers(), function (item) {
            item.Checked(newValue);
        });
    });

    self.CreateUpdate = function () {
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Role()))
            .done(function (result) {
                self.Roles.push(new RoleOnControllerModel(result.Data));
            });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Role()))
            .done(function (result) {
                self.Roles.replace(self.SelectedRole(), new RoleOnControllerModel(ko.toJS(self.Role())));
                self.ModeOfButton("Create");
            });
        }
    };



}