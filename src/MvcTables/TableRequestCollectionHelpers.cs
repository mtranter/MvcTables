namespace MvcTables
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    #endregion

    internal static class TableRequestCollectionHelpers
    {
        public static IQueryable<TModel> PaginateRows<TModel>(this IQueryable<TModel> rows, TableRequestModel model)
        {
            return
                rows.SortBy(model.SortColumn, model.SortAscending)
                    .Skip(model.PageSize*(model.PageNumber - 1))
                    .Take(model.PageSize);
        }

        public static IEnumerable<TModel> PaginateRows<TModel>(this IEnumerable<TModel> rows, TableRequestModel model)
        {
            return
                rows.SortBy(model.SortColumn, model.SortAscending)
                    .Skip(model.PageSize*(model.PageNumber - 1))
                    .Take(model.PageSize);
        }
    }
}