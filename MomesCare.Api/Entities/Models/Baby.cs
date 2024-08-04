using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MomesCare.Api.Entities.Base;

namespace MomesCare.Api.Entities.Models
{
    public class Baby : BaseBaby
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public ApplicationUser user { get; set; }
        public List<BabyCareScheduled> scheduleds { get; set; }
    }


}
