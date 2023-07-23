using BusinessObjects.Models;
using BusinessObjects.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AssignmentDao
    {
        public static void SaveAssignment(UploadAssignmentViewModel model)
        {
            String path = addAssFileToAPILocal(model);
            model.Path = path;
            //add record to db

            //data access
            using (var context = new PRN231_ProjectContext())
            {
                try
                {
                    Assignment ass = new Assignment
                    {
                        CourseId = model.CourseId,
                        UploaderId = model.UploaderId,
                        Path = model.Path,
                        AssignmentName = model.AssignmentName,
                        RequiredDate = model.RequiredDate,
                    };
                    context.Assignments.Add(ass);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private static String addAssFileToAPILocal(UploadAssignmentViewModel model)
        {
            model.IsResponse = true;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/AllFiles/Assigments");
            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //get file extension
            //FileInfo fileInfo = new FileInfo(model.Assignment.FileName);
            //set file name TeacherID_CourseID_FileName
            string fileName = model.UploaderId + "_" + model.CourseId + "_" + model.Assignment.FileName;
            //model.FileName + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.Assignment.CopyTo(stream);
            }
            model.IsSuccess = true;
            model.Message = "File upload successfully";
            return fileNameWithPath;
        }
        public static IEnumerable<Assignment> GetAssignmentsByCourseId(int courseId)
        {
            List<Assignment> list = new List<Assignment>();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    list = context.Assignments.Include(m => m.Course).Include(m => m.Uploader).Where(a => a.CourseId == courseId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        public static IEnumerable<Assignment> ListAssignmentByTeacherAndCourse(int teacherId, int courseId)
        {
            List<Assignment> list = new List<Assignment>();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    list = context.Assignments.Include(m => m.Course).Include(m => m.Uploader).Where(a => a.UploaderId == teacherId && a.Course.CourseId == courseId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        public static Assignment GetAssignmentsByAssId(int assId)
        {
            Assignment assignment = new Assignment();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    assignment = context.Assignments.Include(m => m.Course).Include(m => m.Uploader).Where(a => a.AssignmentId == assId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return assignment;
        }
    }
}
