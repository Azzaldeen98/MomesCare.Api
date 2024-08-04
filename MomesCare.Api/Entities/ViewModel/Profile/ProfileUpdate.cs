using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Profile
{
    public class ProfileUpdate
    {

        [Required]
        public int id { get; set; }
        [Required]
        public string Image { get; set; }


    }






}
