using BusinessObjects.Models;
using BusinessObjects.ViewModel;

namespace Repositories.Interface
{
     public interface ISubmitAssignmentRespository
    {
        void SubmitAssignment(SubmitAssignmentViewModel submitAssignmentViewModel);
        IEnumerable<SubmitAssignment> ListSubmitAssignmentByAssId(int assId);
        SubmitAssignment GetSubmitAssignmentsById(int id);
    }
}
