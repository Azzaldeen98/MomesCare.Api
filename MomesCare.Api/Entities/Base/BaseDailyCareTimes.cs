using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.Base
{
    public class BaseDailyCareTimes
    {

        [Required]
        public TimeSpan time { get; set; }

        [Required]
        public string descript { get; set; }

        [Required]
        public bool state { get; set; } = true;
    }
}
