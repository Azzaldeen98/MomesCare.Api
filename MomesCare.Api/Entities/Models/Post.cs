using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MomesCare.Api.Entities.Base;

namespace MomesCare.Api.Entities.Models
{
    public class Post : BasePost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }
        public List<Comment> comments { get; set; }
        public List<PostLike> likes { get; set; }
        public ApplicationUser user { get; set; }
    } 
    
    


    //public class ResponsiblePhysician
    //{

    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Key]
    //    [Required]
    //    public int Id { get; set; }
    //    public ApplicationUser user { get; set; }
    //}


}
