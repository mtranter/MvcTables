using System.ComponentModel.DataAnnotations;

namespace MvcTables.Samples.NorthwindEntities
{

    [MetadataType(typeof(Employee_Metadata))]
    public partial class Employee
    {
        
    }

    public class Employee_Metadata
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"\(\d{2,3}\)\s\d{3}-\d{4}")]
        public string HomePhone { get; set; }
    }
}