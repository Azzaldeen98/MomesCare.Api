using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Entities.ViewModel.Courses
{
    public class CourseIndex : BaseCourse
    {
        public int id { get; set; }
     
        public List<CourseItemIndex> courseItemsIndex { get; set; }
    }

}
