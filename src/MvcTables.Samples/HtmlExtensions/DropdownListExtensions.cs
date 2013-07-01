namespace MvcTables.Samples.HtmlExtensions
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    #endregion

    public static class DropdownListExtensions
    {
        public static MvcHtmlString DropDownListFor<TModel, TProperty, TCollection>(this HtmlHelper<TModel> helper,
                                                                                    Expression<Func<TModel, TProperty>>
                                                                                        property,
                                                                                    IEnumerable<TCollection> items,
                                                                                    Func<TCollection, TProperty> value,
                                                                                    Func<TCollection, object> display)
        {
            return helper.DropDownListFor(property, items, value, display, null);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty, TCollection>(this HtmlHelper<TModel> helper,
                                                                                    Expression<Func<TModel, TProperty>>
                                                                                        property,
                                                                                    IEnumerable<TCollection> items,
                                                                                    Func<TCollection, TProperty> value,
                                                                                    Func<TCollection, object> display,
                                                                                    object htmlAttributes)
        {
            var selectItems =
                items.Select(
                             i =>
                             new SelectListItem
                                 {
                                     Text = i == null ? "" : display(i).ToString(),
                                     Value = i == null ? "" : value(i).ToString(),
                                     Selected =
                                         i == null ? true : value(i).Equals(property.Compile()(helper.ViewData.Model))
                                 });

            return helper.DropDownListFor(property, selectItems, htmlAttributes);
        }
    }
}