using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Material
    {
        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string? Path { get; set; }
        public int? CourseId { get; set; }
        public int? UploaderId { get; set; }

        public virtual Course? Course { get; set; }
        public virtual User? Uploader { get; set; }
    }
}
