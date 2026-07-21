using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("ProductCategory", Schema = "SalesLT")]
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }

        public int? ParentProductCategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ParentProductCategoryId")]
        public ProductCategory? ParentProductCategory { get; set; }

        public ICollection<ProductCategory> ChildProductCategories { get; set; } = new List<ProductCategory>();

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
