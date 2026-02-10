using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PasswordHash { get; set; }
        public string Major { get; set; }

        public List<SingleCourseMarks> CourseMarks { get; set; } = new List<SingleCourseMarks>();

        public string FullName => $"{FirstName} {LastName}";
    }
}
