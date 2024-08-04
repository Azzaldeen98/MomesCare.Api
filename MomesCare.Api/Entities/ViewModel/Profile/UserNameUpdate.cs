using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Profile
{
    public class UserNameUpdate
    {
        [Required]
        public string name { get; set; }

    }
}
