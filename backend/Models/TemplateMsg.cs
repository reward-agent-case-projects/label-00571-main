using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dqsm.Backend.Models
{
    /// <summary>
    /// Template Message Table
    /// 发送模板信息记录
    /// </summary>
    [Table("TemplateMsg")]
    public class TemplateMsg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FromUser { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ToUser { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Message { get; set; } = string.Empty;

        public DateTime DateTime { get; set; }
    }
}
