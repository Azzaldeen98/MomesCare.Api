using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{
    public class Notification
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public string id { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string body { get; set; }

        [Required]
        public bool status { get; set; } = true;     
    
        [Required]
        public NotificationType type { get; set; }

        [Required]
        public DateTime createdAt { get; set; }

        public ApplicationUser user { get; set; }
    }
}
