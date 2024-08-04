using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{
    public class BabyHealthCareNotificationsSent 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public int babyId { get; set; }
        [Required]
        public int dailyCareTimeId { get; set; }
        [Required]
        public DateTime createdAt { get; set; }
    }




}
