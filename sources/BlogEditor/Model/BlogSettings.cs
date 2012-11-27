// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model
{
    public class BlogSettings
    {
        public BlogSettings(string urlBlogAPI, string userName, string password)
        {
            this.UrlBlogAPI = urlBlogAPI;
            this.UserName = userName;
            this.Password = password;
        }

        public string UrlBlogAPI { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }
    }
}