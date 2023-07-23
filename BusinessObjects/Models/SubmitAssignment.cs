using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class SubmitAssignment
    {
        public int SubmitAssignmentId { get; set; }
        public string? SubmitAssignmentName { get; set; }
        public int? UploaderId { get; set; }
        public int? AssignmentId { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }

        public virtual Assignment? Assignment { get; set; }
        public virtual User? Uploader { get; set; }
    }
}
