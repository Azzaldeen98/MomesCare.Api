using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Profile
{
    public class ChangePassword
    {
        [Required]
        public string currentPassword { get; set; }
        [Required]
        public string newPassword { get; set; }


    }


   






}
