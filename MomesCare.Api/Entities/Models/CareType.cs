using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MomesCare.Api.Entities.Base;

namespace MomesCare.Api.Entities.Models
{

    public class CareType : BaseCareType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int id { get; set; }

        public List<DailyCareTimes> dailyCareTimes { get; set; }

    }


}
