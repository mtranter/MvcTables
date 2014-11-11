using MvcTables.Configuration;
using MvcTables.Samples.NorthwindEntities;

namespace MvcTables.Samples.App_Start
{
    public class OrderTable : MvcTable<Order>
    {
        public override void Configure(IStaticTableConfiguration<Order> config)
        {
            config.SetAction("ListOrders", "Northwind")
                  .SetCssClass("table table-striped")
                  .SetDefaultPageSize(15)
                  .HiddenColumnFor(c => c.OrderID, cfg => cfg.Hide())
                  .ActionLinkColumn(c => c.Customer.City, "Category", "Home")
                  .DisplayForColumn(c => c.Shipper.CompanyName)
                  .DisplayForColumn(c => c.ShipRegion)
                  .DisplayForColumn(c => c.Order_Details[0].Quantity)
                  .ConfigurePagingControl(p => p.SetContainerCssClass("pagination"));
        }
    }
}