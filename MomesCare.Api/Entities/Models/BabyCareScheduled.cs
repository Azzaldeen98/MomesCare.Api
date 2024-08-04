using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{
    public class BabyCareScheduled
    {
      
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string descript { get; set; }

        [Required]
        public DateTime appointmentAt { get; set; }

        [Required]
        public DateTime createdAt { get; set; }
       
        [Required]
        public bool notification { get; set; }

        public Baby baby { get; set; }

    }
    


}
