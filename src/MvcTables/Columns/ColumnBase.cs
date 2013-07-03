namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion

    internal abstract class ColumnBase<TModel> : IColumnConfiguration<TModel>, IColumnDefinition<TModel>
    {
        private Func<TModel, dynamic> _columnDefFunc;
        private string _name;
        private string _sortExpression;

        protected ColumnBase()
        {
            HeaderAttributes = new Dictionary<string, object>();
            FooterAttributes = new Dictionary<string, object>();
            CellAttributes = new Dictionary<string, object>();
        }

        protected string HeaderText { get; set; }

        protected Func<HtmlHelper<IEnumerable<TModel>>, IEnumerable<TModel>, string> FooterFunc { get; private set; }

        protected Func<TModel, dynamic> DataValueExpressionFunc
        {
            get
            {
                return DataValueExpression == null
                           ? null
                           : (_columnDefFunc ?? (_columnDefFunc = DataValueExpression.Compile()));
            }
        }

        protected Expression<Func<TModel, dynamic>> DataValueExpression { get; set; }

        #region IColumnConfiguration<TModel> Members

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetName(string name)
        {
            Name = name;
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetHeaderText(string headerText)
        {
            HeaderText = headerText;
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.Hide()
        {
            IsHidden = true;
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.IsSortable(bool sortable)
        {
            IsSortable = sortable;
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.DefineFooter(
            Func<HtmlHelper<IEnumerable<TModel>>, IEnumerable<TModel>, string> footerFunc)
        {
            FooterFunc = footerFunc;
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetSortProperty<TProperty>(
            Expression<Func<TModel, TProperty>> sortColumn)
        {
            SortExpression = ExpressionHelper.GetExpressionText(sortColumn);
            IsSortable = true;
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetCellCssClass(string @class)
        {
            CellAttributes = CellAttributes.Merge(new {@class});
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetHeaderCssClass(string @class)
        {
            HeaderAttributes = HeaderAttributes.Merge(new {@class});
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetFooterCssClass(string @class)
        {
            FooterAttributes = FooterAttributes.Merge(new {@class});
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetCellAttributes(object attributes)
        {
            CellAttributes = CellAttributes.Merge(attributes);
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetCellAttributes(
            IDictionary<string, object> attributes)
        {
            CellAttributes = CellAttributes.Merge(attributes);
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetHeaderAttributes(object attributes)
        {
            HeaderAttributes = HeaderAttributes.Merge(attributes);
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetHeaderAttributes(
            IDictionary<string, object> attributes)
        {
            HeaderAttributes = HeaderAttributes.Merge(attributes);
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetFooterAttributes(object attributes)
        {
            FooterAttributes = FooterAttributes.Merge(attributes);
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetFooterAttributes(
            IDictionary<string, object> attributes)
        {
            FooterAttributes = FooterAttributes.Merge(attributes);
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.SetIndex(int index)
        {
            Index = index;
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.ConfigureRawData(
            Expression<Func<TModel, dynamic>> dataValue)
        {
            DataValueExpression = dataValue;
            _columnDefFunc = dataValue.Compile();
            ShowRawValue = true;
            return this;
        }

        IColumnConfiguration<TModel> IColumnConfiguration<TModel>.RenderRawData(bool showInRawMode)
        {
            ShowRawValue = showInRawMode;
            return this;
        }

        #endregion

        #region IColumnDefinition<TModel> Members

        public abstract MvcHtmlString GetCellValue(TModel[] model, int rowIndex, ControllerContext context,
                                                   TextWriter textWriter);

        public abstract MvcHtmlString GetHeaderValue(ControllerContext context, TextWriter textWriter);

        public MvcHtmlString GetFooterValue(IEnumerable<TModel> rows, ControllerContext context, TextWriter textWriter)
        {
            var htmlHelper = HtmlHelperMock.GetHelper(rows, context, textWriter);
            return MvcHtmlString.Create(FooterFunc.ExecuteIfNotNull(htmlHelper, rows));
        }

        public string SortExpression
        {
            get { return _sortExpression; }
            protected set
            {
                _sortExpression = value;

                CellAttributes["data-sort-exp"] = _sortExpression;
                FooterAttributes["data-sort-exp"] = _sortExpression;
                HeaderAttributes["data-sort-exp"] = _sortExpression;
            }
        }

        public bool IsHidden { get; protected set; }

        public bool IsSortable { get; protected set; }

        public string Name
        {
            get { return _name; }
            protected set
            {
                _name = value;
                CellAttributes["data-column-name"] = _name;
                FooterAttributes["data-column-name"] = _name;
                HeaderAttributes["data-column-name"] = _name;
            }
        }

        public IDictionary<string, object> HeaderAttributes { get; protected set; }

        public IDictionary<string, object> CellAttributes { get; protected set; }

        public IDictionary<string, object> FooterAttributes { get; protected set; }

        public int Index { get; private set; }

        #endregion

        public virtual object GetRawValue(TModel model)
        {
            return DataValueExpressionFunc(model).ToString();
        }

        public virtual bool ShowRawValue { get; protected set; }
    }
}