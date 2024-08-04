using Firebase.Auth;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.ViewModel
{

    public class FCMToken
    {
        [AllowNull]
        public string userId { get; set; }

        [Required]
        public string Token { get; set; }
    }



}
