using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModel
{
    public class UploadMaterialViewModel 
    {
        public IFormFile Material { get; set; }
        public string MaterialPath { get; set; }
        public int CourseId { get; set; }
        public int UploaderId { get; set; }
        public string MaterialName { get; set; }
    }
}
