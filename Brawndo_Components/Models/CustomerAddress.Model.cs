using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brawndo_Components.Models
{
    [Table("CustomerAddress", Schema = "SalesLT")]
    [PrimaryKey(nameof(CustomerId), nameof(AddressId))]
    public class CustomerAddress
    {
        public int CustomerId { get; set; }

        public int AddressId { get; set; }

        [Required]
        [MaxLength(50)]
        public string AddressType { get; set; } = null!;

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!;

        [ForeignKey("AddressId")]
        public Address Address { get; set; } = null!;
    }
}
