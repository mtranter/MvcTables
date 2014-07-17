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
        
        public IEnumerable<IColumnDefinition<TModel>> Columns
        {
            get
            {
                var staticColumns = _staticModel.Columns.ToArray();
                var runtimeColumns = _runtimeDefiniton.Columns.ToArray();
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