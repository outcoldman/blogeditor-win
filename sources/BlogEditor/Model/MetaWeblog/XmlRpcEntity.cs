// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model.MetaWeblog
{
    public class XmlRpcEntity
    {
        public XmlRpcEntity(XmlRpcEntityType entityType)
        {
            this.EntityType = entityType;
        }

        public XmlRpcEntityType EntityType { get; private set; }
    }
}