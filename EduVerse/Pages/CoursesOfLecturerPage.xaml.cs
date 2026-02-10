using EduVerse.Data.Interfaces;
using EduVerse.Models;
using System.Collections.ObjectModel;

namespace EduVerse.Pages;


public partial class CoursesOfLecturerPage : ContentPage
{
    private readonly IDatabaseManager _data;
    private readonly int _teacherId;
    public ObservableCollection<Course> Courses { get; set; }

    public CoursesOfLecturerPage(IDatabaseManager data, int teacherId)
    {
        InitializeComponent();
        _data = data;
        _teacherId = teacherId;
        Courses = new ObservableCollection<Course>();
        BindingContext = this;
    }

    private async Task FillCoursesListWithData()
    {
        var lecturerCourses = await _data.GetCoursesByTeacherIdAsync(_teacherId);
        foreach (var course in lecturerCourses)
            Courses.Add(course);
    }

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCoursePage(_data, _teacherId));

    }

    private async void OnSeeStudentsClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.BindingContext is Course course)
        {
            await Navigation.PushAsync(new StudentsInCoursePage(course, _data));
        }
    }
    private async void OnSeeDescriptionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.BindingContext is Course course)
        {
            await DisplayAlertAsync(course.Name, course.Description ?? "No description", "Ok");
        }

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Courses.Clear();
        await FillCoursesListWithData();
    }

}

