using BusinessObjects.DTO;
using BusinessObjects.Models;
using BusinessObjects.ViewModel;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AssignmentRepository
    {
        public void SaveAssignment(UploadAssignmentViewModel model) => AssignmentDao.SaveAssignment(model);
        public IEnumerable<Assignment> GetAssignmentsByCourseId(int courseId) => AssignmentDao.GetAssignmentsByCourseId(courseId);

        public Assignment GetAssignmentsByAssId(int assId)
        => AssignmentDao.GetAssignmentsByAssId(assId);

        public IEnumerable<Assignment> ListAssignmentByTeacherAndCourse(int teacherId, int courseId)
         => AssignmentDao.ListAssignmentByTeacherAndCourse(teacherId, courseId);

    }
}
