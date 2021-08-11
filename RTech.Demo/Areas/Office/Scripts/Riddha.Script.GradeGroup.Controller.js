
/// <reference path="Riddha.Script.GradeGroup.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />


function gradeGroupController() {
    var self = this;
    var config = new Riddha.config();
    var url = "/Api/GradeGroupApi";
    self.GradeGroup = ko.observable(new GradeGroupModel());
    self.GradeGroups = ko.observableArray([]);
    self.SelectedGradeGroup = ko.observable();
    self.ModeOfButton = ko.observable('Create');

    GetGradeGroups();
    function GetGradeGroups() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GardeGroupGridVm);
            self.GradeGroups(data);
        });
    };


    self.CreateUpdate = function () {

        if (self.GradeGroup().Code() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.GradeGroup().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.GradeGroup()))
            .done(function (result) {
                if (result.Status == 4) {
                    self.GradeGroups.push(new GradeGroupModel(result.Data));
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.GradeGroup()))
            .done(function (result) {
                if (result.Status == 4) {
                    self.GradeGroups.replace(self.SelectedGradeGroup(), new GradeGroupModel(ko.toJS(self.GradeGroup())));
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };

    self.Select = function (model) {
        self.SelectedGradeGroup(model);
        self.GradeGroup(new GradeGroupModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };


    self.Reset = function () {
        self.GradeGroup(new GradeGroupModel({ Id: self.GradeGroup().Id() }));
        self.ModeOfButton("Create");
    };

    self.Delete = function (GradeGroup) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + GradeGroup.Id(), null)
            .done(function (result) {
                if (result.Status == 4) {
                    self.GradeGroups.remove(GradeGroup);
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    self.ShowModal = function () {
        $("#gradeGroupCreationModel").modal('show');
    };

    $("#gradeGroupCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });


    self.CloseModal = function () {
        $("#gradeGroupCreationModel").modal('hide');
        self.Reset();
    };



}
