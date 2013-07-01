namespace MvcTables
{
    #region

    using Render;

    #endregion

    public class TableRequestModel
    {
        private const int DefaultPageSize = 10;

        public TableRequestModel()
        {
            SortAscending = true;
            PageNumber = 1;
            PageSize = DefaultPageSize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string SortColumn { get; set; }

        public bool SortAscending { get; set; }

        public TableRenderFormat Format { get; set; }
    }
}