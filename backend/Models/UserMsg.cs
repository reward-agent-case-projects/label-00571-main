using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dqsm.Backend.Models
{
    /// <summary>
    /// User Message Table
    /// 接收、发送微信公众号用户的互动信息
    /// </summary>
    [Table("UserMsg")]
    public class UserMsg
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
