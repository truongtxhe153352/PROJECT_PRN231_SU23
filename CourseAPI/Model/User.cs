using System;
using System.Collections.Generic;

namespace CourseAPI.Model
{
    public partial class User
    {
        public User()
        {
            Assignments = new HashSet<Assignment>();
            Materials = new HashSet<Material>();
            SubmitAssignments = new HashSet<SubmitAssignment>();
            Courses = new HashSet<Course>();
        }

        public int UserId { get; set; }
        public string? Fullname { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<SubmitAssignment> SubmitAssignments { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
