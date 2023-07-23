

using BusinessObjects.Models;

namespace Repositories.Interface
{
    public interface ICourseRepository
    {
        public IEnumerable<Course> GetAllCourseByTeacherId(int teacherId);
        public IEnumerable<Course> GetAllCourseByStudentId(int studentId);

        List<Course> GetAllCourse();
        Course GetCourseById(int courseId);
        void InsertCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
    }
}
