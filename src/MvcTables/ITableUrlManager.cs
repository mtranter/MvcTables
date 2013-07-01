namespace MvcTables
{
    internal interface ITableUrlManager
    {
        string BaseUrl { get; }
        string SourceUrl { get; }
        string GetPagedUrl(int pageNumber);
        string GetSortUrl(string column);
        string GetPageSizeUrl(string searchTerm);
    }
}