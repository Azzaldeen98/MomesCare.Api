using MomesCare.Api.Entities.Base;

namespace MomesCare.Api.Entities.ViewModel.Comment
{
    public class CommentIndex : BaseComment
    {
        public int Id { get; set; }
        public int likes { get; set; }
        public bool userLiked { get; set; }
        public Author? author { get; set; }

    }



}
