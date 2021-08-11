/// <reference path="../../Scripts/knockout-3.4.2.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.DateConversion.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.Backend.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddhasoft.Home.Model.js" />
function userHomeController() {
    var self = this;
    var url = "/Api/HomeApi"
    var config = new Riddha.config();
    var userId = config.UserId;
    var language = config.CurrentLanguage;
    self.Days = ko.observableArray([]);
    self.InTime = ko.observableArray([]);
    self.OutTime = ko.observableArray([]);
    self.AttendanceInfo = ko.observable();
    self.UpcommingNews = ko.observableArray([]);
    self.AttendanceInfoLoading = ko.observable(true);
    self.Menus = ko.observableArray([]);
    self.Devices = ko.observableArray([]);
    self.ActiveDeviceCount = ko.observable('0');
    self.dashBoardDivData = ko.observableArray([]);
    self.CurrentUserAttendance = ko.observable();
    self.CurrentUserAttendanceSummary = ko.observable();
    self.EmpDOBAndJoinInfos = ko.observableArray([]);
    if (Riddha.global.permission.validateAction("6001"))
        getCurrentUserAttendanceInfo();
    if (Riddha.global.permission.validateAction("6004"))
        getCurrentUserAttendanceSummary();
    if (Riddha.global.permission.validateAction("6001"))
        getAttendanceInfo();
    if (Riddha.global.permission.validateAction("6003"))
        getUpcommingNews();
    GetEmpDOBAndJoin();
    self.AllDates = ko.observableArray();
    self.MaxTime = ko.observable("00:00:00");
    self.MinTime = ko.observable("24:00:00");


    function GetEmpDOBAndJoin() {
        Riddha.ajax.get(url + "/GetEmpDOBAndJoin")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmpDOBAndJoinModel);
                self.EmpDOBAndJoinInfos(data);
            });
    };
    function getAttendanceInfo() {
        Riddha.ajax.get(url + "/GetAttendanceInfo")
            .done(function (result) {
                self.AttendanceInfo(new AttendanceInfoModel(result.Data));

                //EmpList
                self.dashBoardDivData(result.Data.EmpList);
                //Devices
                var devices = Riddha.ko.global.arrayMap(ko.toJS(result.Data.Devices), CompanyDeviceGridVm);
                self.Devices(devices);
                var activeDeviceCount = 0;
                devices.forEach(function (device) {
                    if (device.IsOnline()) {
                        activeDeviceCount += 1;
                    }
                });
                self.ActiveDeviceCount(activeDeviceCount);
            });
    }
    //getDashBoardDivData();

    self.getWeekAttendance = function () {

        Riddha.ajax.get(url + "/GetWeekAttendance?userId=" + userId)
            .done(function (result) {
                result.Data.forEach(function (item) {
                    self.Days.push(item.Date);
                    if (item.InTime == "00:00") {
                        self.InTime.push(null);
                    } else {
                        self.InTime.push(item.InTime)
                        self.AllDates.push(item.InTime);
                    }
                    if (item.OutTime == "00:00") {
                        self.OutTime.push(null);
                    } else {
                        self.OutTime.push(item.OutTime)
                        self.AllDates.push(item.OutTime);
                    }
                });
                compareTime();
                LineChart();
            })
    }

    function compareTime() {
        if (self.AllDates().length == 0) {
            self.MinTime("10:00:00");
            self.MaxTime("16:00:00");
        } else {
            self.AllDates().forEach(function (time) {
                var t = time + ":00";
                if (t < self.MinTime()) {
                    self.MinTime(t);
                }
                if (t > self.MaxTime()) {
                    self.MaxTime(t);
                }
            });
        }
    }
    function getCurrentUserAttendanceInfo() {
        Riddha.ajax.get(url + "/GetCurrentUserAttendanceInfo?userId=" + userId)
            .done(function (result) {
                self.CurrentUserAttendance(new CurrentUserAttendanceModel(result.Data));
            });
    };

    function getEmployee() {
        Riddha.ajax.get()
    }

    function getCurrentUserAttendanceSummary() {
        Riddha.ajax.get(url + "/GetCurrentUserAttendanceSummary?userId=" + userId)
            .done(function (result) {
                self.CurrentUserAttendanceSummary(new CurrentUserAttendanceModel(result.Data));
            });
    };

    function ShowChart() {
        var pieChartCanvas = $("#userChart").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {

                value: self.AttendanceInfo().PresentCount(),
                color: "#00C0EF",
                highlight: "#00C0EF",
                label: "Present"
            },
            {
                value: self.AttendanceInfo().AbsentCount(),
                color: "#DD4B39",
                highlight: "#DD4B39",
                label: "Absent"
            },
            {
                value: self.AttendanceInfo().LateInCount(),
                color: "#00A65A",
                highlight: "#00A65A",
                label: "Late In"
            },
            {
                value: self.AttendanceInfo().OnLeaveCount(),
                color: "#F39C12",
                highlight: "#F39C12",
                label: "On Leave"
            },
            {
                value: self.AttendanceInfo().OfficeVisitCount(),
                color: "#0073b7",
                highlight: "#0073b7",
                label: "Office Visit"
            }

        ];
        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,

            //String - The colour of each segment stroke
            segmentStrokeColor: "#ccc",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 1,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 35, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 150,
            //String - Animation easing effect
            animationEasing: 'easeInQuad',//"easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: true,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: false,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=label.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>",
            //String - A tooltip template
            tooltipTemplate: "<%=label%>: <%=value %>  Employee"
        };
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);
    }

    function LineChart() {
        const ctx = document.getElementById('myChart').getContext('2d');

        let days = self.Days();//["2018-03-22", "2018-03-23", "2018-03-24", "2018-03-25", "2018-03-26", "2018-03-27", "2018-03-28"];

        let inTime = self.InTime();//["10:46:07", null, "10:55:26", "10:14:58", "10:54:55", null, "10:28:29"];

        let outTime = self.OutTime(); //["17:46:07", null, "17:55:26", "17:14:58", "17:54:55", "17:54:04", "17:28:29"];
        let data = days.map((day, index) => ({
            x: day,
            y: moment('1970-02-01 ' + inTime[index]).valueOf()
            //y:moment.utc(inTime[index], "HH:mm").valueOf()
        }));

        let data2 = days.map((day, index) => ({
            x: day,
            y: moment('1970-02-01 ' + outTime[index]).valueOf()
            //y:moment.utc(outTime[index], "HH:mm").valueOf()
        }));

        let myChart = new Chart(ctx, {
            type: 'line',
            data:
            {
                datasets: [
                    {
                        label: "In",
                        backgroundColor: '#3cba9f',
                        fill: false,
                        pointBackgroundColor: '#3cba9f',
                        data: data,
                        pointBorderWidth: 2,
                        pointRadius: 5,
                        pointHoverRadius: 7
                    },
                    {
                        label: "Out",
                        backgroundColor: '#c45850',
                        fill: false,
                        pointBackgroundColor: '#c45850',
                        data: data2,
                        pointBorderWidth: 2,
                        pointRadius: 5,
                        pointHoverRadius: 7
                    }
                ]
            },
            options: {
                scales: {
                    xAxes: [
                        {
                            type: 'time',
                            position: 'bottom',
                            time: {
                                unit: 'day'
                            },
                            ticks: {
                                callback: function (label, index, labels) {
                                    var date = new Date(label).toLocaleDateString('hi-In');
                                    var formattedDate = date.split('/').reverse().join('/');
                                    return SuitableDate(formattedDate);
                                }
                            },
                            //scaleLabel: function (valuePayload) {
                            //    alert(valuePayload)
                            //    //return Number(valuePayload.value).toFixed(2).replace('.',',') + '$';
                            //}
                        }
                    ],
                    yAxes: [
                        {
                            type: 'linear',
                            position: 'left',
                            ticks: {
                                min: moment('1970-02-01 ' + self.MinTime()).valueOf(),
                                max: moment('1970-02-01 ' + self.MaxTime()).valueOf(),
                                //stepSize: 3.6e+20,
                                beginAtZero: false,
                                callback: function (value) {
                                    var date = moment(value);
                                    if (date.diff(moment('1970-02-01 23:59:59'), 'minutes') === 0) {
                                        return null;
                                    }

                                    return date.format('h A');
                                }
                            }
                        }
                    ]
                },

                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            return moment("/Date(" + tooltipItem.yLabel + ")/").format('h:mm a')
                            //return data.datasets[tooltipItem.datasetIndex].label + ': ' + epoch_to_hh_mm_ss(tooltipItem.yLabel)
                        }
                    }

                }
            }
        });
        function epoch_to_hh_mm_ss(epoch) {
            return new Date(epoch * 1000).toISOString().substr(12, 7)
        }
    }

    function getUpcommingNews() {
        Riddha.ajax.get(url + "/GetUpcommingNews")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, NewsModel);
                self.UpcommingNews(data);
            });
    };



    self.Type = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सार्वजनिक बिदा" : "Holiday" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "इभेन्ट" : "Event" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "सूचना" : "Notice" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "बिदा" : "Leave" },
    ]);

    self.GetTypeName = function (id) {
        var mapped = ko.utils.arrayFirst(self.Type(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };
    var docHeight = $(window).height();

    self.KendoGridOptions = function (title, type, target) {
        var record = 0;
        return {
            title: title,
            target: target,
            url: "/Api/HomeApi/GetAttendanceInfoDetails",
            paramData: { type: type },
            width: '70%',
            height: docHeight -20,
            multiSelect: false,
            maximize: true,
            groupParam: { field: "DepartmentName" },
            group: false,
            pageSize: 50,
            columns: [
                { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
                { field: 'EmployeeCode', title: language == "ne" ? "कर्मचारी कोड" : "Employee Code", template: "#=SuitableNumber(EmployeeCode)#" },
                { field: 'EmployeeName', title: language == "ne" ? "कर्मचारीको नाम" : "Employee Name" },
                { field: 'DepartmentName', title: language == "ne" ? "विभाग" : "Department" },
                { field: 'PlannedTimeIn', title: language == "ne" ? "तोकीएको आउने समय " : "Planned In", filterable: false, sortable: false, template: "#=SuitableNumber(PlannedTimeIn)#" },
                { field: 'PlannedTimeOut', title: language == "ne" ? "तोकीएको जाने  समय " : "Planned Out", filterable: false, sortable: false, template: "#=SuitableNumber(PlannedTimeOut)#" },
                { field: 'ActualTimeIn', title: language == "ne" ? "कर्मचारी आको समय " : "Actual In", filterable: false, sortable: false, template: "#=SuitableNumber(ActualTimeIn)#" },
                { field: 'ActualTimeOut', title: language == "ne" ? "कर्मचारी गको समय " : "Actual Out", filterable: false, sortable: false, template: "#=SuitableNumber(ActualTimeOut)#" },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }

    self.KendoGridOptionForAbsent = function (title, type, target, showOnCard) {
        showOnCard = showOnCard || false;
        var obj =
        {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetAttendanceInfoDetails",
            paramData: { type: type },
            multiSelect: false,
            excellExport: function () {

            },
            columns: [
                { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
                { field: 'EmployeeCode', title: language == "ne" ? "कर्मचारी कोड" : "Employee Code", template: "#=SuitableNumber(EmployeeCode)#" },
                { field: 'EmployeeName', title: language == "ne" ? "कर्मचारीको नाम" : "Employee Name" }

            ],

            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
        if ((showOnCard || false) == false)
            obj.columns.push({ field: 'DepartmentName', title: language == "ne" ? "विभाग" : "Department" });
        return obj;
    }

    self.KendoGridOptionForLateIn = function (title, type, target) {
        return {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetAttendanceInfoDetails",
            paramData: { type: type },
            multiSelect: false,
            excellExport: function () {

            },
            columns: [
                { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
                { field: 'EmployeeCode', title: language == "ne" ? "कर्मचारी कोड" : "Employee Code", template: "#=SuitableNumber(EmployeeCode)#" },
                { field: 'EmployeeName', title: language == "ne" ? "कर्मचारीको नाम" : "Employee Name" },
                { field: 'DepartmentName', title: language == "ne" ? "विभाग" : "Department" },
                { field: 'PlannedTimeIn', title: language == "ne" ? "तोकीएको आउने समय " : "Planned In", filterable: false, sortable: false, template: "#=SuitableNumber(PlannedTimeIn)#" },
                //{ field: 'PlannedTimeOut', title: language == "ne" ? "तोकीएको जाने  समय " : "Planned Out", filterable: false, sortable: false },
                { field: 'ActualTimeIn', title: language == "ne" ? "कर्मचारी आको समय " : "Actual In", filterable: false, sortable: false, template: "#=SuitableNumber(ActualTimeIn)#" },
                { field: 'LateTime', title: language == "ne" ? "ढिला आको समय " : "Late In", filterable: false, sortable: false, template: "#=SuitableNumber(LateTime)#" },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }

    self.KendoGridOptionForOfficeVisit = function (title, type, target) {
        return {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetAttendanceInfoDetails",
            paramData: { type: type },
            groupParam: { field: "Remark" },
            group: false,
            multiSelect: false,
            excellExport: function () {

            },
            columns: [
                { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
                { field: 'EmployeeCode', title: language == "ne" ? "कर्मचारी कोड" : "Employee Code", template: "#=SuitableNumber(EmployeeCode)#" },
                { field: 'EmployeeName', title: language == "ne" ? "कर्मचारीको नाम" : "Employee Name" },
                { field: 'DepartmentName', title: language == "ne" ? "विभाग" : "Department" },
                { field: 'Remark', title: language == "ne" ? "टिप्पणी" : "Remark" },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }

    self.KendoGridOptionsForDepartments = function (title, type, target) {
        return {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetDepartments",
            paramData: { type: type },
            multiSelect: false,
            excellExport: function () {

            },
            columns: [
                { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
                { field: 'Code', title: language == "ne" ? "कोड" : "Code", template: "#=SuitableNumber(Code)#" },
                { field: 'Name', title: language == "ne" ? "विभागको नाम" : "Name" },
                { field: 'NameNp', title: language == "ne" ? "विभागको नाम नेपाली " : "Name Nepali" },
                { field: 'NumberOfStaff', title: language == "ne" ? "कर्मचारी संख्या" : "Number Of Staff", template: "#=SuitableNumber(NumberOfStaff)#" },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }

    self.KendoGridOptionsForActiveDevice = function (title, type, target) {

        return {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetActiveDeviceKendoGrid",
            paramData: { type: type },
            multiSelect: false,
            excellExport: function () {
            },
            columns: [
                { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
                { field: 'Name', title: "Name", filterable: false, width: 100 },
                { field: 'IP', title: "IpAddress", filterable: false, width: 90 },
                { field: 'SN', title: "SN", filterable: false, width: 130 },
                { field: 'DeviceModel', title: "Model", filterable: false },
                { field: 'DeviceStatus', title: "Status", filterable: false, template: "#=GetDeviceStatus(DeviceStatus)#", width: 150 },
                { field: 'LastActivity', title: "Last Activity", filterable: false, width: 130 },
                { field: 'DevFuns', title: "Funs", filterable: false },
                { field: 'FaceCount', title: "Face ", filterable: false, width: 40 },
                { field: 'FPCount', title: "FP", filterable: false, width: 40 },
                { field: 'TransCount', title: "Trans", filterable: false, width: 40 },
                { field: 'UserCount', title: "User", filterable: false, width: 40 },
                //{ field: 'FirmwareVersion', title: "Fw", filterable: false, width: 40 },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }

    function getDashBoardDivData() {
        Riddha.ajax.get(url + "/GetAttendanceInfoForDiv").done(function (result) {
            self.dashBoardDivData(result.Data);
        });
    }

    self.GetAttendanceInfoByType = function (type) {
        switch (type) {
            case 'Absent':
                return self.AttendanceInfo().AbsentCount();

            case 'Office Visit':
                return self.AttendanceInfo().OfficeVisitCount();

            case 'Kaj':
                return self.AttendanceInfo().KajCount();

            case 'Leave':
                return self.AttendanceInfo().OnLeaveCount();
            default:
                return 0;

        }
    }

    self.GetTypeName = function (type) {
        var typeName = type;
        if (language == "ne") {
            switch (type) {
                case 'Absent':
                    typeName = "अनुपस्थित";
                    break;
                case 'Office Visit':
                    typeName = "कार्यलय भ्रमण";
                    break;
                case 'Kaj':
                    typeName = "काज";
                    break;
                case 'Leave':
                    typeName = "बिदामा";
                    break;
                default:

            };
        }
        return typeName;

    };

}

function resellerHomeController() {
    var self = this;
    var url = "/Api/HomeApi"
    self.DeviceInfo = ko.observable();
    self.CompanyLst = ko.observableArray([]);
    getResellerDeviceInfo();
    var docHeight = $(window).height();
    function getResellerDeviceInfo() {
        Riddha.ajax.get(url + "/GetResellerDashboardDeviceInfo")
            .done(function (result) {
                if (result.Status == 4) {
                    self.DeviceInfo(new ResellerDeviceInfoModel(result.Data.ResellerVm));
                    //var data = Riddha.ko.global.arrayMap(result.Data.CompanyList, ResellerCompanyModel);
                    //self.CompanyLst(data);
                }
            });
    };
    self.KendoGridOptions = {
        title: "Customers",
        target: "#customerKendoGrid",
        url: url + "/GetCustomerKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        selectable: false,
        group: true,
        columns: [
            { field: 'CompanyCode', title: "Company Code", groupable: false, filterable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
            { field: 'CompanyName', title: "Company Name", groupable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 100 },
            { field: 'CompanyAddress', title: "Address", groupable: false, filterable: false },
            { field: 'ContactNo', title: "Contact No", groupable: false, filterable: false, width: 100 },
            { field: 'ContactPerson', title: "Contact Person", groupable: false, filterable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
            { field: 'ServiceStartedFrom', title: "Service Started", groupable: false, filterable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
            { field: 'LicenseExpiredDate', title: "License Expiry", groupable: false, filterable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
            { field: 'NoOfBranches', title: "Branches", groupable: false, filterable: false },
            { field: 'NoOfDevice', title: "Devices", groupable: false, filterable: false },
            { field: 'NoOfEmployee', title: "Employees", groupable: false, filterable: false },
            { field: 'NoOfUsers', title: "Users", groupable: false, filterable: false },
            { field: 'SoftwarePackage', title: "Package", filterable: false }
        ],
        SelectedItem: function (item) {
        },
        SelectedItems: function (items) {
        }
    }

    self.KendoGridOptionsForStockDevice = function (title, target) {
        return {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetResellerStockDeviceList",
            paramData: {},
            multiSelect: false,
            excellExport: function () {
            },
            columns: [
                { field: 'ModelName', title: "Model", filterable: false, sortable: false },
                { field: 'SerialNo', title: "Serial No", filterable: false, sortable: false },
                { field: 'DeviceType', title: "Device Type", filterable: false, sortable: false },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }

    self.KendoGridOptionsForDamageDevice = function (title, target) {
        return {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetResellerDamageDeviceList",
            paramData: {},
            multiSelect: false,
            excellExport: function () {
            },
            columns: [
                { field: 'ModelName', title: "Model", filterable: false, sortable: false },
                { field: 'SerialNo', title: "Serial No", filterable: false, sortable: false },
                { field: 'DeviceType', title: "Device Type", filterable: false, sortable: false },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }
}

function ownerHomeController() {
    var self = this;
    var url = "/Api/HomeApi"
    self.DeviceInfo = ko.observable();
    self.ResellerLst = ko.observableArray([]);
    var docHeight = $(window).height();

    getOwnerDeviceInfo();
    function getOwnerDeviceInfo() {
        Riddha.ajax.get(url + "/GetResellersCustomerInfo")
            .done(function (result) {
                self.DeviceInfo(new OwnerDeviceInfoModel(result.Data.DashBoardAdminVm));
                var data = Riddha.ko.global.arrayMap(result.Data.ResellerList, OwnerResellerModel)
                self.ResellerLst(data);
                ShowChart();
            });
    };

    function ShowChart() {
        var pieChartCanvas = $("#ownerChart").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [

            {
                value: self.DeviceInfo().DamageDeviceCount(),
                color: "red",
                highlight: "#f56955",
                label: "Damaged"
            },
            {
                value: self.DeviceInfo().ResellerDeviceCount(),
                color: "#00A646",
                highlight: "#00a65b",
                label: "Partners"
            },
            {
                value: self.DeviceInfo().CustomerDeviceCount(),
                color: "#F39C01",
                highlight: "#f39c13",
                label: "Customer"
            },
            {
                value: self.DeviceInfo().NewDeviceCount(),
                color: "#00C0EA",
                highlight: "#00c0ee",
                label: "Stock"
            }
        ];
        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,

            //String - The colour of each segment stroke
            segmentStrokeColor: "#ccc",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 1,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 150,
            //String - Animation easing effect
            animationEasing: 'easeInQuad',//"easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: true,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: false,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=label.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>",
            //String - A tooltip template
            tooltipTemplate: "<%=label%>: <%=value %>  Device"
        };
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);
    }

    self.KendoGridOptionsPatners = function (title, target) {
        return {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetResellerDetails",
            paramData: {},
            multiSelect: false,
            excellExport: function () {

            },
            columns: [
                { field: 'Code', title: "Code", filterable: false, sortable: false },
                { field: 'Name', title: "Name", filterable: false, sortable: false },
                { field: 'Address', title: "Address", filterable: false, sortable: false },
                { field: 'Contact', title: "Contact", filterable: false, sortable: false },
                { field: 'ContactPerson', title: "ContactPerson", filterable: false, sortable: false },
                { field: 'Email', title: "Email", filterable: false, sortable: false },
                { field: 'Website', title: "Website", filterable: false, sortable: false },
                { field: 'PanNo', title: "PanNo", filterable: false, sortable: false },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }

    self.KendoGridOptionsCustomer = function (title, target) {
        return {
            title: title,
            target: target,
            width: '70%',
            height: docHeight - 20,
            url: "/Api/HomeApi/GetCompanyDetails",
            paramData: {},
            multiSelect: false,
            excellExport: function () {

            },
            columns: [
                { field: 'Code', title: "Code", filterable: false, sortable: false },
                { field: 'Name', title: "Name", filterable: false },
                { field: 'Address', title: "Address", filterable: false, sortable: false },
                { field: 'Contact', title: "Contact", filterable: false, sortable: false },
                { field: 'ContactPerson', title: "ContactPerson", filterable: false, sortable: false },
                { field: 'Email', title: "Email", filterable: false, sortable: false },
                { field: 'Website', title: "Website", filterable: false, sortable: false },
                { field: 'ResellerName', title: "ResellerName", filterable: false, sortable: false },
                { field: 'DeviceCount', title: "DeviceCount", filterable: false, sortable: false },
            ],
            getSelectedItem: function (item) {
            },
            SelectedItems: function (items) {
            }
        };
    }
}

function menuActionController() {
    var self = this;
    self.Menus = ko.observableArray([]);
    self.ActionCodes = ko.observableArray([]);
    self.SearchField = ko.observable('check');
    var partnerName = localStorage.getItem('resellerName');
    var partnerContact = localStorage.getItem('resellerContact');
    $('.partnerContact').text(partnerContact);
    $('.partnerName').text(partnerName);
    getMenus();
    function getMenus() {
        Riddha.ajax.get("/Api/MenuApi/GetMenus")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, MenuModel);
                self.Menus(data);
            })
    }

}

function licenseInfoController() {
    var self = this;
    self.licenseInfo = ko.observable(new LicenseInfoVm());
    var docHeight = $(document).height();
    self.LicenseExpiryDays = ko.observable(0);




    getlicenseInfo();

    function DateDiffrence() {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();
        today = yyyy + '/' + mm + '/' + dd;
        var dateFirst = new Date(today);
        var dateSecond = new Date(self.licenseInfo().ExpiryDate());
        // time difference
        var timeDiff = Math.abs(dateSecond.getTime() - dateFirst.getTime());
        // days difference
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        // difference
        self.LicenseExpiryDays(diffDays)
        $('#license123').text(diffDays);
    };


    function getlicenseInfo() {
        Riddha.ajax.get("/Api/CompanyApi/GetLicenseInfo")
            .done(function (result) {
                self.licenseInfo(new LicenseInfoVm(result.Data));
                DateDiffrence();
            })
    };
    self.ShowModal = function () {

        $("#licenseInfoModal").modal('show');
    };
}

function changePasswordController() {
    var self = this;
    var url = "/Api/UserApi/ChangePassword";
    var language = Riddha.config().CurrentLanguage;
    self.User = ko.observable(new ChangePasswordModel());
    self.Password = ko.observable('');
    self.IsValid = ko.observable(false);
    self.PasswordInfo = ko.observable('');
    self.PasswordInfoStyle = ko.observable('');

    function GetUser() {
        Riddha.ajax.get(url, null)
            .done(function (result) {
                var data = new ChangePasswordModel(result.Data);
                self.Password(result.Data.Password)
                self.User(data);
            });
    }

    self.PasswordInfo = ko.computed(function () {
        var password = self.User().NewPassword();
        self.PasswordInfoStyle('form-control text-center bg-red-gradient');
        if (/^([0|\+[0-9]{1,5})?([7-9][0-9]{9})$/.test(password) == false) {
            if (password == "") {
                self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
                return language == "ne" ? "उदाहरण: Hamro@hajiri123 " : "eg:(valid password):Hamro@hajiri123";
            }
            else if (password.length < 6) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डको लम्मबाइ ६ भन्दा बढी चाहिन्छ " : "Password should be of more than 6 chars";
            } else if (password.search(/\d/) == -1) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा नम्बर अनिवार्य छ" : "Password should contain a number";
            } else if (password.search(/[a-zA-Z]/) == -1) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा अल्फाबेट अनिवार्य छ" : "Password should contain a alphabet";
            } else if (/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/.test(password) == false) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा बिषेस स्ंकेत अनिवार्य छ" : "Password should contain a special symbol";
            } else if ((/[A-Z]/.test(password) && /[a-z]/.test(password)) == false) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा " : "Password should contain a upper & lower case";
            }
            else {
                self.IsValid(true);
                self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
                return language == "ne" ? "पास्वर्ड ठीक छ" : "Valid Password";
            }
        }
        else {
            self.IsValid(true);
            self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
            return language == "ne" ? "पास्वर्ड ठीक छ" : "Valid Password";
        }

    }, self);

    self.Change = function () {
        if (self.User().CurrentPassword() == "") {
            Riddha.util.localize.Required("CurrentPassword");
            return;
        }
        if (self.User().NewPassword() == "") {
            Riddha.util.localize.Required("NewPassword");
            return;
        }
        if (self.User().ConfirmPassword() == "") {
            Riddha.util.localize.Required("ConfirmPassword");
            return;
        }
        if (!self.IsValid()) {
            Riddha.UI.Toast("Password not valid", 0);
            return;
        }
        if (self.IsValidCurrentPassword() && self.ComparePassword()) {
            Riddha.ajax.post(url, self.User())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    }

    self.ShowModal = function () {
        GetUser();
        $("#changePasswordModal").modal('show');
    }

    self.CloseModal = function () {
        $("#changePasswordModal").modal('hide');
    }

    self.Reset = function () {
        self.User().CurrentPassword('');
        self.User().NewPassword('');
        self.User().ConfirmPassword('');
    }

    self.IsValidCurrentPassword = function () {
        if (self.User().CurrentPassword() != self.Password()) {
            Riddha.UI.Toast("Invalid Current Password", 0);
            return false;
        }
        else {
            return true;
        }
    }
    self.ComparePassword = function () {
        if (self.User().ConfirmPassword() != self.User().NewPassword()) {
            Riddha.UI.Toast("Passwords do not match", 0);
            return false;
        }
        else {
            return true;
        }
    }
}

function showLicenseModal() {
    licenseObj.ShowModal();
}

function showChangePasswordModal() {
    changePasswordObj.ShowModal();
}