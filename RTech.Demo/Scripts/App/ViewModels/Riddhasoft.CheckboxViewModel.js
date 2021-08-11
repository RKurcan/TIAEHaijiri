function CheckboxViewModel() {
    var self = this;
    var a = "Indicates a warning that might need attention.";
    self.Items = ko.observableArray([
            {Id:1,Name: 'Choice 1' },
            { Id: 2, Name: 'Choice 2' },
            { Id: 3, Name: 'Choice 3' },
            { Id: 4, Name: 'Choice 4' }
    ]);
    self.SelectedItems = ko.observableArray([]);
    self.Details = ko.observable("<div class='alert alert-warning' role='alert'><strong>Warning!</strong>" + a + "</div>");
    self.Toggle = function() {
        Riddha.UI.Alert.Message(3,"Name Field is Required",);
    }
    
}
ko.applyBindings(new CheckboxViewModel());