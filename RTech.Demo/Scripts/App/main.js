/// <reference path="../jquery-1.10.2.min.js" />
/// <reference path="../knockout-2.3.0.js" />
/// <reference path="../knockout.validation.js" />


var app = function () {
    var $root = this;
    var Url = {},
         
        models = {
            menu: function (item) {
                item = item || {};
                this.MenuName = ko.observable(item.MenuName || "");
                this.IsGroup = ko.observable(item.IsGroup || false);
                this.Module = ko.observable(item.Module || '');
                this.CssClass = ko.observable(item.CssClass || 'fa fa-files-o');
                this.Click = item.Click || function () { alert('notImplemented') }
                this.Children = ko.observableArray(item.Children || []);
            }
        },
        menu = function () {
            $root.menus = ko.observableArray([{
                MenuName: 'Account',
                IsGroup: false,
                CssClass: 'fa fa-dollar',

                children: [{
                    MenuName: 'Journal',
                    IsGroup: false,
                    CssClass: 'fa fa-dollar',
                    click: function () {
                        
                    },
                    children: [],

                },{
                    MenuName: 'Teller',
                    IsGroup: false,
                    CssClass: 'fa fa-dollar',
                    children: [],

                },{
                    MenuName: 'Report',
                    IsGroup: false,
                    CssClass: 'fa fa-dollar',
                    children: [],

                }]
            }]);
        },
        login = function () {

        },
        logout = function () {

        }
    menu();

}


var currentTab;
var composeCount = 0;
//initilize tabs
$(function () {
    //when ever any tab is clicked this method will be call
    $("#myTab").on("click", "a", function (e) {
        e.preventDefault();
        $(this).tab('show');
        $currentTab = $(this);
    });
    registerComposeButtonEvent();
    registerCloseEvent();
});

//this method will demonstrate how to add tab dynamically
function registerComposeButtonEvent() {
    /* just for this demo */
    //$('#composeButton').click(function (e) {
    $('#sideBarMenu li a').click(function (e) {
        var href = $(this).attr('href');
        var index = $(this).index();
        var name = $(this).text();
        e.preventDefault();
        

        $li = '<li><a href="#' + index + '" data-toggle="tab"><button class="close closeTab" type="button" >×</button>' + name + '</a></li>';
        $('.nav-tabs').append($li);
        $tabpane = '<div class="tab-pane active in"  id="' + index + '">            </div>';
        $('.tab-content').append($tabpane);

        craeteNewTabAndLoadUrl("", href, "#" + index);

        //$(this).tab('show');
        showTab(index);
        registerCloseEvent();
    });

}

//this method will register event on close icon on the tab..
function registerCloseEvent() {

    $(".closeTab").click(function () {

        //there are multiple elements which has .closeTab icon so close the tab whose close icon is clicked
        var tabContentId = $(this).parent().attr("href");
        $(this).parent().parent().remove(); //remove li of tab
        $('#myTab a:last').tab('show'); // Select first tab
        $(tabContentId).remove(); //remove respective tab content

    });
}

//shows the tab with passed content div id..paramter tabid indicates the div where the content resides
function showTab(tabId) {
    $('#myTab a[href="#' + tabId + '"]').tab('show');
}
//return current active tab
function getCurrentTab() {
    return currentTab;
}

//This function will create a new tab here and it will load the url content in tab content div.
function craeteNewTabAndLoadUrl(parms, url, loadDivSelector) {
    $(loadDivSelector).load(url, function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error getting details ! ";

        }
    });
}
//this will return element from current tab
//example : if there are two tabs having  textarea with same id or same class name then when $("#someId") whill return both the text area from both tabs
//to take care this situation we need get the element from current tab.
function getElement(selector) {
    var tabContentId = $currentTab.attr("href");
    return $("" + tabContentId).find("" + selector);

}


function removeCurrentTab() {
    var tabContentId = $currentTab.attr("href");
    $currentTab.parent().remove(); //remove li of tab
    $('#myTab a:last').tab('show'); // Select first tab
    $(tabContentId).remove(); //remove respective tab content
}