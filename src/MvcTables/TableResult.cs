namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web.Mvc;
    using Configuration;
    using Render;

    #endregion

    public static class TableResult
    {
        public static IQueryableTableResultBuilder<TModel> From<TModel>(IQueryable<TModel> model)
        {
            return new QueryableTableResultBuilder<TModel>(model);
        }

        public static IEnumerableTableResultBuilder<TModel> From<TModel>(IEnumerable<TModel> model)
        {
            return new EnumerableTableResultBuilder<TModel>(model);
        }
    }

    public class TableResult<TTable, TModel> : ActionResult
        where TTable : MvcTable<TModel>
    {
        private readonly string _configName;
        private readonly Lazy<TableConfiguration<TModel>> _override = new Lazy<TableConfiguration<TModel>>();
        private readonly IQueryable<TModel> _rows;
        private readonly TableRequestModel _tableRequest;
        private readonly int _totalResults;

        public TableResult(IEnumerable<TModel> rows, int totalResults, TableRequestModel tableRequest)
            : this(rows, totalResults, typeof (TModel).FullName, tableRequest)
        {
        }

        public TableResult(IEnumerable<TModel> rows, string configName, TableRequestModel tableRequest)
            : this(rows, rows.Count(), configName, tableRequest)
        {
        }

        public TableResult(IEnumerable<TModel> rows, int totalResults, string configName, TableRequestModel tableRequest)
        {
            _rows = rows.AsQueryable();
            _totalResults = totalResults;
            _configName = configName;
            _tableRequest = tableRequest;
        }

        public TableResult(IQueryable<TModel> rows, TableRequestModel tableRequest)
            : this(rows, rows.Count(), typeof (TModel).FullName, tableRequest)
        {
        }

        public TableResult(IQueryable<TModel> rows, string configName, TableRequestModel tableRequest)
            : this(rows, rows.Count(), configName, tableRequest)
        {
        }

        public TableResult(IQueryable<TModel> rows, int totalResults, string configName, TableRequestModel tableRequest)
        {
            _rows = rows;
            _totalResults = totalResults;
            _configName = configName;
            _tableRequest = tableRequest;
        }

        public ITableConfiguration<TModel> Overrides
        {
            get { return _override.Value; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var runtimeConfig = GetTableDefinition(context);

            var urlManager = InitUrlManager(context);

            var paginator = new Paginator(urlManager, _totalResults, _tableRequest.PageSize, 8, _tableRequest.PageNumber);

            if (BoolValueExistsAndIsTrue(HtmlConstants.RenderTableRouteValue, context) || !BoolValueExistsAndIsTrue(HtmlConstants.RenderPaginationRouteValue, context))
            {
                if (String.IsNullOrEmpty(_tableRequest.SortColumn))
                {
                    var firstSortable = runtimeConfig.Columns.FirstOrDefault(c => c.IsSortable);
                    if (firstSortable != null)
                    {
                        _tableRequest.SortColumn = firstSortable.SortExpression;
                    }
                }

                var tableRender = TableRenderFactory.Get(runtimeConfig, _tableRequest, urlManager);
                tableRender.Render(_rows.PaginateRows(_tableRequest), _tableRequest, context);
            }

            if (BoolValueExistsAndIsTrue(HtmlConstants.RenderPaginationRouteValue, context))
            {
                var pageRender = new HtmlPaginationRender(paginator);
                pageRender.RenderPagination(runtimeConfig.PagingConfiguration, runtimeConfig.Id, context);
            }
        }

        private ITableDefinition<TModel> GetTableDefinition(ControllerContext ctx)
        {
            var routeVals = ctx.RouteData.Values;
            var area = routeVals.ContainsKey("area") && routeVals["area"] != null ? routeVals["area"].ToString() : null;
            var tableDefinition = TableConfigurations.Configurations.GetOrLoadDefault<TTable, TModel>(routeVals["action"].ToString(), routeVals["controller"].ToString(), area);

            var runtimeConfig = _override.IsValueCreated
                                    ? new RuntimeTableDefinition<TModel>(tableDefinition, _override.Value)
                                    : tableDefinition;
            return runtimeConfig;
        }

        private TableUrlManager InitUrlManager(ControllerContext context)
        {
            var urlHelper = new UrlHelper(context.RequestContext);
            var url = urlHelper.Action(context.RouteData.Values["action"].ToString(),
                                       context.RouteData.Values["controller"].ToString(),
                                       context.RouteData.Values.ContainsKey("id")
                                           ? new {id = context.RouteData.Values["id"]}
                                           : null);
            var qsVals = SanitizeQueryString(context.RequestContext.HttpContext.Request.QueryString);
            var urlManager = new TableUrlManager(url, _tableRequest, qsVals);
            return urlManager;
        }

        private NameValueCollection SanitizeQueryString(NameValueCollection nameValueCollection)
        {
            var clone = new NameValueCollection(nameValueCollection);
            foreach (var key in new[] {HtmlConstants.RenderPaginationRouteValue, HtmlConstants.RenderTableRouteValue})
            {
                if (clone.AllKeys.Contains(key))
                {
                    clone.Remove(key);
                }
            }
            return clone;
        }

        private bool BoolValueExistsAndIsTrue(string key, ControllerContext context)
        {
            return (context.RouteData.Values.ContainsKey(key) &&
                    (bool) context.RouteData.Values[key]) ||
                   (context.HttpContext.Request.QueryString.AllKeys.Contains(key) &&
                    context.HttpContext.Request.QueryString[key].Equals("true",
                                                                        StringComparison.CurrentCultureIgnoreCase));
        }
    }
}