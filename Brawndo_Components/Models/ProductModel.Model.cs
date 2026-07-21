using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("ProductModel", Schema = "SalesLT")]
    public class ProductModel
    {
        [Key]
        public int ProductModelId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public string? CatalogDescription { get; set; }

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<ProductModelProductDescription> ProductModelProductDescriptions { get; set; } = new List<ProductModelProductDescription>();
    }
}
