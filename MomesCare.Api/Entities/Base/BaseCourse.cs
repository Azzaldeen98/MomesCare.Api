using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.Base
{

    public class BaseCourse
    {

        [Required]
        public string title { get; set; }

        [Required]
        public string descript { get; set; }

        [AllowNull]
        public string urlImage { get; set; }


        [Required]
        public string type { get; set; }

      
    }

}
