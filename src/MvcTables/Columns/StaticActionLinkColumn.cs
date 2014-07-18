using System.Web.Routing;

namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    #endregion

    internal class StaticActionLinkColumn<TModel> : ColumnBase<TModel>
    {
        private readonly string _action;
        private readonly string _controller;
        private readonly IDictionary<string, object> _htmlAttributes;
        private readonly string _linkText;
        private readonly Func<TModel, object> _routeValues;

        public StaticActionLinkColumn(string linkText, string action, string controller,
                                      Func<TModel, object> routeValues, IDictionary<string, object> htmlAttributes)
        {
            _linkText = linkText;
            _action = action;
            _controller = controller;
            _routeValues = routeValues ?? ((m) => null);
            _htmlAttributes = htmlAttributes;
        }

        public override MvcHtmlString GetCellValue(TModel[] model, int rowIndex, ControllerContext context,
                                                   TextWriter textWriter)
        {
            var helper = HtmlHelperMock.GetHelper(model, context, textWriter);
            return helper.ActionLink(_linkText, _action, _controller, new RouteValueDictionary(_routeValues(model[rowIndex])), _htmlAttributes);
        }

        public override MvcHtmlString GetHeaderValue(ControllerContext context, TextWriter textWriter)
        {
            return MvcHtmlString.Create(HeaderText);
        }
    }
}