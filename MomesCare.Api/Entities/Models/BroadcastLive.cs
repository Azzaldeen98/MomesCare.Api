using MomesCare.Api.Entities.Base;
using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{
   
    public class BroadcastLive : BaseBroadcastLive
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int id { get; set; }

        public DateTime createdAt { get; set; }

        public List<JoinBroadcastLive> joinsBroadcastLives { get; set; }

        public ApplicationUser user { get; set; }

    }


}
