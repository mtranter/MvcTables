namespace MvcTables.Render
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    internal static class HtmlConstants
    {
        internal const string MvctableContainer = "mvctable-container";
        internal const string InternalFilterClass = "mvctable-trigger";
        internal const string RenderPaginationRouteValue = "RenderPager";
        internal const string RenderTableRouteValue = "RenderTable";
    }

    internal class HtmlTableRender<TModel> : ITableRender<TModel>
    {
        private readonly IPaginator _paginator;
        private readonly ITableDefinition<TModel> _tableDefinition;
        private readonly ITableUrlManager _urlManager;

        public HtmlTableRender(ITableDefinition<TModel> tableDefinition, ITableUrlManager urlManager,
                               IPaginator paginator)
        {
            _tableDefinition = tableDefinition;
            _urlManager = urlManager;
            _paginator = paginator;
        }

        public void Render(IEnumerable<TModel> rows, TableRequestModel model, ControllerContext context)
        {
            var writer = WriterHelper.GetWriter(context);
            var myId = _tableDefinition.Id;

            using (
                new ComplexContentTag("div", new Dictionary<string, object> {{"class", HtmlConstants.MvctableContainer}},
                                      writer))
            {
                using (
                    new ComplexContentTag("form", new Dictionary<string, object> {{"action", _urlManager.BaseUrl}},
                                          writer)
                    )
                {
                    using (new ComplexContentTag("table",
                                                 new Dictionary<string, object>
                                                     {
                                                         {"class", "mvctable " + _tableDefinition.CssClass},
                                                         {"data-source", _urlManager.BaseUrl},
                                                         {"data-table-id", myId},
                                                         {"data-filter", _tableDefinition.FilterExpression}
                                                     }, writer))
                    {
                        using (new ComplexContentTag("thead", writer))
                        {
                            using (new ComplexContentTag("tr", writer))
                            {
                                foreach (var col in _tableDefinition.Columns)
                                {
                                    var attributes = col.HeaderAttributes;
                                    var sortAsc = model.SortAscending;
                                    var isSorted = model.SortColumn == col.SortExpression;
                                    var sortUrl = _urlManager.GetSortUrl(col.SortExpression);
                                    var classMap = new[]
                                                       {
                                                           new
                                                               {
                                                                   Apply = col.IsSortable,
                                                                   @class = "sortable"
                                                               },
                                                           new
                                                               {
                                                                   Apply = isSorted,
                                                                   @class = sortAsc ? "sorted-asc" : "sorted-desc"
                                                               }
                                                       };
                                    attributes["class"] = attributes.ContainsKey("class") ? attributes["class"] : "";
                                    foreach (var cm in classMap.Where(cm => cm.Apply))
                                    {
                                        attributes["class"] += (" " + cm.@class);
                                    }
                                    if (col.IsHidden)
                                    {
                                        col.HeaderAttributes.WithStyle("display", "none");
                                    }

                                    using (new ComplexContentTag("th", col.HeaderAttributes, writer))
                                    {
                                        if (col.IsSortable)
                                        {
                                            using (
                                                new ComplexContentTag("a",
                                                                      new RouteValueDictionary(new {href = sortUrl})
                                                                          .WithClass(HtmlConstants.InternalFilterClass),
                                                                      writer))
                                            {
                                                writer.Write(col.GetHeaderValue(context, writer));
                                            }
                                        }
                                        else
                                        {
                                            writer.Write(col.GetHeaderValue(context, writer));
                                        }
                                    }
                                }
                            }
                        }

                        using (new ComplexContentTag("tbody", writer))
                        {
                            var aRows = rows.ToArray();
                            for (var i = 0; i < aRows.Length; i++)
                            {
                                using (new ComplexContentTag("tr", writer))
                                {
                                    foreach (var col in _tableDefinition.Columns)
                                    {
                                        if (col.IsHidden)
                                        {
                                            col.CellAttributes.WithStyle("display", "none");
                                        }
                                        using (new ComplexContentTag("td", col.CellAttributes, writer))
                                        {
                                            writer.Write(col.GetCellValue(aRows, i, context, writer));
                                        }
                                    }
                                }
                            }
                        }

                        using (new ComplexContentTag("tfoot", writer))
                        {
                            using (new ComplexContentTag("tr", writer))
                            {
                                foreach (var col in _tableDefinition.Columns)
                                {
                                    using (new ComplexContentTag("td", col.CellAttributes, writer))
                                    {
                                        writer.Write(col.GetFooterValue(rows, context, writer));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            writer.Flush();
        }
    }
}