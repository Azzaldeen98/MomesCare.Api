using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MomesCare.Api.Entities.Base;

namespace MomesCare.Api.Entities.Models
{
    public class Comment : BaseComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        public Post post { get; set; }
        public List<CommentLike> likes { get; set; }
        public ApplicationUser user { get; set; }


        public Comment Clone() => new Comment
        {
            Contant = this.Contant,
            Id = this.Id,
            user = this.user,
            likes = this.likes,
            CreatedAt = this.CreatedAt,

        };

        
    }
}
