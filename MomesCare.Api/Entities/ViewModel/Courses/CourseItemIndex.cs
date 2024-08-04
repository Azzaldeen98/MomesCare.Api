using MomesCare.Api.Entities.Base;

namespace MomesCare.Api.Entities.ViewModel.Courses
{
    public class CourseItemIndex : BaseCourseItem
    {
        public int Id { get; set; }
        public CourseIndex courseIndex { get; set; }
    }

}
