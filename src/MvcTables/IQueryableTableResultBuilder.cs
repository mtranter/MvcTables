namespace MvcTables
{
    using Configuration;

    /// <summary>
    /// Helps provide some syntactic sugar for creating <see cref="TableResult{TTable,TModel}"/> instances
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IQueryableTableResultBuilder<TModel> : IEnumerableTableResultBuilder<TModel>
    {
        TableResult<TTable, TModel> Build<TTable>(TableRequestModel model) where TTable : MvcTable<TModel>;
    }
}