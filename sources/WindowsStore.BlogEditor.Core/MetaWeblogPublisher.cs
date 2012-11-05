// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor
{
    using System;
    using System.Threading.Tasks;

    using Windows.Foundation;

    public sealed class MetaWeblogPublisher
    {
        public IAsyncOperation<bool> PublishAsync(string title, string text)
        {
            return this.PublishInternal(title, text).AsAsyncOperation();
        }

        private async Task<bool> PublishInternal(string title, string text)
        {
            return true;
        }
    }
}
