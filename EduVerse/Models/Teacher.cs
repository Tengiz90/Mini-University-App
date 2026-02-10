using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PasswordHash { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();

    }
}
