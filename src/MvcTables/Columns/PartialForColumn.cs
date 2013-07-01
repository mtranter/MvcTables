namespace MvcTables
{
    #region

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion

    internal class PartialForColumn<TModel, TColumn> : ReflectedColumnBase<TModel, TColumn>
    {
        private readonly string _partialName;

        public PartialForColumn(Expression<Func<TModel, TColumn>> columnDefinition, string partialName)
            : base(columnDefinition)
        {
            _partialName = partialName;
            IsSortable = false;
        }

        protected override MvcHtmlString GetCellValueCore(TModel[] model, int rowIndex, TColumn cellValue,
                                                          HtmlHelper<TModel[]> helper)
        {
            return helper.PartialFor(ArrayIndexExpression(model, rowIndex), _partialName);
        }
    }
}