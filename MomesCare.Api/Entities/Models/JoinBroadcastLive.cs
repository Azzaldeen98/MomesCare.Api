using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomesCare.Api.Entities.Models
{


  
    public class JoinBroadcastLive: BaseJoinBroadcastLive
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime createdAt { get; set; }

        public BroadcastLive broadcastLive { get; set; }

        public ApplicationUser user { get; set; }

    }


}
