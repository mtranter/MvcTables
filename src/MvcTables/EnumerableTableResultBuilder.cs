namespace MvcTables
{
    using System.Collections.Generic;
    using Configuration;

    class EnumerableTableResultBuilder<TModel> : IEnumerableTableResultBuilder<TModel>
    {
        private readonly IEnumerable<TModel> _model;

        public EnumerableTableResultBuilder(IEnumerable<TModel> model)
        {
            _model = model;
        }

        public TableResult<TTable, TModel> Build<TTable>(TableRequestModel model, int totalRows) where TTable : MvcTable<TModel>
        {
            return new TableResult<TTable, TModel>(_model, totalRows, model);
        }

        public TableResult<DefaultMvcTable<TModel>, TModel> Default(TableRequestModel model, int totalRows)
        {
            return new TableResult<DefaultMvcTable<TModel>, TModel>(_model, totalRows, model);
        }
    }
}