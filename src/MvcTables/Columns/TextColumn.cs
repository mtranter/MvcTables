namespace MvcTables
{
    #region

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion

    internal class TextColumn<TModel, TColumn> : ReflectedColumnBase<TModel, TColumn>
    {
        private readonly Func<TModel, TColumn> _value;

        public TextColumn(Expression<Func<TModel, TColumn>> value)
            : base(value)
        {
            _value = value.Compile();
        }

        protected override MvcHtmlString GetCellValueCore(TModel[] model, int rowIndex, TColumn cellValue,
                                                          HtmlHelper<TModel[]> helper)
        {
            return MvcHtmlString.Create(FormatUsingFunction(_value(model[rowIndex])));
        }
    }
}