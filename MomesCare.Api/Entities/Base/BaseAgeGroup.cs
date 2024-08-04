using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.Base
{
    public class BaseAgeGroup
    {


        [Required]
        public int min { get; set; }
        [Required]
        public int max { get; set; }

        [Required]
        public TimePeriodScale timePeriodScale { get; set; } = TimePeriodScale.Month;
    }

}
