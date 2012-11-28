// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

(function () {
    "use strict";

    // use this document for creating XML
    var doc = document.implementation.createDocument(null, null, null);

    // function that creates the XML structure
    function x() {
        var node = doc.createElement(arguments[0]), text, child;

        for (var i = 1; i < arguments.length; i++) {
            child = arguments[i];
            if (typeof child == 'string') {
                child = doc.createTextNode(child);
            }
            node.appendChild(child);
        }

        return node;
    };

    function parseUserBlogs(xml) {
        var blogs = new Array();
        
        var items = xml.querySelectorAll("methodResponse > params > param > value > array > data > value > struct");

        for (var i = 0; i < items.length; i++) {
            var nodeStruct = items[i];
            var nodesBlogInfoProperties = nodeStruct.querySelectorAll('member');
            var blogInfoProperties = {};
            for (var j = 0; j < nodesBlogInfoProperties.length; j++) {
                var nodeProperty = nodesBlogInfoProperties[j];
                var nodeName = nodeProperty.querySelector('name');
                var nodeValue = nodeProperty.querySelector('value');
                blogInfoProperties[nodeName.textContent.toLowerCase()] = nodeValue.textContent;
            }
            blogs.push(new OutcoldSolutions.BlogEditor.MetaWeblog.BlogInfo(blogInfoProperties['blogid'], blogInfoProperties['url'], blogInfoProperties['blogname']));
        }

        return blogs;
    };

    WinJS.Namespace.define("OutcoldSolutions.BlogEditor.MetaWeblog", {
        MetaWeblogRegistrationService: WinJS.Class.define(function (blogSettings) {
            this._blogSettings = blogSettings;
        }, {
            load: function () {

                var xml = x('methodCall',
                    x('methodName', 'blogger.getUsersBlogs'),
                    x('params',
                        x('param',
                            x('value',
                                x('string', '')
                            )
                        ),
                        x('param',
                            x('value',
                                x('string', this._blogSettings.userName)
                            )
                        ),
                        x('param',
                            x('value',
                                x('string', this._blogSettings.password)
                            )
                        )
                    )
                );
                
               
                var requestBody = new XMLSerializer().serializeToString(xml).toString();

                return WinJS.xhr({
                    type: "post",
                    url: this._blogSettings.url,
                    headers: { "Content-Type": "text/xml; charset=UTF-8", "Content-Length": requestBody.length },
                    data: requestBody
                }).then(function (result) {
                    return parseUserBlogs(result.responseXML);
                });
            }
        })
    });
})();
