// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    public sealed class MetaWeblogSupport
    {
        public static string GetUserBlogsRequestBody(string userName, string password)
        {
            return CreateXmlBody("blogger.getUsersBlogs", new[] { string.Empty, userName, password }).ToString();
        }

        public static IList<UserBlog> ParseUserBlogsResponse(string response)
        {
            var methodResponse = XDocument.Parse(response);
            var structs =
                methodResponse.Element(XmlRpcProtocol.MethodResponse)
                              .Element(XmlRpcProtocol.Params)
                              .Element(XmlRpcProtocol.Param)
                              .Element(XmlRpcProtocol.Value)
                              .Element(XmlRpcProtocol.Array)
                              .Element(XmlRpcProtocol.Data)
                              .Elements(XmlRpcProtocol.Value)
                              .Elements(XmlRpcProtocol.Struct);

            List<UserBlog> blogs = new List<UserBlog>();

            foreach (var s in structs)
            {
                string blogid = null;
                string url = null;
                string blogName = null;

                foreach (var member in s.Elements())
                {
                    switch (member.Element(XmlRpcProtocol.Name).Value.ToUpperInvariant())
                    {
                        case "BLOGID":
                            blogid = member.Element(XmlRpcProtocol.Value).Element(XmlRpcProtocol.String).Value;
                            break;
                        case "URL":
                            url = member.Element(XmlRpcProtocol.Value).Element(XmlRpcProtocol.String).Value;
                            break;
                        case "BLOGNAME":
                            blogName = member.Element(XmlRpcProtocol.Value).Element(XmlRpcProtocol.String).Value;
                            break;
                    }
                }

                blogs.Add(new UserBlog(blogid, blogName, url));
            }

            return blogs;
        }

        private static XElement CreateXmlBody(string methodName, IEnumerable<object> parameters)
        {
            var methodCall = new XElement(XmlRpcProtocol.MethodCall);
            methodCall.Add(new XElement(XmlRpcProtocol.MethodName, new XText(methodName)));
            methodCall.Add(
                new XElement(
                    XmlRpcProtocol.Params,
                    parameters.Select(
                        p =>
                            {
                                var stringValue = p as string;
                                if (stringValue != null)
                                {
                                    return new XElement(
                                        XmlRpcProtocol.Param,
                                        new XElement(
                                            XmlRpcProtocol.Value,
                                            new XElement(XmlRpcProtocol.String, new XText(stringValue))));
                                }
                                else
                                {
                                    throw new NotSupportedException(
                                        string.Format(CultureInfo.CurrentCulture, "{0} is not supported", p.GetType()));
                                }
                            })));
            return methodCall;
        }
    }
}
