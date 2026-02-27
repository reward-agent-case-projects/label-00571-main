using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dqsm.Backend.Models
{
    /// <summary>
    /// Logs Table
    /// 静态日志服务记录表
    /// </summary>
    [Table("Logs")]
    public class Logs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Action { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string InfoType { get; set; } = string.Empty; // info, debug, error

        [Column(TypeName = "TEXT")]
        public string Info { get; set; } = string.Empty;

        public DateTime DateTime { get; set; }
    }
}
