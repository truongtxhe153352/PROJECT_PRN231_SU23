using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CourseDao
    {
        public static List<Course> GetAllCourse()
        {
            List<Course> list = new List<Course>();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    var courses = context.Courses.Include(c => c.Assignments).Include(u => u.Users).ToList();
                    foreach (var c in courses)
                    {
                        list.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return list;
        }

        public static Course GetCourseById(int id)
        {
            Course c = new Course();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    c = context.Courses.Include(c => c.Assignments).Include(u => u.Users)
                        .Include(c => c.Materials).Where(c => c.CourseId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return c;
        }

        public static void SaveCourse(Course c)
        {
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    context.Courses.Add(c);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateCourse(Course c)
        {
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    context.Entry<Course>(c).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteCourse(Course course)
        {
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var c = context.Courses.Include(c => c.Users).Include(c => c.Assignments)
                        .Include(c => c.Materials).SingleOrDefault(c => c.CourseId == course.CourseId);

                        var userCourse = c.Users.ToList();
                        foreach (var uc in userCourse)
                        {
                            c.Users.Remove(uc);
                        }
                        var assignmentCourse = context.Assignments.Where(a => a.CourseId == course.CourseId).ToList();
                        foreach (var ac in assignmentCourse)
                        {
                            context.Assignments.Remove(ac);
                        }
                        var materialCourse = c.Materials.Where(a => a.CourseId == course.CourseId).ToList();
                        foreach (var mc in materialCourse)
                        {
                            context.Materials.Remove(mc);
                        }
                        context.Courses.Remove(c);

                        if (context.SaveChanges() > 0)
                        {

                            transaction.Commit();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static IEnumerable<Course> GetAllCourseByTeacherId(int teacherId)
        {
            List<Course> list = new List<Course>();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    var user = context.Users.Where(u => u.UserId == teacherId).FirstOrDefault();
                    var courses = context.Courses.Include(c => c.Assignments).Include(u => u.Users).Where(u => u.Users.Contains(user)).ToList();
                    foreach (var c in courses)
                    {
                        list.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return list;
        }

        public static IEnumerable<Course> GetAllCourseByStudentId(int studentId)
        {
            List<Course> list = new List<Course>();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    var user = context.Users.Where(u => u.UserId == studentId).FirstOrDefault();
                    var courses = context.Courses.Include(c => c.Assignments).Include(u => u.Users).Where(u => u.Users.Contains(user)).ToList();
                    foreach (var c in courses)
                    {
                        list.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return list;
        }
    }
}
