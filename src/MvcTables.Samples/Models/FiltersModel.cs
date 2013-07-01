namespace MvcTables.Samples.Models
{
    #region

    using System.Collections.Generic;
    using NorthwindEntities;

    #endregion

    public class FiltersModel
    {
        public string SelectedCustomerId { get; set; }

        public IEnumerable<Customer> Customers { get; set; }
    }
}