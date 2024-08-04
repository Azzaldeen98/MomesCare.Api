using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{
    public class FavoritePosts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]

        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public Post post { get; set; }
        public ApplicationUser user { get; set; }
    }
}
