using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brawndo_Components.Models
{
    [Table("ProductModelProductDescription", Schema = "SalesLT")]
    [PrimaryKey(nameof(ProductModelId), nameof(ProductDescriptionId), nameof(Culture))]
    public class ProductModelProductDescription
    {
        public int ProductModelId { get; set; }

        public int ProductDescriptionId { get; set; }

        [MaxLength(6)]
        public string Culture { get; set; } = null!;

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ProductModelId")]
        public ProductModel ProductModel { get; set; } = null!;

        [ForeignKey("ProductDescriptionId")]
        public ProductDescription ProductDescription { get; set; } = null!;
    }
}
