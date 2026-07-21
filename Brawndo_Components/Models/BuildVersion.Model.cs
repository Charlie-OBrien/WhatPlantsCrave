using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brawndo_Components.Models
{
    [Table("BuildVersion", Schema = "dbo")]
    public class BuildVersion
    {
        [Key]
        public byte SystemInformationId { get; set; }

        [Required]
        [MaxLength(25)]
        [Column("Database Version")]
        public string DatabaseVersion { get; set; } = null!;

        [Required]
        public DateTime VersionDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
