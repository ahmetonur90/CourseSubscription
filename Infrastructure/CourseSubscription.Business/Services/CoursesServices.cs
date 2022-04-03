using CourseSubscription.Business.Abstract;
using CourseSubscription.Data.Abstract;
using CourseSubscription.Entity.Model;

namespace CourseSubscription.Business.Services
{
    public class CoursesServices : ICoursesServices
    {
        private readonly ICoursesRepository _coursesRepository;

        public CoursesServices(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public COURSE GetCourseById(decimal cou_auto_key)
        {
            return _coursesRepository.Get(x => x.COU_AUTO_KEY == cou_auto_key);
        }
    }
}
