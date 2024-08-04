using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.DailyCareTimes
{
    public class DailyCareTimesUpdate : BaseDailyCareTimes
    {


        [Required]
        public int id { get; set; }

        [Required]
        public string descript { get; set; }

        [Required]
        public bool state { get; set; } = true;


        [Required]
        public string time { get; set; }


        [Required]
        public int careTypeId { get; set; }

        [Required]
        public int ageGroupId { get; set; }

     
    }
}
