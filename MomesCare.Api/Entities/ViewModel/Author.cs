using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Entities.ViewModel
{
    public class Author
    {

        public string id { get; set; }
        public string name { get; set; }
        public string image { get; set; }



        public static Author? fromUser(ApplicationUser  user)
        {
            return user==null ? null : new Author {

                id = user.Id,
                name = user.FullName,
                image = user.profile != null ? user.profile.Image : ""

            };
        }
    }
}
