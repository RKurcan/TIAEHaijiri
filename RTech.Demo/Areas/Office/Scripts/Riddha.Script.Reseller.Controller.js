/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Reseller.Model.js" />

//Reseller
function resellerController() {
    var self = this;
    var url = "/Api/ResellerApi";
    self.Reseller = ko.observable(new ResellerModel());
    self.Resellers = ko.observableArray([]);
    self.SelectedReseller = ko.observable();
    self.UserName = ko.observable('');
    self.Password = ko.observable('');
    self.ModeOfButton = ko.observable('Submit');
    self.filterText = ko.observable("");
    self.Captcha = ko.observable("");
    self.ResellerEmailConfirm = ko.observable('');

    //self.SearchReseller = function () {
    //    GetResellers();
    //}

    //GetResellers();
    //function GetResellers() {
    //    //Riddha.ajax.get(url)
    //    Riddha.ajax.get(url + "?searchText=" + self.filterText())
    //        .done(function (result) {
    //            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ResellerGridVm);
    //            self.Resellers(data);
    //        });
    //};


    self.KendoGridOptions = {
        title: "Reseller",
        target: "#resellerKendoGrid",
        url: "/Api/ResellerApi/GetKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        selectable: true,
        group: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Name', title: "Name", width: 180, filterable: true },
            { field: 'Address', title: "Address", width: 160, filterable: true },
            { field: 'ContactNo', title: "Contact No", width: 150, filterable: true },
            { field: 'ContactPerson', title: "Contact Person", filterable: true },
            { field: 'Email', title: "Email", filterable: true },
            { field: 'PAN', title: "PAN", filterable: true },
            { field: 'Status', title: "Status", filterable: true, template: "#=GetResellerStatus(Status)#" },
        ],
        SelectedItem: function (item) {
            self.SelectedReseller(new ResellerGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedReseller(new ResellerGridVm());
        $("#resellerKendoGrid").getKendoGrid().dataSource.read();
    };

    self.CheckDuplicateSNo = function (item, event) {
        Riddha.ajax.get(url + "/CheckDuplicateSNo/?Code=" + item.Code())
            .done(function (result) {
                if (result == true) {
                    //user toast using process   
                    Riddha.UI.Toast("Code already exist!!!", Riddha.CRM.Global.ResultStatus.processError);
                    return self.Reseller().Code("");
                }
            })
    }

    self.CheckDuplicateEmail = function (item, event) {
        var resx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!resx.test(item.Email())) {
            Riddha.UI.Toast("Invalid Emial Address!!!", Riddha.CRM.Global.ResultStatus.processError);
            return self.Reseller().Email("");
        }
        Riddha.ajax.get(url + "/CheckDuplicateEmail/?Email=" + item.Email())
            .done(function (result) {
                if (result == true) {
                    Riddha.UI.Toast("Email already exist!!!", Riddha.CRM.Global.ResultStatus.processError);
                    return self.Reseller().Email("");
                }
            })
    }
    self.IsSubmitClick = ko.observable(false);
    self.CreateUpdate = function () {

        //if (self.Reseller().Code() == "") {
        //    return Riddha.UI.Toast("Please enter code!!!", Riddha.CRM.Global.ResultStatus.processError);
        //}
        if (self.Reseller().Name.hasError()) {
            return Riddha.UI.Toast("Partner name is required", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Reseller().Address.hasError()) {
            return Riddha.UI.Toast(self.Reseller().Address.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Reseller().ContactNo.hasError()) {
            return Riddha.UI.Toast(self.Reseller().ContactNo.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Reseller().ContactPerson() == "") {
            return Riddha.UI.Toast("Contact Person is Requierd", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Reseller().Email() == "") {
            return Riddha.UI.Toast("Email Address is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().WebUrl() == "") {
            return Riddha.UI.Toast("Web Address is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().PAN() == "") {
            return Riddha.UI.Toast("PAN number is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().CompanyRegistrationNo() == "") {
            return Riddha.UI.Toast("Company Registration number is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().CRDUrl() == "") {
            return Riddha.UI.Toast("Company Registration Document is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().PANVATUrl() == "") {
            return Riddha.UI.Toast("PAN/VAT Document is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.ModeOfButton() == 'Submit') {
            self.IsSubmitClick(true);
            var data = { Reseller: self.Reseller(), UserName: self.UserName(), Password: self.Password(), Captcha: self.Captcha };
            Riddha.ajax.post(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        GetResellers();
                        self.Reset();
                        self.CloseModal();
                        self.IsSubmitClick(false);
                        self.ClosePopupModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            var data = { Reseller: self.Reseller(), UserName: self.UserName(), Password: self.Password() };
            Riddha.ajax.put(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        GetResellers();
                        self.ModeOfButton("Submit");
                        self.Reset();

                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Reset = function () {
        self.Reseller(new ResellerModel({ Id: self.Reseller().Id() }));
        self.UserName([]);
        self.Password([]);
    };

    self.Select = function (model) {
        if (self.SelectedReseller() == undefined) {
            Riddha.UI.Toast("Please select partner to edit.", 0);
            return;
        };
        Riddha.ajax.get(url + "/?id=" + self.SelectedReseller().Id())
            .done(function (result) {
                self.Reseller(new ResellerModel(result.Data.Reseller));
                self.UserName(result.Data.UserName);
                self.Password(result.Data.Password);
                self.ModeOfButton('Update');
                self.ShowModal();
            })
    };


    self.Delete = function (reseller, e) {
        if (self.SelectedReseller() == undefined) {
            Riddha.UI.Toast("Please select partner to delete.", 0);
            return;
        };
        Riddha.UI.Confirm("Confirm to Delete this reseller?", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedReseller().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast(result.Message, result.Status);
                        self.Reset();
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ApproveSuspend = function (item) {
        if (self.SelectedReseller() == undefined) {
            Riddha.UI.Toast("Please select partner to delete.", 0);
            return;
        };
        Riddha.ajax.get(url + "/Suspend" + "/?id=" + self.SelectedReseller().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            })
    };

    self.ShowModal = function () {
        $("#resellerCreationModel").modal('show');
    };

    $("#resellerCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#resellerCreationModel").modal('hide');
        self.ModeOfButton("Submit");
    };

    self.ClosePopupModal = function () {
        $("#becomePatnerModel").modal('hide');
        self.ModeOfButton("Submit");
        self.Reset();
    };

    $("#becomePatnerModel").on('hidden.bs.modal', function () {
        self.Reset();
    });


    self.downloadCRD = function () {
        var base = self.Reseller().CRDUrl();
        Riddha.util.viewBase64(base);
        // Tell the browser to save as report.txt.
        //saveAs(blob, "CRD.txt");
    }

    self.NextTab = function myfunction() {
        $('#btnNext').click(function () {
            $('#tabList > .active').next('li').find('a').trigger('click');
        });
    }


    self.PreviousTab = function () {
        $('#btnPrevious').click(function () {
            $('#tabList > .active').prev('li').find('a').trigger('click');
        });
    }

    self.ShowResellerForgetPasswordModal = function () {
        $("#forgetResellerPasswordModal").modal('show');
    }
    self.CloseResellerForgetPasswordModal = function () {
        $("#forgetResellerPasswordModal").modal('hide');
    }
    self.ConfirmResellerEmail = function () {
        Riddha.ajax.get(url + "/ConfirmResellerEmail?email=" + self.ResellerEmailConfirm())
            .done(function (result) {

            })
    }
}

function GetResellerStatus(data) {
    if (data == 'New') {
        return "<span class='badge bg-aqua'>" + "New" + "</span>";
    }
    if (data == 'In Activation') {
        return "<span class='badge bg-blue'>" + "In Activation" + "</span>";
    }
    if (data == 'Suspended') {
        return "<span class='badge bg-red'>" + "Suspended" + "</span>";
    }
    else {
        return "<span class='badge bg-green'>" + data + "</span>";
    }
};

function portalPageController() {
    var self = this;
    var url = "/Api/ResellerApi";
    var demoUrl = "/Api/DemoRequestApi";
    var userUrl = "/Api/UserApi"
    self.Reseller = ko.observable(new ResellerModel());
    self.FacebookPost = ko.observable(new FacebookPostModel());
    self.Resellers = ko.observableArray([]);
    self.SelectedReseller = ko.observable();
    self.UserName = ko.observable('');
    self.Password = ko.observable('');
    self.ModeOfButton = ko.observable('Submit');
    self.filterText = ko.observable("");
    self.Captcha = ko.observable("");
    self.ResellerEmailConfirm = ko.observable('');
    self.UserPasswordReset = ko.observable(new UserPasswordReset());

    self.CheckDuplicateEmail = function (item, event) {
        var resx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!resx.test(item.Email())) {
            Riddha.UI.Toast("Invalid Emial Address!!!", Riddha.CRM.Global.ResultStatus.processError);
            return self.Reseller().Email("");
        }
        Riddha.ajax.get(url + "/CheckDuplicateEmail/?Email=" + item.Email())
            .done(function (result) {
                if (result == true) {
                    Riddha.UI.Toast("Email already exist!!!", Riddha.CRM.Global.ResultStatus.processError);
                    return self.Reseller().Email("");
                }
            })
    }
    self.IsSubmitClick = ko.observable(false);

    self.CreateUpdate = function () {
        if (self.Reseller().Name.hasError()) {
            return Riddha.UI.Toast("Partner name is required", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Reseller().Address.hasError()) {
            return Riddha.UI.Toast(self.Reseller().Address.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Reseller().ContactNo.hasError()) {
            return Riddha.UI.Toast(self.Reseller().ContactNo.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Reseller().ContactPerson() == "") {
            return Riddha.UI.Toast("Contact Person is Requierd", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Reseller().Email() == "") {
            return Riddha.UI.Toast("Email Address is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().WebUrl() == "") {
            return Riddha.UI.Toast("Web Address is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().PAN() == "") {
            return Riddha.UI.Toast("PAN number is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().CompanyRegistrationNo() == "") {
            return Riddha.UI.Toast("Company Registration number is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().CRDUrl() == "") {
            return Riddha.UI.Toast("Company Registration Document is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.Reseller().PANVATUrl() == "") {
            return Riddha.UI.Toast("PAN/VAT Document is Requierd", Riddha.CRM.Global.ResultStatus.processError)
        }
        if (self.ModeOfButton() == 'Submit') {
            self.IsSubmitClick(true);
            var data = { Reseller: self.Reseller(), UserName: self.UserName(), Password: self.Password(), Captcha: self.Captcha };
            Riddha.ajax.post(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Reset();
                        self.CloseModal();
                        self.IsSubmitClick(false);
                        self.ClosePopupModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            var data = { Reseller: self.Reseller(), UserName: self.UserName(), Password: self.Password() };
            Riddha.ajax.put(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.ModeOfButton("Submit");
                        self.Reset();

                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Reset = function () {
        self.Reseller(new ResellerModel({ Id: self.Reseller().Id() }));
        self.UserName([]);
        self.Password([]);
    };

    self.Select = function (model) {
        self.SelectedReseller(model);
        Riddha.ajax.get(url + "/?id=" + model.Id())
            .done(function (result) {
                self.Reseller(new ResellerModel(result.Data.Reseller));
                self.UserName(result.Data.UserName);
                self.Password(result.Data.Password);
                self.ModeOfButton('Update');
                self.ShowModal();
            })
    };

    self.Delete = function (reseller, e) {

        var target = $(e.target);
        var tr = {};
        if (target.is('span')) {
            tr = $(e.target).parent().parent().parent().parent();
        }
        else {
            tr = $(e.target).parent().parent().parent();
        }

        Riddha.UI.Confirm("Confirm to Delete this reseller?", function () {
            Riddha.ajax.delete(url + "/" + reseller.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast(result.Message, result.Status);
                        self.Reset();
                        tr.fadeOut(1000);
                        return;
                        self.Resellers.remove(reseller)
                        self.ModeOfButton("Submit");

                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ApproveSuspend = function (item) {

        //if (item.Status() == 'New') {
        //    return Riddha.UI.Toast("The login account is not registered for this Reseller", Riddha.CRM.Global.ResultStatus.processError);
        //}
        Riddha.ajax.get(url + "/Suspend" + "/?id=" + item.Id())
            .done(function (result) {
                if (result.Status == 4) {
                    item.Status(result.Data);
                    self.Resellers.replace(self.SelectedReseller(), new ResellerModel(ko.toJS(item)));
                }
                Riddha.UI.Toast(result.Message, result.Status);
            })
    };

    self.ShowModal = function () {
        $("#resellerCreationModel").modal('show');
    };

    $("#resellerCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#resellerCreationModel").modal('hide');
        self.ModeOfButton("Submit");
    };

    self.ClosePopupModal = function () {
        $("#becomePatnerModel").modal('hide');
        self.ModeOfButton("Submit");
        self.Reset();
    };

    $("#becomePatnerModel").on('hidden.bs.modal', function () {
        self.Reset();
    });


    self.downloadCRD = function () {
        var base = self.Reseller().CRDUrl();
        Riddha.util.viewBase64(base);
        // Tell the browser to save as report.txt.
        //saveAs(blob, "CRD.txt");
    }

    self.NextTab = function myfunction() {
        $('#btnNext').click(function () {
            $('#tabList > .active').next('li').find('a').trigger('click');
        });
    }


    self.PreviousTab = function () {
        $('#btnPrevious').click(function () {
            $('#tabList > .active').prev('li').find('a').trigger('click');
        });
    }

    self.ShowResellerForgetPasswordModal = function () {
        $("#forgetResellerPasswordModal").modal('show');
    }
    self.CloseResellerForgetPasswordModal = function () {
        $("#forgetResellerPasswordModal").modal('hide');
        self.ResellerEmailConfirm('');
    }
    self.IsResellerResetPWClick = ko.observable(false);
    self.ResetResellerPassword = function () {
        if (self.ResellerEmailConfirm().trim() == "") {
            return Riddha.UI.Toast("Email is Required", 0);
        }
        self.IsResellerResetPWClick(true);
        Riddha.ajax.get(url + "/ResetResellerPassword?email=" + self.ResellerEmailConfirm())
            .done(function (result) {
                if (result.Status == 4) {
                    self.CloseResellerForgetPasswordModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
                self.IsResellerResetPWClick(false);
            })
    }

    //User forgot password
    self.ShowUserForgetPasswordModal = function () {
        $("#forgetUserPasswordModal").modal('show');
    }

    self.IsUserResetPWClick = ko.observable(false);
    self.ResetUserPassword = function () {
        if (self.UserPasswordReset().CompanyCode().trim() == "") {
            return Riddha.UI.Toast("Email is Required", 0);
        }
        if (self.UserPasswordReset().Email().trim() == "") {
            return Riddha.UI.Toast("Email is Required", 0);
        }
        self.IsUserResetPWClick(true);
        Riddha.ajax.get(userUrl + "/ResetUserPassword?companycode=" + self.UserPasswordReset().CompanyCode() + "&email=" + self.UserPasswordReset().Email() + "&username=" + self.UserPasswordReset().Username())
            .done(function (result) {
                if (result.Status == 4) {
                    self.ResetUserForgotPassword();
                    self.CloseUserForgetPasswordModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
                self.IsUserResetPWClick(false);
            })
    }

    self.ResetUserForgotPassword = function () {
        self.UserPasswordReset(new UserPasswordReset())
    }
    self.CloseUserForgetPasswordModal = function () {
        $("#forgetUserPasswordModal").modal('hide');
    }

    //Demo Request
    self.DemoRequest = ko.observable(new DemoRequestModel());
    self.DemoRequests = ko.observableArray([]);

    getDemoRequests();

    function getDemoRequests() {
        Riddha.ajax.get(demoUrl)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DemoRequestModel);
                self.DemoRequests(data);
            });
    }
    self.SubmitDemoRequest = function () {
        if (self.DemoRequest().Name().trim() == "") {
            Riddha.UI.Toast("Name is required", 0);
        }
        if (self.DemoRequest().Address().trim() == "") {
            Riddha.UI.Toast("Name is required", 0);
        }
        if (self.DemoRequest().ContactNo().trim() == "") {
            Riddha.UI.Toast("Name is required", 0);
        }
        if (self.DemoRequest().Email().trim() == "") {
            Riddha.UI.Toast("Name is required", 0);
        }
        Riddha.ajax.post(demoUrl, self.DemoRequest())
            .done(function (result) {
                if (result.Status == 4) {
                    self.ResetDemoRequest();
                    self.CloseDemoRequestPopupModal();
                    $.sweetModal({
                        content: 'All done. Please check your email for demo login credentials.',
                        icon: $.sweetModal.ICON_SUCCESS
                    });
                }
            });
    }

    self.ResetDemoRequest = function () {
        self.DemoRequest(new DemoRequestModel());
    }
    self.ShowDemoRequestPopupModal = function () {
        $("#requestDemoModal").modal('show')
    }

    self.CloseDemoRequestPopupModal = function () {
        self.ResetDemoRequest();
        $("#requestDemoModal").modal('hide')
    }
    self.ShowSuccessModal = function () {
        $("#demoRequestSuccessModal").modal("show");
    }

    self.RemoveDemoRequest = function (item) {
        Riddha.UI.Confirm("Confirm to Delete ?", function () {
            Riddha.ajax.delete(demoUrl + "/" + item.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Reset();
                        self.DemoRequests.remove(item);
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    }
    //region ads

    GetFacebookPost();
    function GetFacebookPost() {
        Riddha.ajax.get("/Api/SocialMediaPostApi/GetPublisPost", null)
            .done(function (result) {
                if (result.Status == 4) {
                    if (result.Data != null) {
                        var durationInSecond = 5;
                        if (result.Data.PublishDuration != 0) {
                            durationInSecond = result.Data.PublishDuration;
                        }
                        var durationInMiliSec = (durationInSecond * 1000);
                        $("#facebookShareModal").modal('show')
                        self.FacebookPost(new FacebookPostModel(result.Data));
                        setTimeout(function () {
                            $("#facebookShareModal").modal("hide");
                        }, durationInMiliSec);
                    }
                }
            })
    }

    self.ShowDemoRequestPopupModal = function () {

    }
    self.ShowDemoRequestPopupModal();
}