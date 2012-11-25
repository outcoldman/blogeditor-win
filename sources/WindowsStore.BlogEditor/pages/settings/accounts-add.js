(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/pages/settings/accounts-add.html?accountType=MetaWeblogAPI", {

        ready: function (element, options) {
            WinJS.log && WinJS.log("accounts-add.html ready", "outcold", "info");

            var that = this;
            
            WinJS.Utilities.query(".win-backbutton", element).listen("click", function() {
                that._onClickGoBack();
            }, false);
            
            WinJS.Utilities.query("#btnVerifyAccount", element).listen("click", function () {
                that._onClickVerifyAccount();
            }, false);
        },

        _onClickGoBack: function () {
            WinJS.log && WinJS.log("accounts-add.html win-backbutton hanlder", "outcold", "info");
            WinJS.UI.SettingsFlyout.show();
        },
        
        _onClickVerifyAccount: function() {
            WinJS.log && WinJS.log("accounts-add.html win-backbutton hanlder", "outcold", "info");

            var blogPostUrl = document.getElementById('account-blog-post-url').value;
            var accountUserName = document.getElementById('account-username').value;
            var accountPassword = document.getElementById('account-password').value;

            var service = new MetaWeblogRegistrationService(blogPostUrl, accountUserName, accountPassword);
            service.load().done(function(result) {
                var list = new WinJS.Binding.List(result);
                list.forEach(function (userBlog) {
                    WinJS.log && WinJS.log("User blog: id - " + userBlog.blogid + ", url - " + userBlog.url + ", name - " + userBlog.blogname, "outcold", "info");
                });
            }, function (error) {
                WinJS.log && WinJS.log(error);
            });
        }
    });
})();