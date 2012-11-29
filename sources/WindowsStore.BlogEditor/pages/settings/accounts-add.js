// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

(function () {
    "use strict";

    var wizzardPages = {
        Progress: 0,
        AddAccount: 1,
        ListBlogs: 2
    };
    
    var currentPage = wizzardPages.AddAccount;
    var loadPromise = null;
    var blogInfoViewModels = new WinJS.Binding.List();

    function showWizardPage(pageIndex) {
        WinJS.Utilities.query('.win-settings-section').addClass('hidden');
        WinJS.Utilities.query('#wizzard-page' + pageIndex).removeClass('hidden');
        currentPage = pageIndex;
    }

    function onClickGoBack() {
        WinJS.log && WinJS.log("accounts-add.html win-backbutton hanlder", "outcold", "info");
        
        if (currentPage != wizzardPages.AddAccount) {
            if (loadPromise != null) {
                loadPromise.cancel();
                loadPromise = null;
            }
            
            blogInfoViewModels.splice(0, blogInfoViewModels.length);
            showWizardPage(wizzardPages.AddAccount);
        } else {
            WinJS.UI.SettingsFlyout.show();
        }
    }

    function onClickVerifyAccount() {
        WinJS.log && WinJS.log("accounts-add.html win-backbutton hanlder", "outcold", "info");

        showWizardPage(wizzardPages.Progress);

        var url = document.getElementById('account-url').value;
        var userName = document.getElementById('account-username').value;
        var password = document.getElementById('account-password').value;

        var blogSettings = new OutcoldSolutions.BlogEditor.MetaWeblog.BlogSettings(userName, password, url);
        var service = new OutcoldSolutions.BlogEditor.MetaWeblog.MetaWeblogRegistrationService(blogSettings);
        loadPromise = service.load();
        loadPromise.done(function (result) {
            if (loadPromise != null) {
                if (result.length > 0) {
                    blogInfoViewModels.splice(0, blogInfoViewModels.length);

                    for (var i = 0; i < result.length; i++) {
                        WinJS.log && WinJS.log("User blog: id - " + result[i].id + ", url - " + result[i].url + ", name - " + result[i].name, "outcold", "info");
                        blogInfoViewModels.push({ info: result[i], settings: blogSettings });
                    }

                    showWizardPage(wizzardPages.ListBlogs);
                } else {
                    // TODO: show no blogs
                }
                loadPromise = null;
            }
        }, function (error) {
            if (loadPromise != null) {
                WinJS.log && WinJS.log(error);
                showWizardPage(wizzardPages.AddAccount);
                loadPromise = null;
                // TODO: show error
            }
        });
    }

    var page = WinJS.UI.Pages.define("/pages/settings/accounts-add.html?accountType=MetaWeblog", {

        ready: function (element, options) {
            WinJS.log && WinJS.log("accounts-add.html ready", "outcold", "info");
            var listBlogInfos = document.getElementById('listBlogInfos');

            WinJS.Utilities.query(".win-backbutton", element).listen("click", function() {
                onClickGoBack();
            }, false);
            
            WinJS.Utilities.query("#btnVerify", element).listen("click", function () {
                onClickVerifyAccount();
            }, false);

            WinJS.Utilities.query("#btnAddAccount", element).listen('click', function () {
                blogInfoViewModels.forEach(function(vm) {
                    if (vm.checked) {
                        var blog = new OutcoldSolutions.BlogEditor.Blog(OutcoldSolutions.BlogEditor.BlogType.MetaWeblog, vm.settings, vm.info);
                        OutcoldSolutions.BlogEditor.BlogEntriesService.current.addBlog(blog);
                    }
                });
                
                WinJS.UI.SettingsFlyout.show();
            }, false);
           
            listBlogInfos.winControl.itemDataSource = blogInfoViewModels.dataSource;
            listBlogInfos.addEventListener('iteminvoked', function (eventInfo) {
                eventInfo.detail.itemPromise.then(function (invokedItem) {
                    listBlogInfos.winControl.selection.getItems().done(function(selectedItems) {
                        WinJS.Utilities.query("#btnAddAccount", element).forEach(function (btn) {
                            btn.disabled = selectedItems.length == 0;;
                        });
                    });
                });
            }, false);
        }
    });
})();