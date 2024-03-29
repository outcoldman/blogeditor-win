﻿// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

(function () {
    "use strict";
    var page = WinJS.UI.Pages.define("/pages/settings/accounts.html", {

        _selectAccountTypes : null,

        ready: function (element, options) {
            WinJS.log && WinJS.log("accounts.html ready", "outcold", "info");

            var that = this;

            this._selectAccountTypes = document.getElementById('selectAccountTypes');

            var btnAddAccount = document.getElementById("btnAddAccount");
            btnAddAccount.addEventListener("click", function() {
                 that._doClickAddAccount();
            }, false);

            var blogs = OutcoldSolutions.BlogEditor.BlogEntriesService.current.getBlogs();
            WinJS.Utilities.query('#listBlogs', element).forEach(function(lv) {
                lv.winControl.itemDataSource = (new WinJS.Binding.List(blogs)).dataSource;
            });
        },

        _doClickAddAccount: function() {
            WinJS.log && WinJS.log("btnAddAccount handler", "outcold", "info");
            WinJS.log && WinJS.log("Selected account type " + this._selectAccountTypes.value, "outcold", "info");

            if (this._selectAccountTypes.value == "MetaWeblog") {
                WinJS.UI.SettingsFlyout.showSettings("accounts-add", "/pages/settings/accounts-add.html?accountType=" + this._selectAccountTypes.value);
            } else {
                WinJS.log && WinJS.log("Unexpected account type " + this._selectAccountTypes.value, "outcold", "error");
            }
        },
    });
})();