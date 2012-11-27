// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model.MetaWeblog
{
    using System.Collections.Generic;

    public class XmlRpcResponse
    {
        public XmlRpcResponse()
        {
            this.Parameters = new List<XmlRpcEntity>();
        }

        public IList<XmlRpcEntity> Parameters { get; private set; }
    }
}