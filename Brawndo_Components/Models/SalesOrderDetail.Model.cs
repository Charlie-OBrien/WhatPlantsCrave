using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brawndo_Components.Models
{
    [Table("SalesOrderDetail", Schema = "SalesLT")]
    [PrimaryKey(nameof(SalesOrderId), nameof(SalesOrderDetailId))]
    public class SalesOrderDetail
    {
        public int SalesOrderId { get; set; }

        public int SalesOrderDetailId { get; set; }

        [Required]
        public short OrderQty { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal UnitPriceDiscount { get; set; }

        [Column(TypeName = "money")]
        public decimal LineTotal { get; set; }

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("SalesOrderId")]
        public SalesOrderHeader SalesOrderHeader { get; set; } = null!;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }
}
