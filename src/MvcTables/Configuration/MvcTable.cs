namespace MvcTables.Configuration
{


    public abstract class MvcTable<TModel> : ITableConfigurator
    {
        public abstract void Configure(IStaticTableConfiguration<TModel> config);
    }

    public sealed class DefaultMvcTable<TModel> : MvcTable<TModel>
    {
        private readonly string _action;
        private readonly string _controller;
        private readonly string _area;

        internal DefaultMvcTable(string action, string controller, string area)
        {
            _action = action;
            _controller = controller;
            _area = area;
        }

        public override void Configure(IStaticTableConfiguration<TModel> config)
        {
            config.SetAction(_action, _controller, _area).ScaffoldAllColumns();
        }
    }

    public sealed class ViewConfigedMvcTable<TModel> : MvcTable<TModel>
    {
        public override void Configure(IStaticTableConfiguration<TModel> config)
        {
            // All configuration shall be set up outside this class.
        }
    }
}