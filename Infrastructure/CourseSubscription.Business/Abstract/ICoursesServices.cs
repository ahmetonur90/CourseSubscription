using CourseSubscription.Entity.Model;

namespace CourseSubscription.Business.Abstract
{
    public interface ICoursesServices
    {
        COURSE GetCourseById(decimal cou_auto_key);

    }
}
