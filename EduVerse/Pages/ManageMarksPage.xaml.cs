using EduVerse.Data.Interfaces;
using EduVerse.Models;

namespace EduVerse.Pages;

public partial class ManageMarksPage : ContentPage
{
    private readonly int _studentId;
    private readonly int _courseId;
    private readonly IDatabaseManager _data;

    public ManageMarksPage(int studentId, int courseId, IDatabaseManager data)
    {
        InitializeComponent();

        _studentId = studentId;
        _courseId = courseId;
        _data = data;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var student = await _data.GetStudentByIdAsync(_studentId);
        var course = await _data.GetCourseByIdAsync(_courseId);

    studentForCourseMarksLabel.Text = $"{student?.FirstName} {student?.LastName} - {course?.Name}";

        setupMarks();
    }
    private async void setupMarks()
    {
        var marks = await _data.ViewStudentMarksByCourseAsync(_studentId, _courseId);
        if (marks != null)
        {
            homework1Entry.Text = marks.Homework1.ToString();
            homework2Entry.Text = marks.Homework2.ToString();
            homework3Entry.Text = marks.Homework3.ToString();
            homework4Entry.Text = marks.Homework4.ToString();
            homework5Entry.Text = marks.Homework5.ToString();
            homework6Entry.Text = marks.Homework6.ToString();
            homework7Entry.Text = marks.Homework7.ToString();
            homework8Entry.Text = marks.Homework8.ToString();
            homework9Entry.Text = marks.Homework9.ToString();
            homework10Entry.Text = marks.Homework10.ToString();
            midtermExamEntry.Text = marks.MidtermExam.ToString();
            presentationEntry.Text = marks.Presentation.ToString();
            finalExamEntry.Text = marks.FinalExam.ToString();
        }
    }

    private async void OnSaveMarksClicked(object sender, EventArgs e)
    {
        var marks = new SingleCourseMarks
        {
            StudentId = _studentId,
            CourseId = _courseId,
            Homework1 = ParseMark(homework1Entry.Text),
            Homework2 = ParseMark(homework2Entry.Text),
            Homework3 = ParseMark(homework3Entry.Text),
            Homework4 = ParseMark(homework4Entry.Text),
            Homework5 = ParseMark(homework5Entry.Text),
            Homework6 = ParseMark(homework6Entry.Text),
            Homework7 = ParseMark(homework7Entry.Text),
            Homework8 = ParseMark(homework8Entry.Text),
            Homework9 = ParseMark(homework9Entry.Text),
            Homework10 = ParseMark(homework10Entry.Text),
            MidtermExam = ParseMark(midtermExamEntry.Text),
            Presentation = ParseMark(presentationEntry.Text),
            FinalExam = ParseMark(finalExamEntry.Text)
        };

        bool updated = await _data.ModifyStudentMarksAsync(marks);

        if (!updated)
        {
            await _data.AddMarksByCourseIdAndStudentIdAsync(_courseId, _studentId, marks);
        }

        await DisplayAlertAsync("Marks Saved",
            $"Marks saved for {studentForCourseMarksLabel.Text}.",
            "OK");

        await Navigation.PopAsync();
    }

    private double ParseMark(string text)
    {
        return double.TryParse(text, out var result) ? result : 0;
    }
}