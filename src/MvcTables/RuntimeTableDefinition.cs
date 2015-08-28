namespace MvcTables
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using Configuration;

    #endregion

    internal class RuntimeTableDefinition<TModel> : ITableDefinition<TModel>
    {
        private readonly ITableDefinition<TModel> _runtimeDefiniton;
        private readonly ITableDefinition<TModel> _staticModel;

        public RuntimeTableDefinition(ITableDefinition<TModel> staticModel, ITableDefinition<TModel> runtimeDefiniton)
        {
            _staticModel = staticModel;
            _runtimeDefiniton = runtimeDefiniton;
        }

        public string CssClass
        {
            get { return _runtimeDefiniton.IfIsNull(r => r.CssClass) ?? _staticModel.CssClass; }
        }

        public string DefaultSortColumn
        {
            get
            {
                return _runtimeDefiniton.IfIsNull(r => r.DefaultSortColumn) ?? _staticModel.DefaultSortColumn;
            }
        }

        public bool? DefaultSortAscending
        {
            get
            {
                return _runtimeDefiniton.IfIsNull(r=>r.DefaultSortAscending) ?? _staticModel.DefaultSortAscending;
            }
        }

        public int? DefaultPageSize
        {
            get
            {
                return _runtimeDefiniton.IfIsNull(r => r.DefaultPageSize) ?? _staticModel.DefaultPageSize;
            }
        }

        public IList<string> HiddenColumns { get { return _runtimeDefiniton.HiddenColumns; }}

        public IEnumerable<IColumnDefinition<TModel>> Columns
        {
            get
            {
                var staticColumns = _staticModel.Columns.Where(definition => !HiddenColumns.Contains(definition.Name)).ToArray();
                var runtimeColumns = _runtimeDefiniton.Columns.Where(definition => !HiddenColumns.Contains(definition.Name)).ToArray();
                for (var i = 0; i < staticColumns.Length; i++)
                {
                    var runtimeCol = runtimeColumns.FirstOrDefault(r => r.Index == i);
                    if (runtimeCol != null)
                    {
                        yield return runtimeCol;
                    }
                    yield return staticColumns[i];
                }
                foreach (var col in runtimeColumns.Where(c => c.Index >= staticColumns.Length))
                {
                    yield return col;
                }
            }
        }

        public PagingControlConfiguration PagingConfiguration
        {
            get
            {
                if (_runtimeDefiniton != null && _runtimeDefiniton.PagingConfiguration != null &&
                    !_runtimeDefiniton.PagingConfiguration.IsDefault)
                {
                    return _runtimeDefiniton.PagingConfiguration;
                }

                return _staticModel.PagingConfiguration;

            }
        }

        public string Id
        {
            get { return _staticModel.Id; }
        }

        public string FilterExpression
        {
            get { return _runtimeDefiniton.IfIsNull(c => c.FilterExpression) ?? _staticModel.FilterExpression; }
        }

        #region ITableDefinition Members


        public string Action
        {
            get { return _staticModel.Action; }
        }

        public string Controller
        {
            get { return _staticModel.Controller; }
        }

        public string Area
        {
            get { return _staticModel.Area; }
        }

        #endregion
    }
}