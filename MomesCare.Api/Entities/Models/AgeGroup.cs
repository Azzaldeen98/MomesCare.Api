using MomesCare.Api.Entities.Base;
using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{
    public class AgeGroup : BaseAgeGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int id { get; set; }

        public List<DailyCareTimes> dailyCareTimes { get; set; }
    }


}
