using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.DailyCareTimes
{
    public class DailyCareTimesCreate : BaseDailyCareTimes
    {

        [Required]
        public string descript { get; set; }

        [Required]
        public bool state { get; set; } = true;

        [Required]
        public  string time { get; set; }

        [Required]
        public int careTypeId { get; set; }

        [Required]
        public int ageGroupId { get; set; }
    }
}
