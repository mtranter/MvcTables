namespace MvcTables
{
    public class Page
    {
        public Page(int pageNumber, string url, bool isCurrent = false)
        {
            PageNumber = pageNumber;
            Url = url;
            IsCurrent = isCurrent;
        }

        public int PageNumber { get; private set; }

        public string Url { get; private set; }

        public bool IsCurrent { get; private set; }
    }
}