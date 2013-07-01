namespace MvcTables.Configuration
{
    internal class PagingControlConfiguration : IPagingControlConfiguration
    {
        public PagingControlConfiguration()
        {
            ContainerCssClass = "table-pagination";
            DisabledCssClass = "disabled";
            ActiveCssClass = "active";
            FirstPageText = "<<";
            PreviousPageText = "<";
            NextPageText = ">";
            LastPageText = ">>";
        }

        public string ContainerCssClass { get; private set; }
        public string DisabledCssClass { get; private set; }
        public string ActiveCssClass { get; private set; }
        public string FirstPageText { get; private set; }
        public string LastPageText { get; private set; }
        public string NextPageText { get; private set; }
        public string PreviousPageText { get; private set; }

        #region IPagingControlConfiguration Members

        IPagingControlConfiguration IPagingControlConfiguration.SetContainerCssClass(string @class)
        {
            ContainerCssClass = @class;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetDisabledClass(string @class)
        {
            DisabledCssClass = @class;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetActiveClass(string @class)
        {
            ActiveCssClass = @class;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetPreviousPageText(string text)
        {
            PreviousPageText = text;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetNextPageText(string text)
        {
            NextPageText = text;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetFirstPageText(string text)
        {
            FirstPageText = text;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetLastPageText(string text)
        {
            LastPageText = text;
            return this;
        }

        #endregion
    }
}