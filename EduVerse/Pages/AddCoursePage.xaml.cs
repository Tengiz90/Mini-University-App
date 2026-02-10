using EduVerse.Data.Interfaces;
using EduVerse.Models;

namespace EduVerse.Pages;

public partial class AddCoursePage : ContentPage
{
    private readonly IDatabaseManager _data;
    private readonly int _teacherId;
    public AddCoursePage(IDatabaseManager data, int teacherId)
    {
        InitializeComponent();
        _data = data;
        _teacherId = teacherId;
    }

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        var idOfCourseToAdd = await _data.GetCountOfAllCoursesAsync() + 1;
        string courseName = courseNameEntry.Text?.Trim();
        string? courseDescription = courseDescriptionEntry.Text?.Trim();

        if (string.IsNullOrEmpty(courseName))
        {
            AddCourseErrorMesasage.Text = "Please enter a course name.";
            AddCourseErrorMesasage.IsVisible = true;
            return;
        } else
        {
            _ = _data.AddCourseByTeacherIdAsync(_teacherId, new Course { Id = idOfCourseToAdd, Name = courseName, Description = courseDescription, TeacherId = _teacherId });
            await DisplayAlertAsync("Success", "", "Ok");
            await Navigation.PopAsync();
        }
    }

}