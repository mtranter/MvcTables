using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.Practices.Unity.Utility;

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

    public class MvcTableHelper<TModel>
    {
        private readonly HtmlHelper _helper;
        private readonly ITableDefinition _tableDefinition;
        private readonly TableRequestModel _model;

        internal MvcTableHelper(HtmlHelper helper, ITableDefinition tableDefinition)
            :this(helper, tableDefinition, new TableRequestModel())
        {
        }
        
        internal MvcTableHelper(HtmlHelper helper, ITableDefinition tableDefinition, TableRequestModel model)
        {
            _helper = helper;
            _tableDefinition = tableDefinition;
            _model = model;
        }

        public MvcHtmlString Table()
        {
            return Render(true, false);
        }

        public MvcHtmlString Pagination()
        {
            return Render(false, true);
        }

        public MvcHtmlString DropdownFilter(string name, IEnumerable<SelectListItem> items)
        {
            var attributes = BuildAttributesWithFilterClass(null);
            return _helper.DropDownList(name, items, attributes);
        }

        public MvcHtmlString TextBoxFilter(string name)
        {
            return TextBoxFilter(name, null);
        }

        public MvcHtmlString TextBoxFilter(string name, object htmlAttributes)
        {
            var attributes = BuildAttributesWithFilterClass(htmlAttributes);
            return _helper.TextBox(name, null, attributes);
        }

        public string FilterClass
        {
            get { return _tableDefinition.FilterExpression; }
        }

        public MvcHtmlString PageSizer(params int[] pageSizeOptions)
        {
            return PageSizer(pageSizeOptions, null);
        }

        public MvcHtmlString PageSizer(IEnumerable<int> pageSizeOptions, object htmlAttributes)
        {
            var name = StaticReflection.StaticReflection.GetMember((TableRequestModel m) => m.PageSize).Name;

            var attributes = BuildAttributesWithFilterClass(htmlAttributes);
            return _helper.DropDownList(name,
                                        pageSizeOptions.Select(
                                            p =>
                                            new SelectListItem()
                                            {
                                                Text = p.ToString(CultureInfo.InvariantCulture),
                                                Value = p.ToString(CultureInfo.InvariantCulture),
                                                Selected = _model.PageSize == p
                                            }), attributes);
        }

        private RouteValueDictionary BuildAttributesWithFilterClass(object htmlAttributes)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            attributes.WithClass( _tableDefinition.FilterExpression).WithAttribute("data-target", _tableDefinition.Id);
            return attributes;
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