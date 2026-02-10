using EduVerse.Data.Implementations;
using EduVerse.Data.Interfaces;
using EduVerse.Enums;
using EduVerse.Models;
namespace EduVerse.Pages;

public partial class LoginPage : ContentPage
{
    private Role? _selectedRole = null;
    private readonly IDatabaseManager _databaseManager;
    private readonly IAPIManager _apiManager;
    private readonly IEmailSender _emailSender;
    private readonly IFileManager _fileManager;
    public LoginPage(IDatabaseManager databaseManager, IAPIManager apiManager, IEmailSender emailSender, IFileManager fileManager)
    {
        _apiManager = apiManager;
        _databaseManager = databaseManager;
        _emailSender = emailSender;
        _fileManager = fileManager;
        InitializeComponent();
    }

    private void OnRoleChanged(object sender, CheckedChangedEventArgs e)
    {
        if (studentRadio.IsChecked)
            _selectedRole = Role.Student;
        else if (lecturerRadio.IsChecked)
            _selectedRole = Role.Teacher;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            var emailString = emailEntry?.Text.ToLower() ?? "";
            var passwordString = passwordEntry?.Text ?? "";

            if (_selectedRole == null)
            {
                ShowError("Please select role");
            }
            else
            {
                HideError();

                if (emailString == null || passwordString == null || emailString.Trim() == "" || passwordString.Trim() == "")
                {
                    ShowError("Please fill all fields");
                }
                else
                {
                    if (_selectedRole == Role.Teacher)
                    {
                        if (await _databaseManager.DoesTeacherWithSuchEmailandPasswordExistAsync(emailString, passwordString))
                        {
                            var teacher = await _databaseManager.GetTeacherByEmailAsync(emailString);
                            await Navigation.PushAsync(new CoursesOfLecturerPage(_databaseManager, teacher.Id));
                            _ = Task.Run(() =>
                            {
                                _fileManager.Add($"Teacher with email - {emailString}, logged in on {DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
                            });
                        }
                        else
                        {
                            ShowError("No teacher with such email and password found");

                        }
                    }
                    else
                    {
                        if (await _databaseManager.DoesStudentWithSuchEmailandPasswordExistAsync(emailString, passwordString))
                        {
                            var student = await _databaseManager.GetStudentByEmailAsync(emailString);
                            await Navigation.PushAsync(new CoursesForStudentsPage(_databaseManager, _apiManager, _emailSender, _fileManager, student.Id));
                            _ = Task.Run(() =>
                            {
                                _fileManager.Add($"Student with email - {emailString}, logged in on {DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
                            });
                            //DisplayAlertAsync("Log File Path", _fileManager.FilePath, "OK");

                        }
                        else
                        {
                            ShowError("No student with such email and password found");
                        }
                    }
                }

            }



        }
    }

    private void ShowError(string message)
    {
        errorLabel.Text = message;
        errorLabel.IsVisible = true;
    }

    private void HideError()
    {
        errorLabel.Text = "";
        errorLabel.IsVisible = false;
    }
    public void resetUI()
    {
        emailEntry.Text = string.Empty;
        passwordEntry.Text = string.Empty;

        studentRadio.IsChecked = false;
        lecturerRadio.IsChecked = false;
        _selectedRole = null;

        errorLabel.Text = string.Empty;
        errorLabel.IsVisible = false;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        resetUI();
    }
}