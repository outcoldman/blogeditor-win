// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor
{
    public sealed class UserBlog
    {
        public UserBlog(string blogId, string blogName, string url)
        {
            this.BlogId = blogId;
            this.BlogName = blogName;
            this.Url = url;
        }

        public string BlogId { get; private set; }

        public string BlogName { get; private set; }

        public string Url { get; private set; }
    }
}