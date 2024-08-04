using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.Base
{
    public class BaseCareType
    {

        [Required]
        public string name { get; set; }

        [Required]
        public bool state { get; set; } = true;
    }

}
