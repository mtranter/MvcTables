namespace MvcTables
{
    #region

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    #endregion

    internal class EditorForColumn<TModel, TColumn> : ReflectedColumnBase<TModel, TColumn>
    {
        public EditorForColumn(Expression<Func<TModel, TColumn>> columnDefinition)
            : base(columnDefinition)
        {
        }

        protected override MvcHtmlString GetCellValueCore(TModel[] model, int rowIndex, TColumn cellValue,
                                                          HtmlHelper<TModel[]> helper)
        {
            var exp = ArrayIndexExpression(model, rowIndex);
            var name = ExpressionHelper.GetExpressionText(exp);
            var fullHtmlFieldName = helper
                .ViewContext
                .ViewData
                .TemplateInfo
                .GetFullHtmlFieldName(name);
            return helper.EditorFor(exp);
        }
    }
}