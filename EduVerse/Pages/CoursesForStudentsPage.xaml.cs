using EduVerse.Data.Interfaces;
using EduVerse.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace EduVerse.Pages;

public partial class CoursesForStudentsPage : ContentPage
{
    private readonly IDatabaseManager _data;
    private readonly IAPIManager _apiManager;
    private readonly IEmailSender _emailSender;
    private readonly IFileManager _fileManager;
    private readonly int _studentId;

    private List<CourseForStudents> _allCourses = new List<CourseForStudents>();

    public ObservableCollection<CourseForStudents> Courses { get; set; } = new ObservableCollection<CourseForStudents>();

    public CoursesForStudentsPage(IDatabaseManager data, IAPIManager apiManager, IEmailSender emailSender, IFileManager fileManager, int studentId)
    {
        InitializeComponent();
        _data = data;
        _apiManager = apiManager;
        _emailSender = emailSender;
        _fileManager = fileManager;
        _studentId = studentId;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        processingIndicator.IsVisible = true;
        enrolledFilterCheckBoxWrapper.IsVisible = false;
        Search.IsVisible = false;
        await LoadCoursesAsync();
        ApplyFilters();
        processingIndicator.IsVisible = false;
        Search.IsVisible = true;
        enrolledFilterCheckBoxWrapper.IsVisible = true;
    }

    private async Task LoadCoursesAsync()
    {
        Courses.Clear();
        _allCourses.Clear();

        DateTime registrationValidUntil = await _apiManager.GetRegistrationOpenUntilDateAsync();

        if (registrationValidUntil == DateTime.MinValue)
        {
            registrationInfoLabel.Text = $"Couldn't get the registration valid until value";
        }
        else
        {
            registrationInfoLabel.Text = $"Registration is valid until: {registrationValidUntil:yyyy-MM-dd}";
        }


        var allCoursesFromDb = await _data.GetAllCoursesAsync();
        foreach (var c in allCoursesFromDb)
        {
            bool isEnrolled = await _data.IsStudentEnrolledInCourseAsync(_studentId, c.Id);

            var courseForStudent = new CourseForStudents
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsEnrolled = isEnrolled,
                ButtonText = isEnrolled ? "Unenroll" : "Enroll",
            };

            _allCourses.Add(courseForStudent);
            Courses.Add(courseForStudent);
        }
    }

    private async void OnEnrollToggleClicked(object sender, EventArgs e)
    {
        bool isRegistrationOpen = await _apiManager.GetIsRegistrationOpenAsync();



        if (sender is Button button && button.CommandParameter is CourseForStudents course)
        {
            if (course.IsEnrolled)
            {
                if (!isRegistrationOpen)
                {
                    await DisplayAlertAsync("Registration Closed", "You cannot unenroll because registration is closed.", "Ok");
                    return;
                }

                await _data.UnenrollStudentFromCourseAsync(_studentId, course.Id);
                course.IsEnrolled = false;
                course.ButtonText = "Enroll";
            }
            else
            {
                if (!isRegistrationOpen)
                {
                    await DisplayAlertAsync("Registration Closed", "You cannot enroll because registration is closed.", "Ok");
                    return;
                }
                await _data.EnrollStudentInCourseAsync(_studentId, course.Id);
                var student = await _data.GetStudentByIdAsync(_studentId);
                _ =  Task.Run(() =>
                {
                    _emailSender.Send(student.Email, "Notice", $"{student.FirstName}, you have successfully enrolled in {course.Name}");
                    _fileManager.Add($"Student with email - {student.Email} has enrolled in {course.Name}");
                }

                 );

                course.IsEnrolled = true;
                course.ButtonText = "Unenroll";
            }

            if (enrolledFilterCheckBox.IsChecked)
                ApplyFilters();


        }
    }

    private async void OnSeeMarksClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CourseForStudents course)
        {
            await Navigation.PushAsync(new ShowMarksPage(_data, _studentId, course.Id));
        }
    }
    private async void OnSeeDescriptionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.BindingContext is CourseForStudents course)
        {
            await DisplayAlertAsync(course.Name, course.Description ?? "No description", "Ok");
        }

    }

    private void OnEnrolledFilterChanged(object sender, CheckedChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }
    private void ApplyFilters()
    {
        string filterString = Search.Text?.Trim() ?? "";
        bool filterEnrolled = enrolledFilterCheckBox.IsChecked;

        Courses.Clear();

        foreach (var c in _allCourses)
        {
            bool matchesSearch = c.Name.Contains(filterString, StringComparison.OrdinalIgnoreCase)
                                 || (c.Description?.Contains(filterString, StringComparison.OrdinalIgnoreCase) ?? false);

            bool matchesEnrolled = !filterEnrolled || c.IsEnrolled;

            if (matchesSearch && matchesEnrolled)
            {
                Courses.Add(c);
            }
        }
    }

}

public class CourseForStudents : BindableObject
{
    private bool isEnrolled;
    private string buttonText = "Enroll";

    public string Name { get; set; }
    public string? Description { get; set; }
    public int Id { get; set; }

    public bool IsEnrolled
    {
        get => isEnrolled;
        set
        {
            if (isEnrolled != value)
            {
                isEnrolled = value;
                OnPropertyChanged();
            }
        }
    }

    public string ButtonText
    {
        get => buttonText;
        set
        {
            if (buttonText != value)
            {
                buttonText = value;
                OnPropertyChanged();
            }
        }
    }


}
