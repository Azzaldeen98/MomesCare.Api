using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel
{


    public class SignUp
    {
        [Required]
        public string uId { get; set; } 

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

    
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "User Name")]
        public string UserName => $"{FirstName} {LastName}";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [MaxLength(10)]
        [DataType(DataType.Text)]
        public string Role { get; set; } = UserRoles.User.ToString();
    }

}
