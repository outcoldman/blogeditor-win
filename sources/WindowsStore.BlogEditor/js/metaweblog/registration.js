// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

(function () {
    "use strict";

    var xmlDocument = Windows.Data.Xml.Dom.XmlDocument;

    function parseUserBlogs(body) {
        var document = new xmlDocument();
        document.loadXml(body);
        // get methodResponse
        var nodesBlogInfos = document.selectNodes('methodResponse/params/param/value/array/data/value/struct');

        var blogs = new Array();

        for (var i = 0; i < nodesBlogInfos.length; i++) {
            var nodeStruct = nodesBlogInfos[i];
            var nodesBlogInfoProperties = nodeStruct.selectNodes('member');
            var blogInfoProperties = {};
            for (var j = 0; j < nodesBlogInfoProperties.length; j++) {
                var nodeProperty = nodesBlogInfoProperties[j];
                var nodeName = nodeProperty.selectSingleNode('name');
                var nodeValue = nodeProperty.selectSingleNode('value');
                blogInfoProperties[nodeName.innerText.toLowerCase()] = nodeValue.innerText;
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
                var document = new xmlDocument();
                var methodCall = document.createElement('methodCall');
                document.appendChild(methodCall);

                var methodName = document.createElement('methodName');
                methodName.innerText = 'blogger.getUsersBlogs';
                methodCall.appendChild(methodName);

                var params = document.createElement('params');
                var paramValues = ['', this._blogSettings.userName, this._blogSettings.password];
                for (var i = 0; i < paramValues.length; i++) {
                    var nodeParam = document.createElement('param');
                    var nodeValue = document.createElement('value');
                    nodeValue.innerText = paramValues[i];
                    nodeParam.appendChild(nodeValue);
                    params.appendChild(nodeParam);
                }

                methodCall.appendChild(params);

                var requestBody = document.getXml();

                return WinJS.xhr({
                    type: "post",
                    url: this._blogSettings.url,
                    headers: { "Content-Type": "text/xml; charset=UTF-8", "Content-Length": requestBody.length },
                    data: requestBody
                }).then(function (result) {
                    return parseUserBlogs(result.responseText);
                });
            }
        })
    });
})();
