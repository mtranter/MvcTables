namespace MvcTables.Samples.App_Start
{
    #region

    using System;
    using System.Configuration;
    using System.Linq;
    using Configuration;
    using NorthwindEntities;

    #endregion

    public class EmployeeTable : MvcTable<Employee>
    {
        public override void Configure(IStaticTableConfiguration<Employee> config)
        {
            var ents = new NorthwindEntities(new Uri(ConfigurationManager.AppSettings["NorthwindUrl"]));
            var titles = ents.Employees.Select(t => new {t.Title}).ToArray().Select(t => t.Title).Distinct();

            config
                .SetCssClass("table table-striped")
                .HiddenColumnFor(c => c.EmployeeID, cfg => cfg.Hide())
                .EditorForColumn(e => e.FirstName)
                .EditorForColumn(e => e.LastName)
                .EditorForColumn(e => e.HomePhone)
                .DropdownColumnFor(e => e.Title, titles, s => s, s => s)
                .ConfigurePagingControl(p => p.SetContainerCssClass("pagination"));
        }
    }

    public class ReadonlyEmployeeConfigurator : MvcTable<Employee>
    {
        public override void Configure(IStaticTableConfiguration<Employee> config)
        {
            config
                .SetCssClass("table table-striped")
                .DisplayForColumn(e => e.EmployeeID, cfg => cfg.IsSortable(false))
                .DisplayForColumn(e => e.FirstName, cfg => cfg.IsSortable(false))
                .DisplayForColumn(e => e.LastName, cfg => cfg.IsSortable(false))
                .DisplayForColumn(e => e.HomePhone, cfg => cfg.IsSortable(false))
                .DisplayForColumn(e => e.Title, cfg => cfg.IsSortable(false))
                .ConfigurePagingControl(p => p.SetContainerCssClass("pagination"));
        }
    }
}