using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Course
    {
        public Course()
        {
            Assignments = new HashSet<Assignment>();
            Materials = new HashSet<Material>();
            Users = new HashSet<User>();
        }

        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Material> Materials { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
