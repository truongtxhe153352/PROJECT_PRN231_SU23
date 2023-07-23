using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Assignment
    {
        public Assignment()
        {
            SubmitAssignments = new HashSet<SubmitAssignment>();
        }

        public int AssignmentId { get; set; }
        public string? AssignmentName { get; set; }
        public string? Path { get; set; }
        public int? CourseId { get; set; }
        public int? UploaderId { get; set; }
        public DateTime? RequiredDate { get; set; }

        public virtual Course? Course { get; set; }
        public virtual User? Uploader { get; set; }
        public virtual ICollection<SubmitAssignment> SubmitAssignments { get; set; }
    }
}
