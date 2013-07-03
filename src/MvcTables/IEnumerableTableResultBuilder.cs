namespace MvcTables
{
    using Configuration;

    /// <summary>
    /// Helps provide some syntactic sugar for creating <see cref="TableResult{TTable,TModel}"/> instances
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IEnumerableTableResultBuilder<TModel>
    {
        TableResult<TTable, TModel> Build<TTable>(TableRequestModel model, int totalRows) where TTable : MvcTable<TModel>;

        TableResult<DefaultMvcTable<TModel>, TModel> Default(TableRequestModel model, int totalRows);
    }
}