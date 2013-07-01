namespace MvcTables.Render
{
    #region

    using System;

    #endregion

    internal class CsvTableRender<TModel> : StringTableRenderBase<TModel>
    {
        public CsvTableRender(ITableDefinition<TModel> tableDefinition) : base(tableDefinition)
        {
        }

        protected override string ContentType
        {
            get { return "text/csv"; }
        }

        protected override string BeginColumnsValue
        {
            get { return String.Empty; }
        }

        protected override string ClosingValue
        {
            get { return String.Empty; }
        }

        protected override string ValueDelimiter
        {
            get { return ", "; }
        }

        protected override string BeginRowValue
        {
            get { return Environment.NewLine; }
        }

        protected override string EndRowValue
        {
            get { return String.Empty; }
        }

        protected override string EndColumnsValue
        {
            get { return String.Empty; }
        }

        protected override string BeginRowsValue
        {
            get { return String.Empty; }
        }

        protected override string EndRowsValue
        {
            get { return String.Empty; }
        }
    }
}