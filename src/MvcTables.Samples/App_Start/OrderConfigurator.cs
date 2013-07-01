namespace MvcTables.Samples.App_Start
{
    #region

    using System.Linq;
    using Configuration;
    using NorthwindEntities;

    #endregion

    public class OrderConfigurator : MvcTable<Order>
    {
        public override void Configure(IStaticTableConfiguration<Order> config)
        {
            config.SetCssClass("table table-striped")
                  .HiddenColumnFor(c => c.OrderID, cfg => cfg.Hide())
                  .ActionLinkColumn(c => c.Customer.City, "Category", "Home")
                  .DisplayForColumn(c => c.Shipper.CompanyName)
                  .DisplayForColumn(c => c.ShipRegion)
                  .DisplayForColumn(c => c.Order_Details[0].Quantity)
                  .ConfigurePagingControl(p => p.SetContainerCssClass("pagination"));
        }
    }

    public class OrderFilterConfigurator : MvcTable<Order>
    {
        public override void Configure(IStaticTableConfiguration<Order> config)
        {
            config
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

    public class ParentOrderConfigurator : MvcTable<Order>
    {
        public override void Configure(IStaticTableConfiguration<Order> config)
        {
            config
                .SetCssClass("table table-striped")
                .ActionLinkColumn(c => c.OrderID, "", "#", o => new {o.OrderID}, new {@class = "childFilter"})
                .DisplayForColumn(c => c.Customer.ContactName)
                .DisplayForColumn(c => c.Shipper.CompanyName)
                .AddColumn(c => c.Total,
                           cfg => cfg.FormatUsing(d => d.ToString("c")).IsSortable(false).SetHeaderText("Order Total"))
                .ConfigurePagingControl(p => p.SetContainerCssClass("pagination"));
        }
    }

    public class ChildOrderDetailConfigurator : MvcTable<Order_Detail>
    {
        public override void Configure(IStaticTableConfiguration<Order_Detail> config)
        {
            config
                .SetCssClass("table table-striped")
                .DisplayForColumn(c => c.Product.ProductName,
                                  cfg =>
                                  cfg.DefineFooter(
                                                   (h, rows) =>
                                                   rows.Any() ? "Order ID: " + h.ViewData.Model.First().OrderID : ""))
                .DisplayForColumn(c => c.Quantity)
                .DisplayForColumn(c => c.UnitPrice)
                .DisplayForColumn(c => c.LineTotal,
                                  cfg =>
                                  cfg.DefineFooter(
                                                   (h, rows) =>
                                                   rows.Any() ? rows.Sum(od => od.LineTotal).ToString("c") : ""));
        }
    }
}