namespace MvcTables.Samples.App_Start
{
    #region

    using System.Linq;
    using Configuration;
    using NorthwindEntities;

    #endregion

    public class ChildOrderDetailTable : MvcTable<Order_Detail>
    {

        internal const string Filter = "child-table-filter";

        public override void Configure(IStaticTableConfiguration<Order_Detail> config)
        {
            config.SetAction("ListChildOrderDetails", "Northwind")
                .SetCssClass("table table-striped")
                .SetFilterClass(Filter)
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