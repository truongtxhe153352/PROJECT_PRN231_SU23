using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModel
{
    public class SubmitAssignmentViewModel 
    {
        public IFormFile SubmitFile { get; set; }
        public int? SubmitAssignmentId { get; set; }
        public string? SubmitAssignmentName { get; set; }
        public int? UploaderId { get; set; }
        public int? AssignmentId { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
    }
}
