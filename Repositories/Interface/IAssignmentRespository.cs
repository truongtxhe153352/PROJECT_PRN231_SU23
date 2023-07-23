using BusinessObjects.Models;
using BusinessObjects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IAssignmentRespository
    {
        void SaveAssignment(UploadAssignmentViewModel uploadAssignmentViewModel);
        IEnumerable<Assignment> GetAssignmentsByCourseId(int courseId);
        IEnumerable<Assignment> ListAssignmentByTeacherAndCourse(int teacherId, int courseId);
        Assignment GetAssignmentsByAssId(int assId);
    }
}
