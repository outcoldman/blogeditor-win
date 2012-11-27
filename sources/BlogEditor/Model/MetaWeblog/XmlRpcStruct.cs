// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model.MetaWeblog
{
    using System.Collections.Generic;

    public class XmlRpcStruct : XmlRpcEntity
    {
        public XmlRpcStruct()
            : base(XmlRpcEntityType.Struct)
        {
            this.Members = new Dictionary<string, XmlRpcEntity>();
        }

        public IDictionary<string, XmlRpcEntity> Members { get; private set; }
    }
}