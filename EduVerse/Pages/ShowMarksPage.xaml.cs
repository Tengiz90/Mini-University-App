using EduVerse.Data.Interfaces;
using EduVerse.Models;

namespace EduVerse.Pages;

public partial class ShowMarksPage : ContentPage
{

    private readonly IDatabaseManager _data;
    private readonly int _studentId;
    private readonly int _courseId;

    public ShowMarksPage(IDatabaseManager data, int studentId, int courseId)
    {
        _studentId = studentId;
        _courseId = courseId;
        InitializeComponent();
        _data = data;

    
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var marks = await _data.ViewStudentMarksByCourseAsync(_studentId, _courseId);

        if (marks == null)
        {
            studentForCourseMarksLabel.Text = "No marks available for this course.";
            return;
        }


        homework1Label.Text = marks.Homework1.ToString();
        homework2Label.Text = marks.Homework2.ToString();
        homework3Label.Text = marks.Homework3.ToString();
        homework4Label.Text = marks.Homework4.ToString();
        homework9Label.Text = marks.Homework9.ToString();
        homework5Label.Text = marks.Homework5.ToString();
        homework6Label.Text = marks.Homework6.ToString();
        homework7Label.Text = marks.Homework7.ToString();
        homework8Label.Text = marks.Homework8.ToString();
        homework10Label.Text = marks.Homework10.ToString();
        midtermExamLabel.Text = marks.MidtermExam.ToString();
        presentationLabel.Text = marks.Presentation.ToString();
        finalExamLabel.Text = marks.FinalExam.ToString();

        
    }
}

