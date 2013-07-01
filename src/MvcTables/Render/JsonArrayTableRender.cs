namespace MvcTables.Render
{
    internal class JsonArrayTableRender<TModel> : StringTableRenderBase<TModel>
    {
        public JsonArrayTableRender(ITableDefinition<TModel> tableDefinition)
            : base(tableDefinition)
        {
        }

        protected override string ContentType
        {
            get { return "application/json"; }
        }

        protected override string BeginColumnsValue
        {
            get { return "{columns:["; }
        }

        protected override string ClosingValue
        {
            get { return "}"; }
        }

        protected override string ValueDelimiter
        {
            get { return ", "; }
        }

        protected override string BeginRowValue
        {
            get { return "["; }
        }

        protected override string EndRowValue
        {
            get { return "]"; }
        }

        protected override string EndColumnsValue
        {
            get { return "], "; }
        }

        protected override string BeginRowsValue
        {
            get { return "data:["; }
        }

        protected override string EndRowsValue
        {
            get { return "]"; }
        }
    }
}