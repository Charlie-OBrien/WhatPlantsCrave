using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("Product", Schema = "SalesLT")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(25)]
        public string ProductNumber { get; set; } = null!;

        [MaxLength(15)]
        public string? Color { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal StandardCost { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal ListPrice { get; set; }

        [MaxLength(5)]
        public string? Size { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Weight { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductModelId { get; set; }

        [Required]
        public DateTime SellStartDate { get; set; }

        public DateTime? SellEndDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }

        public byte[]? ThumbNailPhoto { get; set; }

        [MaxLength(50)]
        public string? ThumbnailPhotoFileName { get; set; }

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory? ProductCategory { get; set; }

        [ForeignKey("ProductModelId")]
        public ProductModel? ProductModel { get; set; }

        public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();
    }
}
