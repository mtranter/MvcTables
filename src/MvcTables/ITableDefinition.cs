namespace MvcTables
{
    #region

    using System.Collections.Generic;
    using Configuration;

    #endregion

    internal interface ITableDefinition
    {
        string CssClass { get; }
        string FilterExpression { get; }
        PagingControlConfiguration PagingConfiguration { get; }
        string Id { get; }
        string Action { get; }
        string Controller { get; }
        string Area { get; }
        string DefaultSortColumn { get; }
        bool? DefaultSortAscending { get; }
    }

    internal interface ITableDefinition<in TModel> : ITableDefinition
    {
        IEnumerable<IColumnDefinition<TModel>> Columns { get; }
    }
}