// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

(function () {
    "use strict";

    WinJS.Namespace.define("OutcoldSolutions.BlogEditor", {
        BlogType: WinJS.Class.define(null, null, {
            MetaWeblog: "MetaWeblog"
        }),
        
        Blog: WinJS.Class.define(function (blogType, blogSettings, blogInfo, id) {
            this.type = blogType;
            this.settings = blogSettings;
            this.info = blogInfo;
            if (id == undefined) {
                id = OutcoldSolutions.newGuid();
            }
            this.id = id;
        }, {}, {
            serialize: function (blog) {
                var blogSettingsJson = '';
                var blogInfoJson = '';
                if (blog.type == OutcoldSolutions.BlogEditor.BlogType.MetaWeblog) {
                    blogSettingsJson = OutcoldSolutions.BlogEditor.MetaWeblog.BlogSettings.serialize(blog.settings);
                    blogInfoJson = OutcoldSolutions.BlogEditor.MetaWeblog.BlogInfo.serialize(blog.info);
                } else {
                    throw "Unknown blog provider";
                }
                var data = {
                    type: blog.type,
                    settingsJson: blogSettingsJson,
                    infoJson: blogInfoJson,
                    id: blog.id
                };
                return JSON.stringify(data);
            },
            
            deserialize: function(jsonString) {
                var data = JSON.parse(jsonString);
                var blogSettings = null;
                var blogInfo = null;
                if (data.type == OutcoldSolutions.BlogEditor.BlogType.MetaWeblog) {
                    blogSettings = OutcoldSolutions.BlogEditor.MetaWeblog.BlogSettings.deserialize(data.settingsJson);
                    blogInfo = OutcoldSolutions.BlogEditor.MetaWeblog.BlogInfo.deserialize(data.infoJson);
                } else {
                    throw "Unknown blog provider";
                }
                return new OutcoldSolutions.BlogEditor.Blog(data.type, blogSettings, blogInfo, data.id);
            }
        })
    });
})();

(function () {
    "use strict";

    var localSettings = Windows.Storage.ApplicationData.current.localSettings;
    var storageKeyConst = 'blogs';

    function loadSettings() {
        return localSettings.values[storageKeyConst];
    }
    
    function saveSettings(value) {
        localSettings.values[storageKeyConst] = value;
    }

    function saveBlogs(blogsArray) {
        var serializedBlogsArray = new Array();

        for (var i = 0; i < blogsArray.length; i++) {
            var blog = blogsArray[i];
            serializedBlogsArray.push(OutcoldSolutions.BlogEditor.Blog.serialize(blog));
        }

        saveSettings(JSON.stringify(serializedBlogsArray));
    }
    
    function loadBlogs() {
        var settings = loadSettings();

        if (settings == undefined) {
            return new Array();
        }
            
        var serializedBlogsArray = JSON.parse(settings);
        var blogsArray = new Array();
        
        for (var i = 0; i < serializedBlogsArray.length; i++) {
            var serializedBlog = serializedBlogsArray[i];
            blogsArray.push(OutcoldSolutions.BlogEditor.Blog.deserialize(serializedBlog));
        }

        return blogsArray;
    }

    WinJS.Namespace.define("OutcoldSolutions.BlogEditor", {
        
        BlogEntriesService: WinJS.Class.define(function () {
        }, {
            getBlogs: function () {
                return loadBlogs();
            },

            addBlog: function (blog) {
                var blogs = loadBlogs();
                blogs.push(blog);
                saveBlogs(blogs);
            },

            removeBlog: function (blog) {
                var removed = false;
                var blogs = loadBlogs();
                for (var i = 0; i < blogs.length; i++) {
                    if (blogs[i].id == blog.id) {
                        blogs.splice(i, 1);
                        removed = true;
                        break;
                    }
                }
                if (removed) {
                    saveBlogs(blogs);
                }
            }
        })
    });

    OutcoldSolutions.BlogEditor.BlogEntriesService.current = new OutcoldSolutions.BlogEditor.BlogEntriesService();
})();