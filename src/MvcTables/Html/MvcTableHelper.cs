namespace MvcTables.Html
{
    #region

    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Configuration;
    using Render;
    using System.Linq;

    #endregion

    public class MvcTableHelper
    {
        private readonly HtmlHelper _helper;
        private readonly ITableDefinition _tableDefinition;

        internal MvcTableHelper(HtmlHelper helper, ITableDefinition tableDefinition)
        {
            _helper = helper;
            _tableDefinition = tableDefinition;
        }

        public MvcHtmlString Table()
        {
            return Render(true, false);
        }

        public MvcHtmlString Pagination()
        {
            return Render(false, true);
        }

        private MvcHtmlString Render(bool table, bool pager)
        {
            var routevals = new RouteValueDictionary();
            routevals[HtmlConstants.RenderTableRouteValue] = table;
            routevals[HtmlConstants.RenderPaginationRouteValue] = pager;
            routevals["area"] = _tableDefinition.Area;
            return _helper.Action(_tableDefinition.Action, _tableDefinition.Controller, routevals);
        }
    }
}