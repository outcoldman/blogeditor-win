// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model.MetaWeblog
{
    using System;
    using System.Collections.Generic;

    internal class MetaWeblogRegistrationService
    {
        private readonly IXmlRpcService xmlRpcService;

        public MetaWeblogRegistrationService(IXmlRpcService xmlRpcService)
        {
            this.xmlRpcService = xmlRpcService;
        }

        public IEnumerable<BlogInfo> Load(BlogSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            return null;
        }
    }
}
