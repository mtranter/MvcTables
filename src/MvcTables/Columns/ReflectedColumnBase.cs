namespace MvcTables
{
    #region

    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion

    internal abstract class ReflectedColumnBase<TModel, TColumn> : ColumnBase<TModel>,
                                                                   IFormatableColumnConfiguration<TModel, TColumn>
    {
        private Func<TModel, TColumn> _columnDefinitionFunc;
        private string _formatString = "{0}";

        protected ReflectedColumnBase(Expression<Func<TModel, TColumn>> columnDefinition)
        {
            Name = SortExpression = ExpressionHelper.GetExpressionText(columnDefinition);
            ColumnDefinition = columnDefinition;
            IsSortable = true;
            ShowRawValue = true;
            FormatUsingFunction = c => c.ToString();
        }

        protected Expression<Func<TModel, TColumn>> ColumnDefinition { get; private set; }
        protected Func<TColumn, string> FormatUsingFunction { get; private set; }

        protected TColumn GetCellObject(TModel[] model, int rowIndex)
        {
            return ArrayIndexExpression(model, rowIndex).Compile()(model);
        }

        protected string FormatValue(TColumn value)
        {
            return FormatUsingFunction == null ? String.Format(_formatString, value) : FormatUsingFunction(value);
        }

        public override MvcHtmlString GetCellValue(TModel[] model, int rowIndex, ControllerContext context,
                                                   TextWriter textWriter)
        {
            var mock = HtmlHelperMock.GetHelper(model, context, textWriter);
            return GetCellValueCore(model, rowIndex, GetCellObject(model, rowIndex), mock);
        }

        public override MvcHtmlString GetHeaderValue(ControllerContext context, TextWriter textWriter)
        {
            if (!String.IsNullOrEmpty(HeaderText))
            {
                return MvcHtmlString.Create(HeaderText);
            }

            try
            {
                var modelMetadata = ModelMetadata.FromLambdaExpression(ColumnDefinition,
                                                                       new ViewDataDictionary<TModel>());
                return MvcHtmlString.Create(modelMetadata.GetDisplayName());
            }
            catch
            {
                return MvcHtmlString.Empty;
            }
        }

        protected abstract MvcHtmlString GetCellValueCore(TModel[] model, int rowIndex, TColumn cellValue,
                                                          HtmlHelper<TModel[]> helper);

        protected Expression<Func<TModel[], TColumn>> ArrayIndexExpression(TModel[] source, int rowIndex)
        {
            var visitor = new InstanceToIndexExpressionMaker<TModel>(rowIndex);
            return (Expression<Func<TModel[], TColumn>>) visitor.Visit(ColumnDefinition);
        }

        public override object GetRawValue(TModel model)
        {
            if (ShowRawValue)
            {
                if (DataValueExpressionFunc == null)
                {
                    var val = (_columnDefinitionFunc ?? (_columnDefinitionFunc = ColumnDefinition.Compile()))(model);
                    return FormatValue(val);
                }
                return FormatValue(DataValueExpressionFunc(model));
            }

            return null;
        }

        #region IFormatableColumnConfiguration<TModel,TColumn> Members

        public IColumnConfiguration<TModel> SetFormatString(string format)
        {
            _formatString = format;
            return this;
        }

        public IColumnConfiguration<TModel> FormatUsing(Func<TColumn, string> formatUsing)
        {
            FormatUsingFunction = formatUsing;
            return this;
        }

        #endregion
    }
}