using System.Collections.Generic;
using System.Linq;

namespace MvcTables.Configuration
{
    internal class PagingControlConfiguration : IPagingControlConfiguration
    {
        public PagingControlConfiguration(ITableDefinition tableDefinition)
        {
            TableDefinition = tableDefinition;
            ContainerCssClass = "table-pagination";
            DisabledCssClass = "disabled";
            ActiveCssClass = "active";
            FirstPageText = "<<";
            PreviousPageText = "<";
            NextPageText = ">";
            LastPageText = ">>";
            IsDefault = true;
        }

        public string ContainerCssClass { get; private set; }
        public string DisabledCssClass { get; private set; }
        public string ActiveCssClass { get; private set; }
        public string FirstPageText { get; private set; }
        public string LastPageText { get; private set; }
        public string NextPageText { get; private set; }
        public string PreviousPageText { get; private set; }
        public IEnumerable<int> PageSizes { get; private set; }
        public ITableDefinition TableDefinition { get; private set; }

        internal bool IsDefault { get; private set; }

        #region IPagingControlConfiguration Members

        IPagingControlConfiguration IPagingControlConfiguration.SetContainerCssClass(string @class)
        {
            ContainerCssClass = @class;
            IsDefault = false;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetDisabledClass(string @class)
        {
            DisabledCssClass = @class;
            IsDefault = false;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetActiveClass(string @class)
        {
            ActiveCssClass = @class;
            IsDefault = false;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetPreviousPageText(string text)
        {
            PreviousPageText = text;
            IsDefault = false;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetNextPageText(string text)
        {
            NextPageText = text;
            IsDefault = false;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetFirstPageText(string text)
        {
            FirstPageText = text;
            IsDefault = false;
            return this;
        }

        IPagingControlConfiguration IPagingControlConfiguration.SetLastPageText(string text)
        {
            LastPageText = text;
            IsDefault = false;
            return this;
        }

        public IPagingControlConfiguration SetPageSizes(params int[] sizes)
        {   
            PageSizes = sizes;
            IsDefault = false;
            return this;
        }

        #endregion
    }
}