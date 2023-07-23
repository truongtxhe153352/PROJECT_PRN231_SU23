using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class MaterialDto
    {
        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string? Path { get; set; }
        public int? CourseId { get; set; }
        public int? UploaderId { get; set; }
        public string UploaderName { get; set; }
        public string CourseName { get; set; }
    }
}
