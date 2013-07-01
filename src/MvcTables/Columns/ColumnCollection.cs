namespace MvcTables
{
    #region

    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    internal class ColumnCollection<TModel> : IEnumerable<ColumnBase<TModel>>
    {
        private readonly List<ColumnBase<TModel>> _inner = new List<ColumnBase<TModel>>();

        public void Add(ColumnBase<TModel> column)
        {
            ((IColumnConfiguration<TModel>) column).SetIndex(column.Index == 0 ? _inner.Count : column.Index);
            _inner.Add(column);
        }

        #region IEnumerable<IColumnDefinition<TModel>> Members

        public IEnumerator<ColumnBase<TModel>> GetEnumerator()
        {
            return _inner.OrderBy(c => c.Index).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}