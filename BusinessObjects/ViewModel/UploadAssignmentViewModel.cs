using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModel
{
        public class ReponseModel
        {
            public string? Message { get; set; }
            public bool? IsSuccess { get; set; }
            public bool? IsResponse { get; set; }
        }

        public class UploadAssignmentViewModel : ReponseModel
        {
            public IFormFile Assignment { get; set; }
            public int? AssignmentId { get; set; }
            public string? AssignmentName { get; set; }
            public string? Path { get; set; }
            public int? CourseId { get; set; }
            public int? UploaderId { get; set; }
            public DateTime? RequiredDate { get; set; }
        }
    }
