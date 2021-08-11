/// <reference path="Riddha.Script.Bank.Model.js" />
/// <reference path="Riddha.Script.PayrollConfiguration.Model.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />

function payRollConfigurationController() {
    var self = this;
    var url = "/Api/PayrollConfigurationApi";
    var config = new Riddha.config();
    self.PayRollConfiguration = ko.observable(new PayrollConfigurationModel());
    self.PayRollConfigurations = ko.observableArray([]);
    self.SelectedPayRollConfiguration = ko.observable();
    self.ModeOfButton = ko.observable('Create');

    getPayRollConfig();
    function getPayRollConfig() {
        Riddha.ajax.get(url + "/GetPayrollConfig")
        .done(function (result) {
            self.PayRollConfiguration(new PayrollConfigurationModel(result.Data));
            debugger;
            self.PayRollConfiguration().PresentInHoliday(result.Data.PresentInHoliday.toString());
            self.PayRollConfiguration().PresentInDayOff(result.Data.PresentInDayOff.toString());
            if (self.PayRollConfiguration().Id() > 0) {
                self.ModeOfButton('Update');
            }
        })
    };
    self.CreateUpdate = function () {
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.PayRollConfiguration()))
            .done(function (result) {
                Riddha.UI.Toast(result.Message, result.Status);
                getPayRollConfig();
            });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.PayRollConfiguration()))
            .done(function (result) {
                Riddha.UI.Toast(result.Message, result.Status);
                getPayRollConfig();
            });
        };
    }

    self.Reset = function () {
        self.PayRollConfiguration(new PayrollConfigurationModel({ Id: self.PayRollConfiguration().Id() }));
    };
}