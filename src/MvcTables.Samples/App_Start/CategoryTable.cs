namespace MvcTables.Samples.App_Start
{
    #region

    using Configuration;
    using NorthwindEntities;

    #endregion

    public class CategoryTable : MvcTable<Category>
    {
        public override void Configure(IStaticTableConfiguration<Category> config)
        {
            config
                .ScaffoldAllColumns()
                .SetAction("ListCategories", "Northwind")
                .SetCssClass("table table-striped");
        }
    }


}