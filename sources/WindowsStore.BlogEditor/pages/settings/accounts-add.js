(function () {
    "use strict";
    var metaWeblogSupport = OutcoldSolutions.BlogEditor.MetaWeblogSupport;

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

            var requestBody = metaWeblogSupport.getUserBlogsRequestBody(accountUserName, accountPassword);

            WinJS.xhr({
                type: "post",
                url: blogPostUrl,
                headers: { "Content-Type": "text/xml; charset=UTF-8", "Content-Length": requestBody.length },
                data: requestBody
            }).done(function (result) {
                WinJS.log && WinJS.log(result);
                var userBlogsResponse = metaWeblogSupport.parseUserBlogsResponse(result.responseText);
                var list = new WinJS.Binding.List(userBlogsResponse);
                list.forEach(function(userBlog) {
                    WinJS.log && WinJS.log("User blog: id - " + userBlog.blogId + ", url - " + userBlog.url + ", name - " + userBlog.blogName, "outcold", "info");
                });
            }, function(error) {
                WinJS.log && WinJS.log(error);
            });
        }
    });
})();