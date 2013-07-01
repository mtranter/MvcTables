using MvcTables.Configuration;
using MvcTables.Samples.NorthwindEntities;

namespace MvcTables.Samples.App_Start
{
    public class OrderFilterTable : MvcTable<Order>
    {
        public override void Configure(IStaticTableConfiguration<Order> config)
        {
            config.SetAction("ListOrdersForFilters", "Northwind")
                  .SetCssClass("table table-striped")
                  .HiddenColumnFor(c => c.OrderID, cfg => cfg.Hide())
                  .DisplayForColumn(c => c.Customer.ContactName)
                  .DisplayForColumn(c => c.Customer.City)
                  .DisplayForColumn(c => c.Shipper.CompanyName)
                  .DisplayForColumn(c => c.ShipRegion)
                  .DisplayForColumn(c => c.Order_Details[0].Quantity)
                  .ConfigurePagingControl(p => p.SetContainerCssClass("pagination"));
        }
    }
}