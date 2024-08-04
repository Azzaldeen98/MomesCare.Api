using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Courses
{
    public class CreateCourseItem : BaseCourseItem
    {

        [Required]
        public int courseId { get; set; }


    }

}
