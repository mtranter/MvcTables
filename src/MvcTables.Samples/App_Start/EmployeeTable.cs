using System;
using System.Configuration;
using System.Linq;
using MvcTables.Configuration;
using MvcTables.Samples.NorthwindEntities;

namespace MvcTables.Samples.App_Start
{
    public class EmployeeTable : MvcTable<Employee>
    {
        public override void Configure(IStaticTableConfiguration<Employee> config)
        {
            var ents = new NorthwindEntities.NorthwindEntities(new Uri(ConfigurationManager.AppSettings["NorthwindUrl"]));
            var titles = ents.Employees.Select(t => new {t.Title}).ToArray().Select(t => t.Title).Distinct();

            config
                .SetAction("ListEmployees", "Northwind")
                .SetCssClass("table table-striped")
                .HiddenColumnFor(c => c.EmployeeID, cfg => cfg.Hide())
                .EditorForColumn(e => e.FirstName)
                .EditorForColumn(e => e.LastName)
                .EditorForColumn(e => e.HomePhone)
                .DropdownColumnFor(e => e.Title, titles, s => s, s => s)
                .ConfigurePagingControl(p => p.SetContainerCssClass("pagination"));
        }
    }
}