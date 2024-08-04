using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.BroadcastLive
{
    public class CreateBroadcastLive : BaseBroadcastLive
    {
        [Required]
        public string link { get; set; }
    }

}
