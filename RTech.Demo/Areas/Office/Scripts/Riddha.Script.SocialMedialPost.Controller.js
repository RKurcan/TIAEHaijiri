/// <reference path="riddha.script.socialmedialpost.model.js" />


function socialMediaPostController() {
    var self = this;
    self.SocialMediaPost = ko.observable(new SocialMediaPostModel());
    self.SocialMediaPosts = ko.observableArray([]);
    self.SelectedSocialMediaPost = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    var url = "/Api/SocialMediaPostApi";

    GetSocialMediaPosts();
    function GetSocialMediaPosts() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), SocialMediaPostModel);
                self.SocialMediaPosts(data);
            });
    };



    self.CreateUpdate = function () {
        if (self.SocialMediaPost().Title() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.SocialMediaPost().Message() == "") {
            return Riddha.util.localize.Required("Message");
        }
        if (self.SocialMediaPost().PhotoURL() == "") {
            return Riddha.util.localize.Required("Photo");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.SocialMediaPost()))
                .done(function (result) {
                    GetSocialMediaPosts();
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.SocialMediaPost()))
                .done(function (result) {
                    GetSocialMediaPosts();
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.CloseModal();
                });
        };
    }
    self.Reset = function () {
        self.SocialMediaPost(new SocialMediaPostModel({ Id: self.SocialMediaPost().Id() }));
    };

    self.Select = function (model) {
        self.SelectedSocialMediaPost(model);
        self.SocialMediaPost(new SocialMediaPostModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (socialMediaPost) {
        Riddha.UI.Confirm("Delete Confirm", function () {
            Riddha.ajax.delete(url + "/" + socialMediaPost.Id(), null)
                .done(function (result) {
                    GetSocialMediaPosts();
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }, '', "Delete " + socialMediaPost.Title())
    };


    self.ShowModal = function () {
        $("#socialMediaPostCreationModel").modal('show');
    };

    $("#socialMediaPostCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
        self.ModeOfButton("Create");
    });

    self.CloseModal = function () {
        $("#socialMediaPostCreationModel").modal('hide');
        self.Reset();
        self.ModeOfButton("Create");
    };
}