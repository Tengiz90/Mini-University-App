using EduVerse.Data.Interfaces;
using EduVerse.Models;
using System.Collections.ObjectModel;

namespace EduVerse.Pages;


public partial class StudentsInCoursePage : ContentPage
{
    public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

    private readonly Course _course;
    private readonly IDatabaseManager _dataSource;

    public StudentsInCoursePage(Course selectedCourse, IDatabaseManager data)
    {
        InitializeComponent();
        _course = selectedCourse;
        _dataSource = data;


        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        courseLabel.Text = $"Students in {_course.Name}";

        Students.Clear();
        var enrolledStudents = await _dataSource.GetStudentsByCourseIdAsync(_course.Id);

        foreach (var student in enrolledStudents)
        {
            Students.Add(student);
        }
    }
    private async void OnManageMarksClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var student = button?.CommandParameter as Student;

        if (student != null)
        {
            await Navigation.PushAsync(new ManageMarksPage(student.Id, _course.Id, _dataSource));
        }
    }
}

