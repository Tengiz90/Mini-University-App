using EduVerse.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Models
{
    public class CourseCategoryMapping
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public CourseCategory Category { get; set; }
    }
}
