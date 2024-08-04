using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.Models
{
    public class Course: BaseCourse
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime createdAt { get; set; }

        public List<CourseItem> courseItems { get; set; }


    }




}
