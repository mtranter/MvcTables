namespace MvcTables.Samples.App_Start
{
    #region

    using Configuration;
    using NorthwindEntities;

    #endregion

    public class InvoiceTable : MvcTable<Invoice>
    {
        public override void Configure(IStaticTableConfiguration<Invoice> config)
        {
            config
                .SetCssClass("table table-striped")
                .DisplayForColumn(m => m.ProductName)
                .DisplayForColumn(m => m.Quantity)
                .DisplayForColumn(m => m.Region)
                .DisplayForColumn(m => m.Salesperson)
                .EditorForColumn(m => m.Discount);
        }
    }
}