using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDao
    {
        public static List<User> getAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    users = context.Users.Include(x => x.Role).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }
        public static User FindUserById(int uId)
        {
            User u = new User();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    u = context.Users
                        .Include(x => x.Role)
                        .SingleOrDefault(x => x.UserId == uId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return u;
        }
        public static void InsertUser(User u)
        {
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    context.Users.Add(u);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateUser(User u)
        {
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    context.Entry<User>(u).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //TOFIX
        public static void DeleteUser(User u)
        {
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    //Todo: delete materials --> submitAssignment --> Assignments
                    var p1 = context.Users.SingleOrDefault(c => c.UserId == u.UserId);
                    var materials = context.Materials.Where(c => c.Uploader.UserId == p1.UserId).ToList();
                    var assignments = context.Assignments.Where(c => c.Uploader.UserId == p1.UserId).ToList();
                    //1. delete materials
                    context.Materials.RemoveRange(materials);
                    //2. delete submitAssignments of Ass
                    foreach (var ass in assignments)
                    {
                        //delete assignments file
                        if (File.Exists(ass.Path))
                        {
                            File.Delete(ass.Path);
                        }
                        var subAss = context.SubmitAssignments.Where(a => a.AssignmentId == ass.AssignmentId).ToList();
                        foreach (var sub in subAss)
                        {
                            //delete SubAssignments file
                            if (File.Exists(ass.Path))
                            {
                                File.Delete(ass.Path);
                            }
                        }
                        context.SubmitAssignments.RemoveRange(subAss);
                    }

                    //3. delete ass
                    context.Assignments.RemoveRange(assignments);
                    //4. delete user course
                    var c = context.Users.Include(c => c.Courses)
                        .SingleOrDefault(c => c.UserId == u.UserId);

                    var userCourse = c.Courses.ToList();
                    foreach (var uc in userCourse)
                    {
                        c.Courses.Remove(uc);
                    }
                    //5. delete submitass of user
                    var subAssignments = context.SubmitAssignments.Where(c => c.Uploader.UserId == p1.UserId).ToList();
                    context.SubmitAssignments.RemoveRange(subAssignments);
                    //delete user
                    context.Users.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static User checkLogin(string email, string password)
        {
            User user = new User();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    user = context.Users.Where(u => u.Password.Equals(password) && u.Email.Equals(email)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public static string GetRoleByEmail(string email)
        {
            string role = "";
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    role = context.Users.Include(u => u.Role).Where(u => u.Email.Equals(email)).Select(u => u.Role.RoleName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return role;
        }

        public static User GetUserByEmail(string email)
        {
            User user = new User();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    user = context.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
    }
}
