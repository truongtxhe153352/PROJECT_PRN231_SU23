using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class AssigmentDto
    {
        public int AssignmentId { get; set; }
        public string? AssignmentName { get; set; }
        public string? Path { get; set; }
        public int? CourseId { get; set; }
        public int? UploaderId { get; set; }
        public String? TeacherName { get; set; }
        public DateTime? RequiredDate { get; set; }
    }
}
