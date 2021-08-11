/// <reference path="Riddha.Script.Bank.Model.js" />
//Bank
function bankController() {
    var self = this;
    self.Bank = ko.observable(new BankModel());
    self.Banks = ko.observableArray([]);
    self.SelectedBank = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    var url = "/Api/BankApi";

    GetBanks();
    function GetBanks() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), BankGridVm);
            self.Banks(data);
        });
    };

  

    self.CreateUpdate = function () {
        if (self.Bank().Code() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Bank().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Bank()))
            .done(function (result) {
                self.Banks.push(new BankModel(result.Data));
                Riddha.UI.Toast(result.Message, result.Status);
                self.Reset();
                self.CloseModal();
            });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Bank()))
            .done(function (result) {
                self.Banks.replace(self.SelectedBank(), new BankModel(ko.toJS(self.Bank())));
                Riddha.UI.Toast(result.Message, result.Status);
                self.ModeOfButton("Create");
                self.Reset();
                self.CloseModal();
            });
        };
    }
    self.Reset = function () {
        self.Bank(new BankModel({ Id: self.Bank().Id() }));
    };

    self.Select = function (model) {
        self.SelectedBank(model);
        self.Bank(new BankModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (bank) {
        Riddha.UI.Confirm("Delete Confirm", function () {
            Riddha.ajax.delete(url + "/" + bank.Id(), null)
            .done(function (result) {
                self.Banks.remove(bank)
                self.ModeOfButton("Create");
                self.Reset();
                Riddha.UI.Toast(result.Message, result.Status);
            });
        },'',"Delete "+bank.Name())
    };


    self.ShowModal = function () {
        $("#bankCreationModel").modal('show');
    };

    $("#bankCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#bankCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
}