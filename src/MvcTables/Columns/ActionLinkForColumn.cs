namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    #endregion

    internal class ActionLinkForColumn<TModel, TColumn> : ReflectedColumnBase<TModel, TColumn>
    {
        private readonly string _action;
        private readonly string _controller;
        private readonly IDictionary<string, object> _htmlAttributes;
        private readonly Func<TModel, object> _routeValues;

        public ActionLinkForColumn(Expression<Func<TModel, TColumn>> columnDefinition, string action, string controller,
                                   Func<TModel, object> routeValues, IDictionary<string, object> htmlAttributes)
            : base(columnDefinition)
        {
            _action = action;
            _controller = controller;
            _routeValues = routeValues;
            _htmlAttributes = htmlAttributes;
        }

        protected override MvcHtmlString GetCellValueCore(TModel[] model, int rowIndex, TColumn column,
                                                          HtmlHelper<TModel[]> helper)
        {
            var formattedValue = FormatValue(column);
            if(String.IsNullOrEmpty(formattedValue))
                return new MvcHtmlString(String.Empty);
            return helper.ActionLink(formattedValue, _action, _controller,
                                     new RouteValueDictionary(_routeValues(model[rowIndex])), _htmlAttributes);
        }
    }
}