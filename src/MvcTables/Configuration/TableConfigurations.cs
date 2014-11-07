namespace MvcTables.Configuration
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    internal class TableConfigurations
    {
        private object _lock = new object();

        private static readonly TableConfigurations Instance = new TableConfigurations();

        private readonly HashSet<MvcTableStoreNode> _tableConfigs = new HashSet<MvcTableStoreNode>();

        private TableConfigurations()
        {
        }

        public static TableConfigurations Configurations
        {
            get { return Instance; }
        }

        public void Add<TTable, TModel>(TableConfiguration<TModel> table)
            where TTable : MvcTable<TModel>
        {
            // Keep only one node per each MvcTableType
            var oldTableConfig = _tableConfigs.FirstOrDefault(x => x.MvcTableType == typeof(TTable));
            if (oldTableConfig != null)
            {
                _tableConfigs.Remove(oldTableConfig);
            }

            _tableConfigs.Add(new MvcTableStoreNode(table, typeof (TTable)));
        }

        internal ITableConfiguration<TModel> BuildDefault<TModel>(string action, string controller, string area)
        {
            var table = new DefaultMvcTable<TModel>(action, controller, area);
            var config = GetDefaultTableConfiguration<TModel>();
            table.Configure(config);
            this.Add<DefaultMvcTable<TModel>, TModel>(config);
            return config;
        }

        internal ITableDefinition<TModel> Get<TTable, TModel>()
            where TTable : MvcTable<TModel>
        {
            return this.Get<TModel>(typeof(TTable));
        }

        internal ITableDefinition<TModel> Get<TModel>(Type tableType)
        {
            var config =
                _tableConfigs.FirstOrDefault(t => t.MvcTableType == tableType);

            if (config != null)
            {
                return config.Table as ITableDefinition<TModel>;
            }
            return null;
        }

        internal ITableDefinition<TModel> GetOrLoadDefault<TTable, TModel>(string action, string controller, string area)
            where TTable : MvcTable<TModel>
        {
            return this.Get<TTable, TModel>() ?? (BuildDefault<TModel>(action, controller, area) as ITableDefinition<TModel>);
        }

        public TableConfiguration<TModel> GetDefaultTableConfiguration<TModel>()
        {
            return new TableConfiguration<TModel>();
        }

        class MvcTableStoreNode
        {
            public MvcTableStoreNode(ITableDefinition table, Type mvcTableType)
            {
                Table = table;
                MvcTableType = mvcTableType;
            }

            internal ITableDefinition Table { get; private set; }
            internal Type MvcTableType { get;private set; }
        }
    }
}