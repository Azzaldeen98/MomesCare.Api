using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.ViewModel.Comment;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Post
{
    public class PostIndex : BasePost
    {
        public int Id { get; set; }

        public int likes { get; set; }

        public bool userLiked { get; set; } = false;

        public Author author { get; set; }

        [Display(Name = "commentsCount")]
        public int commentsCount => (comments != null) ? comments.Count : 0;
        public List<CommentIndex> comments { get; set; }

    }

 

}
