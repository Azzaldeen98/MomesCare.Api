using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{
    public class Profile
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }
            

        [ForeignKey("User")]
        public string UserId { get; set; }


        [Required]
        public string Image { get; set; }

        public ApplicationUser user { get; set; }

    } 




}
