using Microsoft.AspNetCore.Mvc.Formatters;
using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.ViewModel.Courses;
using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.Models
{
    public class CourseItem: BaseCourseItem
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        public Course course { get; set; }


    }
    



}
