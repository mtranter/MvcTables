namespace MvcTables.Samples.NorthwindEntities
{
    #region

    using System.ComponentModel.DataAnnotations;

    #endregion

    public partial class Order_Detail
    {
        [Display(Name = "Line Total")]
        [DataType(DataType.Currency)]
        public decimal LineTotal
        {
            get { return Quantity*UnitPrice; }
        }
    }
}