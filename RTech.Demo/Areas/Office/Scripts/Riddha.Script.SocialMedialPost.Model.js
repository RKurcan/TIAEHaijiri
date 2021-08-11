

function SocialMediaPostModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Title = ko.observable(item.Title || '');
    self.Message = ko.observable(item.Message || '');
    self.PhotoURL = ko.observable(item.PhotoURL || '');
    self.Publish = ko.observable(item.Publish || false);
    self.PublishDuration = ko.observable(item.PublishDuration || 0);
}