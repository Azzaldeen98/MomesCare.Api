using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.ViewModel
{
    public class SignIn
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [AllowNull]
        public string FCMToken { get; set; }
    }

}
