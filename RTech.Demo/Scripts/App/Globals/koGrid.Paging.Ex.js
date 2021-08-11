/// <reference path="../../knockout-3.4.2.js" />
/// <reference path="../../knockout-grids.js" />
/*
config:{
pageSize:25,
data:{},
enableServerPaging:false,
jsonUrl:''
}
*/
var riddhaKoGrid = function (config) {
    var self = this;
    self.pageSize = config.pageSize;
    self.init = function () {

    };
    var filterOptions = {
        filterText: config.filterText || ko.observable(""),
        //useExternalFilter: false
    };
    var pagingOptions = {
        pageSizes: ko.observableArray([10, 25, 50, 100, 250, 500, 1000]),
        pageSize: ko.observable(self.pageSize || 10),
        totalServerItems: ko.observable(0),
        currentPage: ko.observable(1)
    };
    var setPagingData = function (data, page) {
        if (config.enableServerPaging == false) {
            var pagedData = data.slice((page - 1) * self.pageSize, page * self.pageSize);
            config.data(pagedData);
        }
        else {
            config.data(data);
        }
        pagingOptions.totalServerItems(config.enableServerPaging == true && data.length > 0 ? data[0].TotalCount : data.length);
    };
    var getPagedDataAsync = function (page, searchText) {
        //setTimeout(function () {
        var data;
        if (searchText) {
            var ft = searchText.toLowerCase();
            $.getJSON(config.jsonUrl + "/?pageSize=" + self.pageSize + "&page=" + page + "&searchText=" + searchText, function (largeLoad) {
                //data = largeLoad.Data.filter(function (item) {
                //    return JSON.stringify(item).toLowerCase().indexOf(ft) != -1;
                //});
                setPagingData(largeLoad.Data, page);
            });
        } else {
            $.getJSON(config.jsonUrl + "/?pageSize=" + self.pageSize + "&page=" + page, function (largeLoad) {
                setPagingData(largeLoad.Data, page);
            });
        }
        //}, 0);
    };
    var delayExecute = function (callBack, time) {
        time = time || 100;
        setTimeout(function () {
            callBack;
        }, time);
    };
    var textInputCount = 0;
    var timer = 0;
    filterOptions.filterText.subscribe(function (data) {
        if (config.enableServerPaging == true) {
            textInputCount++;
            if (timer > 0) {
                clearTimeout(timer);
                timer = 0;

            }
            timer = setTimeout(
                function () {
                    getPagedDataAsync(pagingOptions.currentPage(), filterOptions.filterText())
                }
                , 200);

        }
        else {
        }
    });
    pagingOptions.pageSizes.subscribe(function (data) {
        getPagedDataAsync(pagingOptions.currentPage(), filterOptions.filterText());
    });
    pagingOptions.pageSize.subscribe(function (data) {
        
        self.pageSize = data;
        getPagedDataAsync(pagingOptions.currentPage(), filterOptions.filterText());
    });
    pagingOptions.totalServerItems.subscribe(function (data) {
        //getPagedDataAsync(pagingOptions.currentPage(), filterOptions.filterText());
    });
    pagingOptions.currentPage.subscribe(function (data) {
        getPagedDataAsync(pagingOptions.currentPage(), filterOptions.filterText());
    });

    getPagedDataAsync(pagingOptions.currentPage());
    self.gridOptions = {
        data: config.data,
        columnDefs: config.columnDefs,
        //width:100,
        enableColumnResize: false,
        showColumnMenu: false,
        enablePaging: true,
        showFilter: false,
        showGroupPanel: true,
        multiSelect: false,
        pagingOptions: pagingOptions,
        filterOptions: filterOptions,
        jqueryUITheme: true,
        afterSelectionChange: function (data) {
            var selectedItems = data.selectedItems();
            if (selectedItems.length > 0) {
                if (config.getSelectedItem) {
                    config.getSelectedItem(selectedItems[0]);
                }
                if (config.getSelectedItems) {
                    config.getSelectedItems(selectedItems);
                }
            }
        },
        refresh: function () {
            getPagedDataAsync(pagingOptions.currentPage(), filterOptions.filterText());
        }
    };
    return self.gridOptions;
};
