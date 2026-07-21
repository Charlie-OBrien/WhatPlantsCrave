using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("ProductDescription", Schema = "SalesLT")]
    public class ProductDescription
    {
        [Key]
        public int ProductDescriptionId { get; set; }

        [Required]
        [MaxLength(400)]
        public string Description { get; set; } = null!;

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public ICollection<ProductModelProductDescription> ProductModelProductDescriptions { get; set; } = new List<ProductModelProductDescription>();
    }
}
