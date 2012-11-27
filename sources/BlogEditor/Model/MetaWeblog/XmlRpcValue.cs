// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model.MetaWeblog
{
    public class XmlRpcValue : XmlRpcEntity
    {
        public XmlRpcValue(object value)
            : base(XmlRpcEntityType.Value)
        {
            this.Value = value;
        }

        public object Value { get; private set; }
    }
}