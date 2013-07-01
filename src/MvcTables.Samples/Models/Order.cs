namespace MvcTables.Samples.NorthwindEntities
{
    #region

    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    #endregion

    public partial class Order
    {
        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal Total
        {
            get { return Order_Details.Sum(ol => ol.LineTotal); }
        }
    }
}