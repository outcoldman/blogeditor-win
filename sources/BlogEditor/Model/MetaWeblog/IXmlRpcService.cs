// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model.MetaWeblog
{
    public interface IXmlRpcService
    {
        string CreateRequestBody(string methodName, params object[] parameters);

        XmlRpcResponse ParseResponse(string responseBody);
    }
}