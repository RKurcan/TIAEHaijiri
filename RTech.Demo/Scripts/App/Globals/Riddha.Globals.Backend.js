jQuery(document).ready(function ($) {
    $("#NewSubfolder").click(function () {
        var selectedFolder = ckFinderApi.getSelectedFolder();
        ckFinderApi.openInputDialog("", "Please type the new folder name" + ": ", "", function (value) {
            if (value != "") {
                selectedFolder.createNewFolder(value);
            }
        });
    });

    $("#DeleteFolder").click(function () {
        var selectedFolder = ckFinderApi.getSelectedFolder();
        if (selectedFolder.parent != null) {
            selectedFolder.getFiles(function (files) {
                var hasFiles = (files.length > 0);
                if (selectedFolder.hasChildren || hasFiles) {
                    ckFinderApi.openMsgDialog("", selectedFolderCannotBeDeletedNotEmpty.replace("{$SelectedFolderName}", selectedFolder.name));
                } else {
                    ckFinderApi.openConfirmDialog("", "Press OK to delete the {$SelectedFolderName} folder".replace("{$SelectedFolderName}", selectedFolder.name), function (value) {
                        selectedFolder.remove();
                    });
                }
            }, true);
        } else {
            ckFinderApi.openMsgDialog("", "The {$SelectedFolderName} folder cannot be deleted because it's a root folder".replace("{$SelectedFolderName}", selectedFolder.name));
        }
    });

    $("#RenameFolder").click(function () {
        var selectedFolder = ckFinderApi.getSelectedFolder();

        if (selectedFolder.parent != null) {
            selectedFolder.getFiles(function (files) {
                var hasFiles = (files.length > 0);
                if (selectedFolder.hasChildren || hasFiles) {
                    ckFinderApi.openMsgDialog("", selectedFolderCannotBeRenamedNotEmpty.replace("{$SelectedFolderName}", selectedFolder.name));
                } else {
                    ckFinderApi.openInputDialog("", "Please type the new folder name" + ": ", selectedFolder.name, function (value) {
                        if (value != "") {
                            selectedFolder.rename(value);
                        }
                    });
                }
            }, true);
        } else {
            ckFinderApi.openMsgDialog("", "The {$SelectedFolderName} folder cannot be renamed because it's a root folder".replace("{$SelectedFolderName}", selectedFolder.name));
        }
    });

    $("#DeleteFile").click(function () {
        // if (IsSessionActive("/Admin/") && IsPageBrowseAuthorized("/Admin/FileManagerIsFileUsed")) {
        var selectedFile = ckFinderApi.getSelectedFile();
        if (selectedFile != null) {
            var fileUrl = selectedFile.getUrl();
            if (!IsFileUsed(fileUrl)) {
                ckFinderApi.openConfirmDialog("", "Press OK to delete the {$SelectedFolderName} folder".replace("{$SelectedFileName}", selectedFile.name), function (value) {
                    selectedFile.remove();
                });
            } else {
                ckFinderApi.openMsgDialog("", "The {$SelectedFileName} file cannot be deleted because it's used in one or more pages".replace("{$SelectedFileName}", selectedFile.name));
            }
        } else {
            ckFinderApi.openMsgDialog("", "No file selected");
        }
        //}
    });

    $("#RenameFile").click(function () {
        // if (IsSessionActive("/Admin/") && IsPageBrowseAuthorized("/Admin/FileManagerIsFileUsed")) {
        var selectedFile = ckFinderApi.getSelectedFile();
        if (selectedFile != null) {
            var fileUrl = selectedFile.getUrl();
            if (!IsFileUsed(fileUrl)) {
                ckFinderApi.openInputDialog("", "Please type the new file name" + ": ", selectedFile.name, function (value) {
                    if (value != "") {
                        selectedFile.rename(value);
                    }
                });
            } else {
                ckFinderApi.openMsgDialog("", "The {$SelectedFileName} file cannot be renamed because it's used in one or more pages".replace("{$SelectedFileName}", selectedFile.name));
            }
        } else {
            ckFinderApi.openMsgDialog("", "No file selected");
        }
        //}
    });

    $(".filepath-preview").each(function (index) {
        $(this).attr("id", "filepath-preview-" + index);
        swfobject.embedSWF($(this).attr("title"), "filepath-preview-" + index, "100%", "70", "9.0.0", false, false, { wmode: "transparent" }, false, false);
    });

    $(".ckfinder-file-textbox").each(function () {
        var name = $(this).attr("name");
        alert(name);
        if ($(this).val() !== "") {
            var temp = $(this).val().toLowerCase();
            if (temp.endsWith(".swf")) {
                swfobject.embedSWF($(this).val(), "ckfinder-swf-preview-" + name, "100%", "100%", "9.0.0", false, false, { wmode: "transparent" }, false, false);
                $("#ckfinder-swf-preview-" + name).removeClass("hidden");
            } else if (temp.endsWith(".jpg") || temp.endsWith(".jpeg") || temp.endsWith(".gif") || temp.endsWith(".png")) {
                $("#ckfinder-img-preview-" + name).attr("src", $(this).val()).removeClass("hidden");
            } else {
                $("#ckfinder-file-preview-" + name).attr("href", $(this).val()).removeClass("hidden");
            }
        }
    });

    $(".ckfinder-standalone").each(function (index, value) {
        var finder = new CKFinder();
        finder.basePath = "/ckfinder/";
        finder.height = 600;
        //finder.language = adminLanguageCode;
        //finder.resourceType = adminResourceType;
        finder.selectMultiple = false;
        finder.callback = function (api) {
            api.disableFileContextMenuOption("deleteFile", false);
            api.disableFileContextMenuOption("renameFile", false);
            api.disableFolderContextMenuOption("removeFolder", false);
            api.disableFolderContextMenuOption("renameFolder", false);
        };
        ckFinderApi = finder.appendTo(value);
    });

    //Usage: <button type="button" class="btn ckfinder-file" data-ckfinder-file="MyTextBox">...</button>
    $(".ckfinder-file").on("click", function () {
        var finder = new CKFinder();
        finder.basePath = "/ckfinder/";
        finder.height = 600;
        finder.language = "en";
        //finder.resourceType = $(this).attr("data-ckfinder-resourcetype");
        finder.selectActionData = $(this).attr("data-ckfinder-file");
        finder.selectMultiple = false;
        finder.selectActionFunction = function (fileUrl, data, allFiles) {
            $("#ckfinder-swf-preview-" + data["selectActionData"]).addClass("hidden");
            $("#ckfinder-img-preview-" + data["selectActionData"]).addClass("hidden");
            $("#ckfinder-file-preview-" + data["selectActionData"]).addClass("hidden");
            $("#" + data["selectActionData"]).val(fileUrl);
            var temp = fileUrl.toLowerCase();
            if (temp.endsWith(".swf")) {
                swfobject.embedSWF(fileUrl, "ckfinder-swf-preview-" + data["selectActionData"], "100%", "100%", "9.0.0", false, false, { wmode: "transparent" }, false, false);
                $("#ckfinder-swf-preview-" + data["selectActionData"]).removeClass("hidden");
            } else if (temp.endsWith(".jpg") || temp.endsWith(".jpeg") || temp.endsWith(".gif") || temp.endsWith(".png")) {
                $("#ckfinder-img-preview-" + data["selectActionData"]).attr("src", fileUrl).removeClass("hidden");
            } else {
                $("#ckfinder-file-preview-" + data["selectActionData"]).attr("href", fileUrl).removeClass("hidden");
            }
        };
        finder.callback = function (api) {
            api.disableFileContextMenuOption("deleteFile", false);
            api.disableFileContextMenuOption("renameFile", false);
            api.disableFolderContextMenuOption("removeFolder", false);
            api.disableFolderContextMenuOption("renameFolder", false);
        };
        finder.popup();
    });
    $(".ckfinder-file-remove").on("click", function () {
        var selectActionData = $(this).attr("data-ckfinder-file");
        $("#" + selectActionData).val("");
        $("#ckfinder-swf-preview-" + selectActionData).addClass("hidden");
        $("#ckfinder-img-preview-" + selectActionData).addClass("hidden");
        $("#ckfinder-file-preview-" + selectActionData).addClass("hidden");
    });
    //Usage: <button type="button" class="btn ckfinder-file-multiple" data-ckfinder-file="MyTextBox">...</button>
    $(".ckfinder-file-multiple").on("click", function () {
        var finder = new CKFinder();
        finder.basePath = "/ckfinder/";
        finder.height = 600;
        finder.language = "";
        finder.selectActionData = $(this).attr("data-ckfinder-file");
        finder.resourceType = $(this).data("ckfinder-resourcetype");
        finder.selectMultiple = true;
        finder.selectActionFunction = function (fileUrl, data, allFiles) {
            var existingTokens = $("#" + data["selectActionData"]).tokenfield('getTokens');
            var isContained;
            for (var i = 0; i < allFiles.length; i++) {
                isContained = false;
                $.each(existingTokens, function (index, token) {
                    if (token.value === allFiles[i].url) {
                        isContained = true;
                    }
                });
                if (!isContained) {
                    $("#" + data["selectActionData"]).tokenfield("createToken", allFiles[i].url);
                }
            }
        };
        finder.callback = function (api) {
            api.disableFileContextMenuOption("deleteFile", false);
            api.disableFileContextMenuOption("deleteFiles", false);
            api.disableFileContextMenuOption("renameFile", false);
            api.disableFolderContextMenuOption("removeFolder", false);
            api.disableFolderContextMenuOption("renameFolder", false);

            var toolId = api.addTool(helpSelectMultipleFiles);
            api.showTool(toolId);
        };
        finder.popup();
    });

    $(".set-segment").click(function () {
        var segmentFieldValue = $("#PageName").val().toLowerCase().replace(/ /g, "-");
        if ($("#Segment").val().trim() != "") {
            if (confirm(segmentNotEmptyPressOkToOverwrite)) {
                $("#Segment").val(segmentFieldValue);
            }
        } else {
            $("#Segment").val(segmentFieldValue);
        }
    });

    if ($(".OnChangePageTemplateId").length) {
        function onChangePageTemplateId() {
            if ($(".OnChangePageTemplateId").val() == "") {
                $("#Segment").attr("readonly", "readonly");
                $("#Segment").val("");
                $("#SegmentBtn").attr("disabled", "disabled");
                $("#Url").removeAttr("readonly");
            } else {
                $("#Url").attr("readonly", "readonly");
                $("#Url").val("");
                $("#Segment").removeAttr("readonly");
                $("#SegmentBtn").removeAttr("disabled");
            }
        }
        onChangePageTemplateId();
        $(".OnChangePageTemplateId").change(function () {
            onChangePageTemplateId();
        });
    }

    $(".page-jump").click(function (e) {
        window.location.href = $(this).attr("data-page-jump-url");
        return false;
    });

    $(".insert-at-cursor-onclick").click(function () {
        var textArea = $("#" + $(this).data("insert-at-cursor-textarea-id"));
        var insertedValue = $(this).data("insert-at-cursor-value");
        textArea.insertAtCaret(insertedValue);
        $(this).val("");
    });

    $(".insert-at-cursor-onchange").change(function () {
        var textArea = $("#" + $(this).data("insert-at-cursor-textarea-id"));
        textArea.insertAtCaret($(this).val());
        $(this).val("");
    });

    //Attaches a tooltip message to the invalid fields. It requires a CSS setting for .field-validation-error
    $(".form-group").on("focus", ".input-validation-error", function (event) {
        var errElement = $(this).next().find(":last-child");
        $(this).tooltip("destroy");
        $(this).tooltip({
            title: errElement.html(),
            container: "body"
        });
    });

    $('label[data-toggle="tooltip"]').each(function (index, value) {
        $(value).html('<span data-toggle="tooltip" title="' + $(value).attr("title") + '">' + $(value).text() + '</span>');
        $(value).removeAttr("title");
        $(value).removeAttr("data-toggle");
    });
    $('img[data-toggle="tooltip"],span[data-toggle="tooltip"],a[data-toggle="tooltip"],i[data-toggle="tooltip"],button[data-toggle="tooltip"],input[data-toggle="tooltip"]').tooltip({
        container: "body",
        html: true
    });

    $(".reset").click(function () {
        window.location.href = window.location.href.split('?')[0] + "?reset=true";
        return false;
    });

    $(".action-delete").click(function () {
        if (confirm("You are deleting the following item:\n{$ItemName}\n\nPress OK to confirm".replace("{$ItemName}", $(this).data("action-delete-item")))) {
            $(this).closest("form")
                   .attr("action", $(this).data("action"))
                   .prepend('<input type="hidden" name="deleteId" value="' + $(this).data("id") + '" />');
            return true;
        } else {
            return false;
        }
    });

    $(".action-post-id").click(function () {
        $(this).closest("form")
                .attr("action", $(this).data("action"))
                .prepend('<input type="hidden" name="postId" value="' + $(this).data("id") + '" />');
        return true;
    });

    $(".action-post").click(function () {
        $(this).closest("form")
                .attr("action", $(this).data("action"));
        return true;
    });

    $(".action-post-confirm").click(function () {
        if (confirm(toConfirmSubmitPressOK)) {
            $(this).closest("form")
                    .attr("action", $(this).data("action"));
            return true;
        } else {
            return false;
        }
    });

    $(".reset-form").click(function () {
        window.location.href = window.location.href;
        return false;
    });

    $(".redirect").click(function () {
        window.location.href = $(this).attr("data-redirect-url");
        return false;
    });

    $("form").on("click", ".submit-confirm", function () {
        if (confirm(toConfirmSubmitPressOK || "Confirm to Submit??")) {
            $(this).closest("form").submit();
        }
        return false;
    });

    if ($("form").length && typeof $("form").data("validator") !== "undefined") {
        $("form").data("validator").settings.showErrors = function (map, errors) {
            this.defaultShowErrors();
            if (errors.length) {
                $(".validation-summary-errors").removeClass("alert-success").addClass("alert-danger");
                $(".validation-summary-success").hide();
            }
        }
    }

    $("select.toggle-sql-authentication-type").change(function () {
        ToggleSqlAuthenticationType(this);
    });
    $("select.toggle-sql-authentication-type").each(function () {
        ToggleSqlAuthenticationType(this);
    });
    function ToggleSqlAuthenticationType(trigger) {
        var selectedValue = $(trigger).val();
        if (selectedValue === "IntegratedWindowsAuthentication") {
            //IntegratedWindowsAuthentication
            $("#CurrentWindowsUser").prop("disabled", false).closest(".form-group").show();
            $("#SqlUsername").prop("disabled", true).closest(".form-group").hide();
            $("#SqlPassword").prop("disabled", true).closest(".form-group").hide();
        } else {
            //SqlServerAccount
            $("#CurrentWindowsUser").prop("disabled", true).closest(".form-group").hide();
            $("#SqlUsername").prop("disabled", false).closest(".form-group").show();
            $("#SqlPassword").prop("disabled", false).closest(".form-group").show();
        }
    };

    $("select.toggle-admin-language-code").change(function () {
        $("#IsChangeAdminLanguageCode").val("true");
        $(this).closest("form")[0].submit();
    });

    $(".toggle-ignore-db-exists-warning").change(function () {
        if (!$(this).prop("checked")) {
            $(".toggle-reset-db-if-does-exists").prop("checked", false);
        }
    });

    $(".toggle-reset-db-if-does-exists").change(function () {
        if ($(this).prop("checked")) {
            $(".toggle-ignore-db-exists-warning").prop("checked", true);
        }
    });
});

//This function is invoked by SWFObject once the <object> has been created
var callback = function (e) {
    //Only execute if SWFObject embed was successful
    if (!e.success || !e.ref) {
        return false;
    };
    function swfLoadEvent(fn) {
        //Ensure fn is a valid function
        if (typeof fn !== "function") { return false; }
        //This timeout ensures we don't try to access PercentLoaded too soon
        var initialTimeout = setTimeout(function () {
            //Ensure Flash Player's PercentLoaded method is available and returns a value
            if (typeof e.ref.PercentLoaded !== "undefined" && e.ref.PercentLoaded()) {
                //Set up a timer to periodically check value of PercentLoaded
                var loadCheckInterval = setInterval(function () {
                    //Once value == 100 (fully loaded) we can do whatever we want
                    if (e.ref.PercentLoaded() === 100) {
                        //Execute function
                        fn();
                        //Clear timer
                        clearInterval(loadCheckInterval);
                    }
                }, 1500);
            }
        }, 200);
    };
    swfLoadEvent(function () {
        //Put your code here
        alert("The SWF has finished loading!");
    });
};

function CreateBlockUI() {
    var blockUIMessage = '<img src="/content/backend/images/loader.gif" width="126" height="22" />';
    $.blockUI({
        message: blockUIMessage,
        css: {
            top: ($(window).height() - 126) / 2 + 'px',
            left: ($(window).width() - 126) / 2 + 'px',
            width: '126px'
        }
    });
}

var ckFinderApi;

function SetTinyMceFileUrl(fileUrl, data, allFiles) {
    top.tinymce.activeEditor.windowManager.getParams().oninsert(fileUrl);
}

AddAntiForgeryTokenByFormField = function (data) {
    data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
    return data;
};

AddAntiForgeryTokenByField = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function IsSessionActive(redirectToPath) {
    var isSessionActive = false;
    var AntiForgeryTokenData;
    $.ajax({
        url: "/Admin/IsSessionActive/",
        type: "POST",
        data: AddAntiForgeryTokenByFormField({ AntiForgeryTokenResult: AntiForgeryTokenData }),
        cache: false,
        async: false,
        success: function (result) {
            isSessionActive = (result.toString().toLowerCase() === "true");
        }
    });
    if (!isSessionActive) {
        //alert(sessionExpiredLoginAgain);
        window.location.href = redirectToPath;
    }
    return isSessionActive;
}

function IsPageBrowseAuthorized(pageUrl) {
    var isPageUrlAuthorized = false;
    var AntiForgeryTokenData;
    var pageAction = pageUrl.split("/")[2];
    $.ajax({
        url: "/Admin/IsPageBrowseAuthorized/" + pageAction + "/",
        type: "POST",
        data: AddAntiForgeryTokenByFormField({ AntiForgeryTokenResult: AntiForgeryTokenData }),
        cache: false,
        async: false,
        success: function (result) {
            isPageUrlAuthorized = (result.toString().toLowerCase() === "true");
        }
    });
    if (!isPageUrlAuthorized) {
        alert(notAuthorizedToBrowsePage + ": " + pageAction);
    }
    return isPageUrlAuthorized;
}

function IsFileUsed(fileUrl) {
    var isFileUsed = false;
    var AntiForgeryTokenData;
    $.ajax({
        url: "/Admin/FileManagerIsFileUsed?f=" + encodeURI(fileUrl),
        type: "POST",
        data: AddAntiForgeryTokenByFormField({ AntiForgeryTokenResult: AntiForgeryTokenData }),
        cache: false,
        async: false,
        success: function (result) {
            isFileUsed = (result.toString().toLowerCase() === "true");
        }
    });
    return isFileUsed;
}
$jqGrid = {
    gridInstance: {},
    bind: function (tableId, url, colHeaderArray, ColModelArray, filterTextBox, caption) {
        $jqGrid.gridInstance =
        jQuery(tableId).jqGrid({
            autowidth: false,
            shrinkToFit: true,
            url: url,
            datatype: "json",
            colNames: colHeaderArray,
            colModel: ColModelArray,
            sortname: 'id',
            viewrecords: true,
            loadonce: true,
            pager: $('#pager'),
            sortorder: "desc",
            caption: caption || '',
            edit: {
                addCaption: "Add Record",
                editCaption: "Edit Record",
                bSubmit: "Submit",
                bCancel: "Cancel",
                bClose: "Close",
                saveData: "Data has been changed! Save changes?",
                bYes: "Yes",
                bNo: "No",
                bExit: "Cancel",
            }

        });

    },
};