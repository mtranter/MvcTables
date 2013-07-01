namespace MvcTables
{
    using System.Linq;
    using Configuration;

    class QueryableTableResultBuilder<TModel> : IQueryableTableResultBuilder<TModel>
    {
        private readonly IQueryable<TModel> _model;

        public QueryableTableResultBuilder(IQueryable<TModel> model)
        {
            _model = model;
        }

        public TableResult<TTable, TModel> Build<TTable>(TableRequestModel model) where TTable : MvcTable<TModel>
        {
            return new TableResult<TTable, TModel>(_model, model);
        }

        public TableResult<TTable, TModel> Build<TTable>(TableRequestModel model, int totalRows) where TTable : MvcTable<TModel>
        {
            return new TableResult<TTable, TModel>(_model, totalRows, model);
        }
    }
}