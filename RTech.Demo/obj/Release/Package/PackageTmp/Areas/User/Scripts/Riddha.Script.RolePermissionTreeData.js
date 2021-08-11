var lang = new Riddha.config().CurrentLanguage;
var packageId = parseInt(new Riddha.config().PackageId);
var rolePermissionsData = [
    {
        id: "0006", text: lang == "ne" ? "ड्यास्बोर्ड विजेट" : "Dashboard Widgets", expanded: false, items: [
            { id: "6001", text: lang == "ne" ? "हाजिरी जानकारी " : "Attendance Info" },
            { id: "6002", text: lang == "ne" ? "पात्रो" : "Calender" },
            { id: "6003", text: lang == "ne" ? "समाचार" : "News" },
            { id: "6004", text: lang == "ne" ? "समाचार" : "Self Attendance Info" }
        ],
    },
    {
        id: "0001", text: lang == "ne" ? "सेट अप" : "Setup", expanded: false, checked: false, items: [
            {
                id: "1001", text: lang == "ne" ? "आर्थिक वर्ष" : "Fiscal Year", items: [
                    { id: "1038", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1011", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "1012", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "1013", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "1002", text: lang == "ne" ? "हाजिरी मेसिन्" : "Device Management", items: [
                    { id: "1039", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1014", text: lang == "ne" ? "परिमार्जन " : "Update" },
                ]
            },
            {
                id: "1003", text: lang == "ne" ? "कम्पनीको परिचय" : "Company Profile", items: [
                    { id: "1040", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1015", text: lang == "ne" ? "परिमार्जन " : "Update" },
                ]
            },
            {
                id: "1004", text: lang == "ne" ? "शाखा ब्यवस्थापन" : "Branch Management", items: [
                    { id: "1041", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1016", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "1017", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "1018", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "1005", text: lang == "ne" ? "विभाग" : "Department", items: [
                    { id: "1042", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1019", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "1020", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "1021", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "1006", text: lang == "ne" ? "फाँट" : "Section", items: [
                    { id: "1043", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1022", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "1023", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "1024", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "1007", text: lang == "ne" ? "पद" : "Designation", items: [
                    { id: "1044", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1025", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "1026", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "1027", text: lang == "ne" ? "हटाउने " : "Delete" },
                    { id: "1028", text: lang == "ne" ? "बिदा कोटा" : "Leave Quota" }
                ]
            },
            {
                id: "1008", text: lang == "ne" ? "दर्जा समुह" : "Grade Group", items: [
                    { id: "1045", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1029", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "1030", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "1031", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "1009", text: lang == "ne" ? "सिफ्ट" : "Shift", items: [
                    { id: "1046", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1032", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "1033", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "1034", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "1010", text: lang == "ne" ? "बैंक" : "Bank", items: [
                    { id: "1047", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "1035", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "1036", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "1037", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            }
        ]
    },
    {
        id: "0002", text: lang == "ne" ? "कार्यलय ब्यवस्थापन" : "Office Management", expanded: false, items: [
            {
                id: "2001", text: lang == "ne" ? "कर्मचारी" : "Employee", items: [
                    { id: "2036", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2009", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "2010", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "2011", text: lang == "ne" ? "हटाउने " : "Delete" },
                    { id: "2012", text: lang == "ne" ? "बिबाराण " : "View Details" },
                    { id: "2044", text: lang == "ne" ? "सिफ्ट प्रकार" : "Shift Type" },
                    { id: "2013", text: lang == "ne" ? "हाजिरी मेसिनमा कर्मचारी ल्याउने" : "Pull Device Employee" },
                    { id: "2014", text: lang == "ne" ? "लागिन" : "Login" },
                    { id: "2015", text: lang == "ne" ? "एक्सल फरमेट डाउनलोड" : "Download Excel Format" },
                    { id: "2016", text: lang == "ne" ? "एक्स्पोर्ट" : "Export" },
                    { id: "2017", text: lang == "ne" ? "अपलोड एक्सल फाइल" : "Upload Excel File" }
                ]
            },
            {
                id: "8004", text: lang == "ne" ? "स्थिर कार्यसुची" : "Fixed Roster", items: [
                    { id: "7122", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "7123", text: lang == "ne" ? "सिर्जना" : "Create" },
                ]
            },
            {
                id: "2002", text: lang == "ne" ? "मासिक कार्यसुची" : "Monthly Roster", items: [
                    { id: "2037", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2018", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "2045", text: lang == "ne" ? "एक्स्पोर्ट " : "Export Roster" },
                    { id: "2046", text: lang == "ne" ? "अपलोड एक्सल फाइल" : "Upload Roster" },
                ]
            },
            {
                id: "2003", text: lang == "ne" ? "साप्ताहिक कार्यसुची" : "Weekly Roster", items: [
                    { id: "2038", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2019", text: lang == "ne" ? "सिर्जना" : "Create" },
                ]
            },
            {
                id: "2004", text: lang == "ne" ? "सार्वजनिक बिदा" : "Holiday", items: [
                    { id: "2039", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2020", text: lang == "ne" ? "डिफल्ट सार्वजनिक बिदा ल्याउने" : "Pull Default Holiday" },
                    { id: "2021", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "2022", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "2023", text: lang == "ne" ? "हटाउने " : "Delete" },

                ]
            },
            {
                id: "2005", text: lang == "ne" ? "हाजिरी" : "Manual Punch", items: [
                    { id: "2040", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2024", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "2025", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "2026", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },

            {
                id: "2006", text: lang == "ne" ? "सूचना" : "Notice", items: [
                    { id: "2041", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2027", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "2028", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "2029", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "2007", text: lang == "ne" ? "इभेन्ट" : "Event", items: [
                    { id: "2042", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2030", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "2031", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "2032", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },

            {
                id: "2008", text: lang == "ne" ? "कार्यलय भ्रमन" : "Office Visit", items: [
                    { id: "2043", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2033", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "2034", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "2035", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "8019", text: lang == "ne" ? "कार्यलय भ्रमन स्विकृती" : "Office Visit Approval", items: [
                    { id: "7225", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "7226", text: lang == "ne" ? "स्विकृती" : "Approval" },
                ]
            },
            {
                id: "8020", text: lang == "ne" ? "काज" : "Kaj", items: [
                    { id: "7227", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "7228", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "7229", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "7230", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },

            {
                id: "8021", text: lang == "ne" ? "काज स्विकृती" : "Kaj Approval", items: [
                    { id: "7231", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "7232", text: lang == "ne" ? "स्विकृती" : "Approval" },
                ]
            },
        ]
    },
    {
        id: "0003", text: lang == "ne" ? "बिदा व्यवस्थापन" : "Leave Management", expanded: false, items: [
            {
                id: "3001", text: lang == "ne" ? "बिदा" : "Leave Master", items: [
                    { id: "3025", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "3006", text: lang == "ne" ? "डिफल्ट बिदा ल्याउने" : "Pull Default Leave" },
                    { id: "3007", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "3008", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "3009", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "3002", text: lang == "ne" ? "बिदा निवेदन" : "Leave Application", items: [
                    { id: "3026", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "3010", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "3011", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "3012", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "3003", text: lang == "ne" ? "बिदा मज्जुरी" : "Leave Approval", items: [
                    { id: "3027", text: lang == "ne" ? "अवलोकन" : "View" },
                    { id: "3013", text: lang == "ne" ? "स्विकृती" : "Approve" },
                    { id: "3014", text: lang == "ne" ? "अस्विकार" : "Reject" }
                ]
            },
            {
                id: "3004", text: lang == "ne" ? "बिदा मीलाउने" : "Leave Settlement", items: [
                    { id: "3028", text: lang == "ne" ? "अवलोकन" : "View" },
                    { id: "3018", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "3019", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "3020", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "3005", text: lang == "ne" ? "बिदा कोटा" : "Leave Quota", items: [
                    { id: "3029", text: lang == "ne" ? "अवलोकन" : "View" },
                    { id: "3021", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "3022", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "3023", text: lang == "ne" ? "हटाउने " : "Delete" },
                    { id: "3024", text: lang == "ne" ? "बिदा कोटा" : "Leave Quota" }
                ]
            },
            {
                id: "8016", text: lang == "ne" ? "बिदा प्रतिस्थापन" : "Leave Replacement", items: [
                    { id: "7196", text: lang == "ne" ? "अवलोकन" : "View" },
                    { id: "7197", text: lang == "ne" ? "रीफ्रेस" : "Refresh" },
                    { id: "7198", text: lang == "ne" ? "अनुमोदन" : "Approve" },
                    { id: "7201", text: lang == "ne" ? "अनुमोदनको सूची" : "Approved List" }
                ]
            },
            {
                id: "8023", text: lang == "ne" ? "Leave Opening Balance" : "Leave Opening Balance", items: [
                    { id: "7233", text: lang == "ne" ? "अवलोकन" : "View" },
                    { id: "7233", text: lang == "ne" ? "रीफ्रेस" : "Refresh" },
                    { id: "7234", text: lang == "ne" ? "सिर्जना" : "Create" },
                ]
            }
        ]
    },
    {
        id: "0009", text: lang == "ne" ? "मोबाइल अनुरोध" : "Mobile Request", expanded: false, items: [
            {
                id: "8030", text: lang == "ne" ? "हाजिरी अनुरोध" : "Manual Punch Request", items: [
                    { id: "7265", text: lang == "ne" ? "अवलोकन" : "View" },
                    { id: "7266", text: lang == "ne" ? "विवरण" : "View Details" },
                    { id: "7267", text: lang == "ne" ? "अनुमोदन गर्नुहोस्" : "Approve" },
                    { id: "7268", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            },
            {
                id: "8031", text: lang == "ne" ? "कार्यलय भ्रमन" : "Office Visit Request", items: [
                    { id: "2043", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "2033", text: lang == "ne" ? "सिर्जना" : "View Details" },
                    { id: "2034", text: lang == "ne" ? "परिमार्जन " : "Approve" },
                    { id: "2035", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            }
        ]
    },
    {
        id: "0004", text: lang == "ne" ? "प्रयोगकर्ता ब्यवस्थापन" : "User Management", expanded: false, items: [
            {
                id: "4001", text: lang == "ne" ? "प्रयोगकर्ताको समुह/भूमिका" : "User Role/Group", items: [
                    { id: "4009", text: lang == "ne" ? "अवलोकन" : "View" },
                    { id: "4003", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "4004", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "4005", text: lang == "ne" ? "हटाउने " : "Delete" },
                    { id: "4010", text: lang == "ne" ? "अनुमती" : "Permission" }
                ]
            },
            {
                id: "4002", text: lang == "ne" ? "प्रयोगकर्ता" : "User", items: [
                    { id: "4011", text: lang == "ne" ? "अवलोकन " : "View" },
                    { id: "4006", text: lang == "ne" ? "सिर्जना" : "Create" },
                    { id: "4007", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                    { id: "4008", text: lang == "ne" ? "हटाउने " : "Delete" },
                ]
            }
        ]
    }
]

var reportMenu = {
    id: "0005", text: lang == "ne" ? "प्रतिबेदन" : "Report", expanded: false, items: getPackageWiseReportItems()
};



var hrMenu = {
    id: "0007", text: lang == "ne" ? "मानव स्रोत व्यवस्थापन" : "HR Management", expanded: false, hidden: true, items: [
        {
            id: "7001", text: lang == "ne" ? "कर्मचारी स्थिति" : "Employee Status", items: [
                { id: "7121", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7100", text: lang == "ne" ? "सम्झौता अवलोकन" : "Contract View" },
                { id: "7101", text: lang == "ne" ? "सम्झौता सिर्जना" : "Contract Create" },
                { id: "7102", text: lang == "ne" ? "सम्झौता परिमार्जन " : "Contract Edit" },
                { id: "7103", text: lang == "ne" ? "सम्झौता हटाउने " : "Contract Delete" },
                { id: "7104", text: lang == "ne" ? "राजीनामा अवलोकन " : "Resignation View" },
                { id: "7105", text: lang == "ne" ? "राजीनामा सिर्जना/परिमार्जन " : "Resignation Create/Update" },
                { id: "7106", text: lang == "ne" ? "समापन अवलोकन " : "Termination View" },
                { id: "7107", text: lang == "ne" ? "समापन सिर्जना/परिमार्जन " : "Termination Create/Update" },
            ]
        },
        {
            id: "7002", text: lang == "ne" ? "कर्मचारी प्रमाणिकरण" : "Employee Status Verification", items: [
                { id: "7108", text: lang == "ne" ? "सम्झौता प्रमाणिकरण" : "Contract Verification" },
                { id: "7109", text: lang == "ne" ? "सम्झौता अवलोकन" : "Contract View" },
                { id: "7110", text: lang == "ne" ? "सम्झौता अनुमोदन" : "Contract Approval" },
                { id: "7111", text: lang == "ne" ? "राजीनामा प्रमाणिकरण " : "Resignation Verification" },
                { id: "7112", text: lang == "ne" ? "राजीनामा अवलोकन" : "Resignation View" },
                { id: "7113", text: lang == "ne" ? "राजीनामा अनुमोदन" : "Resignation Approval" },
                { id: "7114", text: lang == "ne" ? "समापन प्रमाणिकरण " : "Termination Verification" },
                { id: "7115", text: lang == "ne" ? "समापन अवलोकन" : "Termination View" },
                { id: "7116", text: lang == "ne" ? "समापन अनुमोदन" : "Termination Approval" },
            ]
        },
        {
            id: "7004", text: lang == "ne" ? "रोजगार स्थिति" : "Employment Status", items: [
                { id: "7117", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7118", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7119", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7120", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "7005", text: lang == "ne" ? "शिक्षा" : "Education", items: [
                { id: "7137", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7138", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7139", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7140", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },

        {
            id: "7006", text: lang == "ne" ? "भाषा" : "Language", items: [
                { id: "7141", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7142", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7143", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7144", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "7007", text: lang == "ne" ? "लाइसेन्स" : "License", items: [
                { id: "7145", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7146", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7147", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7148", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "7008", text: lang == "ne" ? "कौशल" : "Skill", items: [
                { id: "7149", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7150", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7151", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7152", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "8006", text: lang == "ne" ? "पाठ्यक्रम" : "Course", items: [
                { id: "7125", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7126", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7127", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7128", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "8007", text: lang == "ne" ? "सत्र" : "Session", items: [
                { id: "7129", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7130", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7131", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7132", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "8008", text: lang == "ne" ? "सहभागी सत्र" : "Participating Session", items: [
                { id: "7133", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7134", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7135", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7136", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "8011", text: lang == "ne" ? "यात्रा" : "Travel", items: [
                { id: "7159", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7160", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7161", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7162", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "8010", text: lang == "ne" ? "अनुकूलन क्षेत्र" : "Custom Field", items: [
                { id: "7155", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7156", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7157", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7158", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "8012", text: lang == "ne" ? "शास्त्रीय मामला" : "Disciplinary Cases", items: [
                { id: "7163", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7164", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7165", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7166", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "8014", text: lang == "ne" ? "यात्रा टिकट" : "Travel Ticket", items: [
                { id: "7179", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7180", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7181", text: lang == "ne" ? "परिमार्जन " : "Edit" },
                { id: "7182", text: lang == "ne" ? "हटाउने " : "Delete" },
            ]
        },
        {
            id: "8015", text: lang == "ne" ? "यात्रा टिकटको अनुमोदन" : "Travel Ticket Approval", items: [
                { id: "7190", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7191", text: lang == "ne" ? "अनुमोदन" : "Approval" },
                { id: "7192", text: lang == "ne" ? "उल्टाउन" : "Revert" },
                { id: "7193", text: lang == "ne" ? "विवरण" : "Details" },
            ]
        },
        {
            id: "8015", text: lang == "ne" ? "रिपोर्टिंग लाइनअप" : "Reporting Lineup", items: [
                { id: "7254", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7255", text: lang == "ne" ? "अनुमोदन" : "Approval" },
                { id: "7256", text: lang == "ne" ? "उल्टाउन" : "Revert" },
                { id: "7257", text: lang == "ne" ? "विवरण" : "Details" },
            ]
        }

    ]
};

var payRollMenu = {
    id: "0008", text: lang == "ne" ? "तलबी बिबरण व्यवस्थापन" : "Payroll Management", expanded: false, hidden: true, items: [
        {
            id: "8001", text: lang == "ne" ? "भत्ता" : "Allowance", items: [
                { id: "7220", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7202", text: lang == "ne" ? "अनुमोदन" : "Create" },
                { id: "7203", text: lang == "ne" ? "परिमार्जन" : "Edit" },
                { id: "7204", text: lang == "ne" ? "हटाउने" : "Delete" },
            ]
        },
        {
            id: "8002", text: lang == "ne" ? "कटौती" : "Deduction", items: [
                { id: "7205", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7206", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7207", text: lang == "ne" ? "परिमार्जन" : "Edit" },
                { id: "7208", text: lang == "ne" ? "हटाउने" : "Delete" },
            ]
        },
        {
            id: "8003", text: lang == "ne" ? "तलबी सेटअप" : "Payroll Setup", items: [
                { id: "7209", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7210", text: lang == "ne" ? "सिर्जना" : "Create" },
                { id: "7211", text: lang == "ne" ? "परिमार्जन" : "Edit" },
                { id: "7212", text: lang == "ne" ? "हटाउने" : "Delete" },
            ]
        },
        {
            id: "8005", text: lang == "ne" ? "तलबी प्रमाणिकरण" : "Payroll Verification", items: [
                { id: "7213", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7214", text: lang == "ne" ? "अनुमोदन" : "Approval" },
                { id: "7215", text: lang == "ne" ? "उल्टाउन" : "Revert" },
                { id: "7216", text: lang == "ne" ? "विवरण" : "Details" },
            ]
        },
        {
            id: "8017", text: lang == "ne" ? "तलबी कन्फिगरेसन" : "Payroll Configuration", items: [
                { id: "7199", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7200", text: lang == "ne" ? "सिर्जना" : "Create/Update" },
            ]
        },
        {
            id: "8024", text: lang == "ne" ? "तलब सिट" : "Salary Sheet", items: [
                { id: "7242", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7243", text: lang == "ne" ? "Salary Post" : "Salary Post" },
            ]
        },
        {
            id: "8027", text: lang == "ne" ? "तलब भुक्तानी" : "Salary Payment", items: [
                { id: "7258", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7259", text: lang == "ne" ? "Pay Salary" : "Pay Salary" },
            ]
        },
        {
            id: "8028", text: lang == "ne" ? "तलब भुक्तान पर्ची" : "Salary Payslip", items: [
                { id: "7260", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7261", text: lang == "ne" ? "Pay Salary" : "Generate Payslip" },
            ]
        },
        {
            id: "8029", text: lang == "ne" ? "तलब स्वीकृति" : "Salary Approval", items: [
                { id: "7262", text: lang == "ne" ? "अवलोकन" : "View" },
                { id: "7263", text: lang == "ne" ? "Approval" : "Approval" },
                { id: "7264", text: lang == "ne" ? "Review" : "Review" },
            ]
        },


    ]
}
if (packageId > 0) {
    rolePermissionsData.push(hrMenu);
}
if (packageId == 2 || packageId == 3) {
    rolePermissionsData.push(payRollMenu);
}
rolePermissionsData.push(reportMenu);

//functions
function getPackageWiseReportItems() {
    var hrRptItems = {
        id: "8013", text: lang == "ne" ? "मानव स्रोत प्रतिवेदन" : "HR Report", items: [
            { id: "7170", text: lang == "ne" ? "कर्मचारीको अनुसार बिदाको प्रतिबेदन" : "Employees Personal Information Report" },
            { id: "7167", text: lang == "ne" ? "कर्मचारीको सम्झौताको प्रतिबेदन" : "Employee Contract Report" },
            { id: "7169", text: lang == "ne" ? "कर्मचारीको राजीनाको प्रतिबेदन" : "Employee Resignation Report" },
            { id: "7168", text: lang == "ne" ? "कर्मचारीको समापनको प्रतिबेदन" : "Employee Termination Report" },
            { id: "7183", text: lang == "ne" ? "कर्मचारीको अनुभवको प्रतिबेदन" : "Employee Experience Report" },
            { id: "7184", text: lang == "ne" ? "कर्मचारीको योग्यताको प्रतिबेदन" : "Employee Qualification Report" },
            { id: "7185", text: lang == "ne" ? "पाठ्यक्रमको प्रतिबेदन" : "Course Report" },
            { id: "7186", text: lang == "ne" ? "सत्रको प्रतिबेदन" : "Session Report" },
            { id: "7187", text: lang == "ne" ? "सहभागीको प्रतिबेदन" : "Participant Report" },
            { id: "7188", text: lang == "ne" ? "शास्त्रीय मामला अनुसारको प्रतिबेदन" : "Disciplinary Case Wise Report" },
            { id: "7189", text: lang == "ne" ? "शास्त्रीय कर्मचारी अनुसारको प्रतिबेदन" : "Disciplinary Employee Wise Report" },
        ]
    };

    var rptItems = [
        {
            id: "5001", text: lang == "ne" ? "हाजिरी प्रतिबेदन" : "Attendance Report", items: [
                { id: "5011", text: lang == "ne" ? "कर्मचारीको दैनिक हाजिरी प्रतिबेदन" : "Daily Employee Attendance Report" },
                { id: "5002", text: lang == "ne" ? "दैनिक चाँडै आउने प्रतिबेदन" : "Daily Early In Report" },
                { id: "5003", text: lang == "ne" ? "दैनिक चाँडै जाने प्रतिबेदन" : "Daily Early Out Report" },
                { id: "5004", text: lang == "ne" ? "कर्मचारीको अनुपस्थित हुने दैनिक प्रतिबेदन" : "Daily Employee Absent Report" },
                { id: "5005", text: lang == "ne" ? "दैनिक ढिला आउने प्रतिबेदन" : "Daily Late In Report" },
                { id: "5006", text: lang == "ne" ? "दैनिक ढीला जाने प्रतिबेदन" : "Daily Late Out Report" },
                { id: "5007", text: lang == "ne" ? "दैनीकी  भुलेको पन्च प्रतिवेदन" : "Daily Missing Punches Report" },
                { id: "5010", text: lang == "ne" ? "दैनिक कर्मचारीको बिदाको प्रतिबेदन" : "Daily Employee Leave Report" },
                { id: "7222", text: lang == "ne" ? "दैनिक कर्मचारीको ओ.टी को प्रतिबेदन" : "Daily Employee OT Report" },
                { id: "7218", text: lang == "ne" ? "दैनिक म्यानोल हाजिरी को प्रतिबेदन" : "Daily Manual Punch Report" },
                { id: "7220", text: lang == "ne" ? "दैनिक कार्यलय भ्रमन को प्रतिबेदन" : "Daily Office Visit Report" },
                { id: "5008", text: lang == "ne" ? "मासिक हाजीरी प्रतिवेदन" : "Monthly Attendance Report" },
                { id: "5009", text: lang == "ne" ? "कर्मचारीको मासिक संक्षेप प्रतिवेदन" : "Monthly Employee Summary Report" },
                { id: "7124", text: lang == "ne" ? "मासिक कर्मचारीको स्थिर प्रतिबेदन" : "Monthly Attendance Statistic Report" },
                { id: "5012", text: lang == "ne" ? "मासिक कर्मचारी बहु पंच प्रतिवेदन" : "Monthly Employee Multi Punch Report" },
                { id: "7178", text: lang == "ne" ? "मासिक कार्यसुचीको प्रतिबेदन" : "Monthly Roster Report" },
                { id: "7171", text: lang == "ne" ? "मासिक चाँडै आउने प्रतिबेदन" : "Monthly Early In Report" },
                { id: "7172", text: lang == "ne" ? "मासिक चाँडै जानेको प्रतिबेदन" : "Monthly Early Out Report" },
                { id: "7173", text: lang == "ne" ? "मासिक ढिला आउनेको प्रतिबेदन" : "Monthly Late In Report" },
                { id: "7174", text: lang == "ne" ? "मासिक ढीला जानेको प्रतिबेदन" : "Monthly Late Out Report" },
                { id: "7175", text: lang == "ne" ? "मासिक अनुपस्थित हुनेको प्रतिबेदन" : "Monthly Absent Report" },
                { id: "7176", text: lang == "ne" ? "मासिक भुलेको पन्चको प्रतिबेदन" : "Monthly Missing Punches Report" },
                { id: "7177", text: lang == "ne" ? "मासिक कर्मचारीको बिदाको प्रतिबेदन" : "Monthly Leave Report" },
                { id: "7194", text: lang == "ne" ? "मासिक ओ.टी को प्रतिबेदन" : "Monthly Ot Report" },
                { id: "7195", text: lang == "ne" ? "मासिक ओ.टी को दावी प्रतिबेदन" : "Monthly Ot Claim Report" },
                { id: "7219", text: lang == "ne" ? "मासिक म्यानोल हाजिरी को प्रतिबेदन" : "Monthly Manual Punch Report" },
                { id: "7221", text: lang == "ne" ? "मासिक कार्यलय भ्रमन को प्रतिबेदन" : "Monthly Office Visit Report" },
            ]
        },
        {
            id: "8009", text: lang == "ne" ? "बिदाको प्रतिबेदन" : "Leave Report", items: [
                { id: "7153", text: lang == "ne" ? "कर्मचारीको अनुसार बिदाको प्रतिबेदन" : "Employee Wise Leave Report" },
                { id: "7154", text: lang == "ne" ? "बिदा अनुसार बिदाको प्रतिबेदन" : "Leave Wise Leave Report" },
            ]
        },
        {
            id: "8018", text: lang == "ne" ? "तलबी प्रतिवेदन" : "Payroll Report", items: [
                { id: "7223", text: lang == "ne" ? "कर्मचारीको तलबी संक्षेप प्रतिबेदन" : "Employee Payroll Summary Report" },
                { id: "7224", text: lang == "ne" ? "कर्मचारीको भत्ता प्रतिबेदन" : "Employee Allownace Report" },
            ]
        }

    ]
    if (packageId == 1 || packageId == 2) {
        rptItems.push(hrRptItems);
    }
    //else if (packageId == 2) {
    //    rptItems.push(hrRptItems);
    //    rptItems.push(hrRptItems);
    //}
    return rptItems;
}