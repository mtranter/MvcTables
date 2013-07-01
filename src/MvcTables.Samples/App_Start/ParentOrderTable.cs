using MvcTables.Configuration;
using MvcTables.Samples.NorthwindEntities;

namespace MvcTables.Samples.App_Start
{
    public class ParentOrderTable : MvcTable<Order>
    {
        public override void Configure(IStaticTableConfiguration<Order> config)
        {
            config.SetAction("ListParentOrders", "Northwind")
                  .SetCssClass("table table-striped")
                  .ActionLinkColumn(c => c.OrderID, "", "#", o => new { o.OrderID }, new { @class = ChildOrderDetailTable .Filter})
                  .DisplayForColumn(c => c.Customer.ContactName)
                  .DisplayForColumn(c => c.Shipper.CompanyName)
                  .AddColumn(c => c.Total,
                             cfg => cfg.FormatUsing(d => d.ToString("c")).IsSortable(false).SetHeaderText("Order Total"))
                  .ConfigurePagingControl(p => p.SetContainerCssClass("pagination"));
        }
    }
}