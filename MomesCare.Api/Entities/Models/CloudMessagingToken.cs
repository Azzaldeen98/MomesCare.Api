
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{
    public class CloudMessagingToken
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public string  id{ get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string  Token{ get; set; }
        
        public ApplicationUser user { get; set; }
    }
}
