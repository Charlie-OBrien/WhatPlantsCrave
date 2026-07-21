using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("ErrorLog", Schema = "dbo")]
    public class ErrorLog
    {
        [Key]
        public int ErrorLogId { get; set; }

        [Required]
        public DateTime ErrorTime { get; set; }

        [Required]
        [MaxLength(128)]
        public string UserName { get; set; } = null!;

        [Required]
        public int ErrorNumber { get; set; }

        public int? ErrorSeverity { get; set; }

        public int? ErrorState { get; set; }

        [MaxLength(126)]
        public string? ErrorProcedure { get; set; }

        public int? ErrorLine { get; set; }

        [Required]
        [MaxLength(4000)]
        public string ErrorMessage { get; set; } = null!;
    }
}
