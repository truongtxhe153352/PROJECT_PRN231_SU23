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
    public class SubmitAssignmentDao
    {
        public static void SubmitAssignment(SubmitAssignmentViewModel model)
        {
            String path = addAssFileToAPILocal(model);
            model.Path = path;
            //add record to db

            //data access
            using (var context = new PRN231_ProjectContext())
            {
                try
                {
                    SubmitAssignment sbAss = new SubmitAssignment
                    {
                        SubmitAssignmentName = model.SubmitAssignmentName,
                        UploaderId = model.UploaderId,
                        Path = model.Path,
                        AssignmentId = model.AssignmentId,
                        Description = model.Description,
                    };
                    context.SubmitAssignments.Add(sbAss);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static IEnumerable<SubmitAssignment> ListSubmitAssignmentByAssId(int assId)
        {
            List<SubmitAssignment> list = new List<SubmitAssignment>();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    list = context.SubmitAssignments.Include(m => m.Uploader).Include(m => m.Assignment).Where(a => a.Assignment.AssignmentId == assId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        private static String addAssFileToAPILocal(SubmitAssignmentViewModel model)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory()
                , "wwwroot/AllFiles/SubmitAssignment");
            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //get file extension
            FileInfo fileInfo = new FileInfo(model.SubmitFile.FileName);
            //set file name TeacherID_CourseID_FileName
            string fileName = model.UploaderId + "_" + model.AssignmentId + "_" + model.SubmitFile.FileName;
            //model.FileName + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.SubmitFile.CopyTo(stream);
            }
            return fileNameWithPath;
        }

        public static SubmitAssignment GetSubmitAssignmentsById(int id)
        {
            SubmitAssignment subAss = new SubmitAssignment();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    subAss = context.SubmitAssignments.Include(m => m.Uploader).Include(m => m.Assignment).Where(a => a.SubmitAssignmentId == id).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return subAss;
        }

    }
}
