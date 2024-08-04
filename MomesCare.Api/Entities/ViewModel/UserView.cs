using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel
{
    public class UserView
    {
        public string id { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public string role { get; set; }= UserRoles.User.ToString();
        public string urlImage { get; set; }
    }
}
