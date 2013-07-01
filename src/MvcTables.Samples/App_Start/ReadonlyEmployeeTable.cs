namespace MvcTables.Samples.App_Start
{
    #region

    using Configuration;
    using NorthwindEntities;

    #endregion

    public class ReadonlyEmployeeTable : MvcTable<Employee>
    {
        public override void Configure(IStaticTableConfiguration<Employee> config)
        {
            config
                .SetAction("ReadonlyEmployees", "Northwind")
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