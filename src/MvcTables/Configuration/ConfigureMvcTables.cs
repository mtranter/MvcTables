namespace MvcTables.Configuration
{
    #region

    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    #endregion

    public class ConfigureMvcTables : IAssemblyConfiguration
    {

        private static readonly Lazy<ConfigureMvcTables> Instance = new Lazy<ConfigureMvcTables>();

        void IAssemblyConfiguration.As<TType>()
        {
            FromAssembly(typeof (TType).Assembly);
        }

        void IAssemblyConfiguration.As(Type type)
        {
            FromAssembly(type.Assembly);
        }

        public static void FromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(t => typeof (ITableConfigurator).IsAssignableFrom(t));
            foreach (var t in types)
            {
                var config = Activator.CreateInstance(t);
                var tmodel = t.BaseType.GetGenericArguments()[0];
                var tableConfigType = typeof (TableConfiguration<>).MakeGenericType(tmodel);
                var tableConfig = DependencyResolver.Current.GetService(tableConfigType);
                var configMethod = typeof (MvcTable<>).MakeGenericType(tmodel).GetMethod("Configure");
                configMethod.Invoke(config, new[] {tableConfig});

                var addMethod =
                    typeof (TableConfigurations).GetMethods()
                                                .First(m => m.Name == "Add" && m.GetParameters().Length == 1)
                                                .MakeGenericMethod(config.GetType(), tmodel);
                addMethod.Invoke(TableConfigurations.Configurations,
                                 new[] {tableConfig});
            }
        }

        public static IAssemblyConfiguration InTheSameAssembly
        {
            get { return Instance.Value; }
        }
    }
}