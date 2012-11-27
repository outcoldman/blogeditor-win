// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model
{
    public class BlogInfo
    {
        public BlogInfo(string id, string name, string url)
        {
            this.Id = id;
            this.Name = name;
            this.Url = url;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Url { get; private set; }
    }
}