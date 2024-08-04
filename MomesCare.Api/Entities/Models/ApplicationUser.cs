using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MomesCare.Api.Entities.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
            

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        public Profile profile { get; set; }
        public Doctor doctor { get; set; }
        public CloudMessagingToken cloudMessagingToken { get; set; }
        public List<Post> posts { get; set; }
        public List<Comment> comments { get; set; }
        public List<PostLike> postLikes { get; set; }
        public List<CommentLike> commentLikes { get; set; }
        public List<FavoritePosts> favoritePosts { get; set; }
        public List<FavoriteComments> favoriteComments { get; set; }
        public List<Baby> babies { get; set; }
        public List<BroadcastLive> broadcastLives { get; set; }
        public List<JoinBroadcastLive> joinsBroadcastLives { get; set; }
        public List<Notification> notifications { get; set; }


    }
}
