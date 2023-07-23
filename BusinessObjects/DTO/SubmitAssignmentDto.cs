using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class SubmitAssignmentDto
    {
        public int? SubmitAssignmentId { get; set; }
        public string? SubmitAssignmentName { get; set; }
        public int? UploaderId { get; set; }
        public string? StudentName { get; set; }
        public int? AssignmentId { get; set; }
        public string? AssignmentName { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
    }
}
