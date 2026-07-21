using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("SalesOrderHeader", Schema = "SalesLT")]
    public class SalesOrderHeader
    {
        [Key]
        public int SalesOrderId { get; set; }

        [Required]
        public byte RevisionNumber { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ShipDate { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public bool OnlineOrderFlag { get; set; }

        [MaxLength(23)]
        public string SalesOrderNumber { get; set; } = null!;

        [MaxLength(25)]
        public string? PurchaseOrderNumber { get; set; }

        [MaxLength(15)]
        public string? AccountNumber { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public int? ShipToAddressId { get; set; }

        public int? BillToAddressId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShipMethod { get; set; } = null!;

        [MaxLength(15)]
        public string? CreditCardApprovalCode { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal TaxAmt { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Freight { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalDue { get; set; }

        public string? Comment { get; set; }

        [Required]
        public Guid Rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!;

        public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();
    }
}
