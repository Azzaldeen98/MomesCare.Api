using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel
{
    

    public class UserInfo
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; } 

        public string Token { get; set; } = "";


    }


    

}
