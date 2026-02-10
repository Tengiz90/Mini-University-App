using EduVerse.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<CourseCategoryMapping> CourseCategories { get; set; } = new List<CourseCategoryMapping>();
        public List<SingleCourseMarks> CourseMarks { get; set; } = new List<SingleCourseMarks>();
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
