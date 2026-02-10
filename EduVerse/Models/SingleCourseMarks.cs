using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Models
{
    public class SingleCourseMarks
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public double Homework1 { get; set; }  = 0.0;
        public double Homework2 { get; set; }  = 0.0;
        public double Homework3 { get; set; }  = 0.0;
        public double Homework4 { get; set; }  = 0.0;
        public double Homework5 { get; set; }  = 0.0;
        public double Homework6 { get; set; }  = 0.0;
        public double Homework7 { get; set; }  = 0.0;
        public double Homework8 { get; set; }  = 0.0;
        public double Homework9 { get; set; }  = 0.0;
        public double Homework10 { get; set; } = 0.0;
        public double MidtermExam { get; set; } = 0.0;
        public double Presentation { get; set; } = 0.0;
        public double FinalExam { get; set; } = 0.0;
    }
}
