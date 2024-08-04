using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.ViewModel.Profile
{
    public class UpdateImage
    {

        [AllowNull]
        public int id { get; set; }

        [Required]
        public string UrlImage { get; set; }


    }






}
