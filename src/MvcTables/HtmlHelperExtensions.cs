namespace MvcTables
{
    #region

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    #endregion

    internal static class HtmlHelperExtensions
    {
        internal static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                    string partialViewName)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            var viewData = new ViewDataDictionary(helper.ViewData)
                               {
                                   TemplateInfo = new TemplateInfo
                                                      {
                                                          HtmlFieldPrefix = name
                                                      }
                               };

            return helper.Partial(partialViewName, model, viewData);
        }

        internal static MvcHtmlString DropdownListFor<TModel, TEnum>(this HtmlHelper<TModel> helper,
                                                                     Expression<Func<TModel, TEnum>> property)
        {
            var selected = property.Compile()(helper.ViewData.Model);
            var items =
                Enum.GetValues(typeof (TEnum))
                    .Cast<TEnum>()
                    .Select(
                            e =>
                            new SelectListItem
                                {
                                    Text = e.ToString(),
                                    Value = e.ToString(),
                                    Selected = (int) (object) e == (int) (object) selected
                                });
            return helper.DropDownListFor(property, items);
        }
    }
}