

namespace MvcTables
{
    #region

    using System.Collections.Specialized;
    using System.Linq;
    using StaticReflection;
    #endregion

    internal class TableUrlManager : ITableUrlManager
    {
        private readonly string _actionUrl;
        private readonly NameValueCollection _urlParams;

        public TableUrlManager(string actionUrl, TableRequestModel requestModel, NameValueCollection routeValues)
        {
            _actionUrl = actionUrl;
            _urlParams = routeValues.Merge(requestModel);
        }

        public string BaseUrl
        {
            get { return _actionUrl; }
        }

        public string SourceUrl
        {
            get { return _actionUrl + GetSeperator() + _urlParams.ToQueryString(); }
        }

        public string GetPagedUrl(int pageNumber)
        {
            var pageNumberProp = StaticReflection.GetMember<TableRequestModel, int>((t) => t.PageNumber);
            return BaseUrl + GetSeperator() +
                   CloneValues(pageNumberProp.Name, pageNumber.ToString(), _urlParams).ToQueryString();
        }

        public string GetSortUrl(string column)
        {
            if (string.IsNullOrEmpty(column))
            {
                return null;
            }
            var sortColProp = StaticReflector.GetMember<TableRequestModel, string>((t) => t.SortColumn);
            var sortDirProp = StaticReflector.GetMember<TableRequestModel, bool>((t) => t.SortAscending);
            var sorted = column.Equals(_urlParams[sortColProp.Name]);
            var ascending = false;
            if (sorted)
            {
                ascending = _urlParams[sortDirProp.Name] == "True";
            }
            return BaseUrl + GetSeperator() +
                   CloneValues(sortDirProp.Name, (!ascending).ToString(),
                               CloneValues(sortColProp.Name, column, _urlParams)).ToQueryString();
        }

        public string GetPageSizeUrl(string pageSize)
        {
            var pageSizeProp = StaticReflection.GetMember<TableRequestModel, int>((t) => t.PageSize);
            return BaseUrl + GetSeperator() + CloneValues(pageSizeProp.Name, pageSize, _urlParams).ToQueryString();
        }

        public string GetSeperator()
        {
            return (_actionUrl.Contains("?") ? "&" : "?");
        }

        public NameValueCollection CloneValues(string exceptKey, string newValue, NameValueCollection dict)
        {
            var clone = new NameValueCollection();

            foreach (var key in dict.AllKeys.Where(s => s != exceptKey))
            {
                clone[key] = dict[key];
            }

            clone[exceptKey] = newValue;

            return clone;
        }
    }
}