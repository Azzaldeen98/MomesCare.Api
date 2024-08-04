using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.Base
{
    public class BaseBroadcastLive
    {

        [Required]
        public string link { get; set; }

        [Required]
        public string descript { get; set; }

        [Required]
        public BroadcastLiveStatus status { get; set; }

        [Required]
        public DateTime startDateTime { get; set; }


    }
}
