using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("Customer", Schema = "SalesLT")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public bool NameStyle { get; set; }

        [MaxLength(8)]
        public string? Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(10)]
        public string? Suffix { get; set; }

        [MaxLength(128)]
        public string? CompanyName { get; set; }

        [MaxLength(256)]
        public string? SalesPerson { get; set; }

        [MaxLength(50)]
        public string? EmailAddress { get; set; }

        [MaxLength(25)]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(128)]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string PasswordSalt { get; set; } = null!;

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; } = new List<SalesOrderHeader>();

        public ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
    }
}
