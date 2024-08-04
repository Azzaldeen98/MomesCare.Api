using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.Models
{
    public class Doctor
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }
            

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string Specialization { get; set; }

        [AllowNull]
        public string Descript  { get; set; }

        public bool Status { get; set; } = true;

        public ApplicationUser user { get; set; }

    }


}
