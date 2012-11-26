// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

(function () {
    "use strict";

    WinJS.Namespace.define("OutcoldSolutions.BlogEditor.MetaWeblog", {
        BlogSettings : WinJS.Class.define(function(userName, password, url) {
            this.userName = userName;
            this.password = password;
            this.url = url;
            this.savePassword = false;
        }, {}, {
            serialize: function (blogEntry) {
                var data = {
                    userName: blogEntry.userName,
                    url: blogEntry.url,
                    savePassword: blogEntry.savePassword
                };
                
                if (blogEntry.savePassword) {
                    // TODO: understand how to store sensetive data
                    data.password = blogEntry.password;
                }

                return JSON.stringify(data);
            },
            
            deserialize : function(jsonString) {
                var data = JSON.parse(jsonString);
                var blogSettings = new OutcoldSolutions.BlogEditor.MetaWeblog.BlogSettings(data.userName, data.password, data.url);
                blogSettings.savePassword = data.savePassword;
                return blogSettings;
            }
        }),
        
        BlogInfo : WinJS.Class.define(function(id, url, name) {
            this.id = id;
            this.url = url;
            this.name = name;
        }, {}, {
            serialize: function(blogInfo) {
                return JSON.stringify(blogInfo);
            },
            
            deserialize: function(jsonString) {
                var data = JSON.parse(jsonString);
                return new OutcoldSolutions.BlogEditor.MetaWeblog.BlogInfo(data.id, data.url, data.name);
            }
        })
    });
})();