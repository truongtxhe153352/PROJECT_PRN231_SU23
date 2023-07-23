using BusinessObjects.DTO;
using BusinessObjects.Models;
using BusinessObjects.ViewModel;
using DataAccess;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CourseRepository : ICourseRepository
    {
        public void DeleteCourse(Course course) => CourseDao.DeleteCourse(course);

        public List<Course> GetAllCourse() => CourseDao.GetAllCourse();

        public IEnumerable<Course> GetAllCourseByStudentId(int studentId) => CourseDao.GetAllCourseByStudentId(studentId);

        public IEnumerable<Course> GetAllCourseByTeacherId(int teacherId) => CourseDao.GetAllCourseByTeacherId(teacherId);

        public Course GetCourseById(int courseId) => CourseDao.GetCourseById(courseId);

        public void InsertCourse(Course course) => CourseDao.SaveCourse(course);

        public void UpdateCourse(Course course) => CourseDao.UpdateCourse(course);
    }
}
