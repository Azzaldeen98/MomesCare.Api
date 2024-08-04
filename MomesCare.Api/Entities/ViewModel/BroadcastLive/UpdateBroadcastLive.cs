using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.BroadcastLive
{
    public class UpdateBroadcastLive : BaseBroadcastLive
    {
        public int id { get; set; }

        [Required]
        public string link { get; set; }
    }

}
