using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("Address", Schema = "SalesLT")]
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        [MaxLength(60)]
        public string AddressLine1 { get; set; } = null!;

        [MaxLength(60)]
        public string? AddressLine2 { get; set; }

        [Required]
        [MaxLength(30)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string StateProvince { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string CountryRegion { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string PostalCode { get; set; } = null!;

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
    }
}
