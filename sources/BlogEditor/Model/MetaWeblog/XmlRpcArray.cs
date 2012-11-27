// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model.MetaWeblog
{
    using System.Collections.Generic;

    public class XmlRpcArray : XmlRpcEntity
    {
        public XmlRpcArray()
            : base(XmlRpcEntityType.Array)
        {
            this.Entities = new List<XmlRpcEntity>();
        }

        public IList<XmlRpcEntity> Entities { get; private set; }
    }
}