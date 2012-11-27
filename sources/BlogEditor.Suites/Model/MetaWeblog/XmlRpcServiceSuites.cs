// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Suites.Model.MetaWeblog
{
    using System.Linq;
    using System.Xml.Linq;

    using NUnit.Framework;

    using OutcoldSolutions.BlogEditor.Model.MetaWeblog;

    public class XmlRpcServiceSuites : SuitesBase
    {
        public override void OnTestFixtureSetUp()
        {
            base.OnTestFixtureSetUp();

            using (var registration = this.Container.Registration())
            {
                registration.Register<XmlRpcService>();
            }
        }

        [Test]
        public void CreateRequestBody_ArrayOfSimpleValues_ShouldCreateRequest()
        {
            // Arrange
            var service = this.Container.Resolve<XmlRpcService>();

            // Act
            var request = service.CreateRequestBody("blogger.getUsersBlogs", string.Empty, "user", "password");

            // Assert
            var xRequest = XElement.Parse(request);
            Assert.AreEqual("methodCall", xRequest.Name.LocalName);
            Assert.AreEqual("blogger.getUsersBlogs", xRequest.Elements("methodName").First().Value);

            var xParams = xRequest.Element("params");
            Assert.AreEqual(string.Empty, xParams.Elements("param").First().Element("value").Element("string").Value);
            Assert.AreEqual("user", xParams.Elements("param").Skip(1).First().Element("value").Element("string").Value);
            Assert.AreEqual("password", xParams.Elements("param").Skip(2).First().Element("value").Element("string").Value);
        }

        [Test]
        public void ParseResponse_GetUsersBlogsResponseSample_TwoBlogs()
        {
            // Arrange
            const string Response = @"<methodResponse><params><param><value><array><data><value><struct><member><name>blogid</name><value><string>ru</string></value></member><member><name>url</name><value><string>http://outcoldman.com//ru/blog/index</string></value></member><member><name>blogName</name><value><string>admin ru</string></value></member></struct></value><value><struct><member><name>blogid</name><value><string>en</string></value></member><member><name>url</name><value><string>http://outcoldman.com//en/blog/index</string></value></member><member><name>blogName</name><value><string>admin en</string></value></member></struct></value></data></array></value></param></params></methodResponse>";
            var service = this.Container.Resolve<XmlRpcService>();

            // Act
            var xmlRpcResponse = service.ParseResponse(Response);

            // Assert
            Assert.AreEqual(1, xmlRpcResponse.Parameters.Count);

            var array = xmlRpcResponse.Parameters[0];
            Assert.AreEqual(XmlRpcEntityType.Array, array.EntityType);
            Assert.AreEqual(2, ((XmlRpcArray)array).Entities.Count);

            var struct1 = (XmlRpcStruct)((XmlRpcArray)array).Entities[0];
            Assert.AreEqual(XmlRpcEntityType.Struct, struct1.EntityType);
            Assert.AreEqual("ru", ((XmlRpcValue)struct1.Members["blogid"]).Value);
            Assert.AreEqual("http://outcoldman.com//ru/blog/index", ((XmlRpcValue)struct1.Members["url"]).Value);
            Assert.AreEqual("admin ru", ((XmlRpcValue)struct1.Members["blogName"]).Value);

            var struct2 = (XmlRpcStruct)((XmlRpcArray)array).Entities[1];
            Assert.AreEqual(XmlRpcEntityType.Struct, struct2.EntityType);
            Assert.AreEqual("en", ((XmlRpcValue)struct2.Members["blogid"]).Value);
            Assert.AreEqual("http://outcoldman.com//en/blog/index", ((XmlRpcValue)struct2.Members["url"]).Value);
            Assert.AreEqual("admin en", ((XmlRpcValue)struct2.Members["blogName"]).Value);
        }
    }
}
