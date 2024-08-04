using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{


    public class DailyCareTimes : BaseDailyCareTimes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int id { get; set; }

        public CareType careType { get; set; }

        public AgeGroup ageGroup { get; set; } 

    }




}
