using EduVerse.Data.Implementations;
using EduVerse.Data.Interfaces;
using EduVerse.Enums;
using EduVerse.Models;
using System.Text.RegularExpressions;

namespace EduVerse.Pages;

public partial class RegisterPage : ContentPage
{
    private Role _selectedRole;
    private readonly IDatabaseManager _databaseManager;
    private readonly IEmailSender _emailSender;
    private readonly IAPIManager _apiManager;
    private readonly IFileManager _fileManager;
    public RegisterPage(IDatabaseManager databaseManager, IAPIManager apiManager, IEmailSender emailSender, IFileManager fileManager)
    {

        _databaseManager = databaseManager;
        _emailSender = emailSender;
        _apiManager = apiManager;
        _fileManager = fileManager;
        InitializeComponent();
    }
    private void OnRoleChanged(object sender, CheckedChangedEventArgs e)
    {
        if (studentRadio.IsChecked)
        {
            _selectedRole = Role.Student;
            majorEntry.IsVisible = true;
        }
        else
        {
            _selectedRole = Role.Teacher;
            majorEntry.IsVisible = false;
            majorErrorMessage.IsVisible = false;
        }
    }
    private async void OnNextClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            btn.IsEnabled = false;
            var isClientSideErrorDetected = false;
            if (emailEntry.Text == null || !emailEntry.Text.Contains("@"))
            {
                emailErrorMessage.IsVisible = true;
                emailErrorMessage.Text = "Email must contain @ symbol";
                isClientSideErrorDetected = true;
            }
            else
            {
                emailErrorMessage.IsVisible = false;

            }
            if (passwordEntry.Text == null || passwordEntry.Text == "" || passwordEntry.Text != repeatPasswordEntry.Text)
            {
                repeatPasswordErrorMessage.IsVisible = true;
                repeatPasswordErrorMessage.Text = "Passwords must match and be filled";
                isClientSideErrorDetected = true;

            }
            else
            {
                repeatPasswordErrorMessage.IsVisible = false;

            }
            if (idEntry.Text == null || idEntry.Text.Trim() == "" || !Regex.IsMatch(idEntry.Text, @"^\d+$"))
            {
                idErrorMessage.IsVisible = true;
                idErrorMessage.Text = "Please enter your id as whole number";
                isClientSideErrorDetected = true;
            }
            else
            {
                idErrorMessage.IsVisible = false;
                idErrorMessage.Text = "";
            }
            if (firstNameEntry.Text == null || firstNameEntry.Text.Trim() == "")
            {
                firstNameErrorMessage.IsVisible = true;
                firstNameErrorMessage.Text = "Please enter your first name";
                isClientSideErrorDetected = true;
            }
            else
            {
                firstNameErrorMessage.IsVisible = false;

            }
            if (lastNameEntry.Text == null || lastNameEntry.Text.Trim() == "")
            {
                lastNameErrorMessage.IsVisible = true;
                lastNameErrorMessage.Text = "Please enter your last name";
                isClientSideErrorDetected = true;
            }
            else
            {
                lastNameErrorMessage.IsVisible = false;

            }
            if (!studentRadio.IsChecked && !lecturerRadio.IsChecked)
            {

                roleSelectErrorMessage.IsVisible = true;
                roleSelectErrorMessage.Text = "Please select a role";
                isClientSideErrorDetected = true;


            }
            else
            {
                roleSelectErrorMessage.IsVisible = false;
            }

            if (_selectedRole == Role.Student)
            {
                if (majorEntry.Text == null || majorEntry.Text.Trim() == "")
                {
                    majorErrorMessage.IsVisible = true;
                    majorErrorMessage.Text = "Please enter your major";
                    isClientSideErrorDetected = true;
                }
                else
                {
                    majorErrorMessage.IsVisible = false;
                }
            }


            if (isClientSideErrorDetected)
            {
                btn.IsEnabled = true;

            }
            else
            {
                processingIndicator.IsVisible = true;
                if (_selectedRole == Role.Student)
                {
                    if (await _databaseManager.DoesStudentWithSuchIdAlreadyExistAsync(int.Parse(idEntry.Text!)))
                    {
                        idErrorMessage.Text = "Student with such id already exists";
                        await mainScroll.ScrollToAsync(0, 0, true);
                        idErrorMessage.IsVisible = true;
                        processingIndicator.IsVisible = false;
                        btn.IsEnabled = true;
                    }
                    else if (await _databaseManager.DoesStudentWithSuchEmailAlreadyExistAsync(emailEntry.Text!.Trim().ToLower()))
                    {
                        emailErrorMessage.Text = "Student with such email already exists";
                        await mainScroll.ScrollToAsync(0, 0, true);
                        emailErrorMessage.IsVisible = true;
                        processingIndicator.IsVisible = false;
                        btn.IsEnabled = true;
                    }
                    else
                    {
                        idErrorMessage.Text = "";
                        idErrorMessage.IsVisible = false;
                        if(await RegisterStudent(int.Parse(idEntry.Text!), firstNameEntry.Text!, lastNameEntry.Text!, emailEntry.Text!, passwordEntry.Text!, dobPicker.Date, majorEntry.Text))
                        {
                            await Navigation.PushAsync(new LoginPage(_databaseManager, _apiManager, _emailSender, _fileManager));
                            ClearFields();
                        }
                        btn.IsEnabled = true;
                        processingIndicator.IsVisible = false;
                    }
                }
                else
                {
                    if (await _databaseManager.DoesTeacherWithSuchIdAlreadyExistAsync(int.Parse(idEntry.Text!)))
                    {
                        idErrorMessage.Text = "Teacher with such id already exists";
                        await mainScroll.ScrollToAsync(0, 0, true);
                        idErrorMessage.IsVisible = true;
                        processingIndicator.IsVisible = false;
                        btn.IsEnabled = true;

                    }
                    else if (await _databaseManager.DoesTeacherWithSuchEmailAlreadyExistAsync(emailEntry.Text!.Trim().ToLower()))
                    {
                        emailErrorMessage.Text = "Teacher with such email already exists";
                        await mainScroll.ScrollToAsync(0, 0, true);
                        emailErrorMessage.IsVisible = true;
                        processingIndicator.IsVisible = false;
                        btn.IsEnabled = true;

                    }
                    else
                    {
                        idErrorMessage.Text = "";
                        idErrorMessage.IsVisible = false;
                        if(await RegisterTeacher(int.Parse(idEntry.Text!), firstNameEntry.Text!, lastNameEntry.Text!, emailEntry.Text!, passwordEntry.Text!, dobPicker.Date))
                        {
                            await Navigation.PushAsync(new LoginPage(_databaseManager, _apiManager, _emailSender, _fileManager));
                            ClearFields();
                        }
                        
                        btn.IsEnabled = true;
                        processingIndicator.IsVisible = false;
                    }
                }
            }
        }
    }

    private async Task<bool> RegisterStudent(int id, string firstName, string lastName, string email, string password, DateTime? date, string major)
    {
        string hashedPassword = await Task.Run(() =>
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        });
        DateTime finalDate = date ?? DateTime.Now;
        DateOnly finalDateOnly = DateOnly.FromDateTime(finalDate);
        var codeToSend = GenereateRandom4DigitCode();
        SendCodeToEmail(email, codeToSend);
        string? code = null;
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            code = await DisplayPromptAsync("We sent you a code on email", $"Please enter the code:{Environment.NewLine}");

        }
        else
        {
            code = await DisplayPromptAsync("We sent you a code on email", $"Please enter the code:");

        }

        if (code != null)
        {
            if (code == codeToSend)
            {
                await _databaseManager.AddStudentAsync(new Student { Id = id, FirstName = firstName, LastName = lastName, Email = email.ToLower(), DateOfBirth = finalDateOnly, PasswordHash = hashedPassword, Major = major });
                _ = Task.Run(() =>
                {
                    _fileManager.Add($"Student with id - {idEntry.Text}, with email - {emailEntry.Text} registered on {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                });
                
                return true;

            }
            else
            {
                await DisplayAlertAsync("Wrong code", "try again", "Ok");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private async Task<bool> RegisterTeacher(int id, string firstName, string lastName, string email, string password, DateTime? date)
    {
        string hashedPassword = await Task.Run(() =>
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        });
        DateTime finalDate = date ?? DateTime.Now;
        DateOnly finalDateOnly = DateOnly.FromDateTime(finalDate);
        var codeToSend = GenereateRandom4DigitCode();
        SendCodeToEmail(email, codeToSend);
        string? code = null;
        
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            code = await DisplayPromptAsync("We sent you a code on email", $"Please enter the code:{Environment.NewLine}");

        }
        else
        {
            code = await DisplayPromptAsync("We sent you a code on email", $"Please enter the code:");

        }

        if (code != null)
        {
            if (code == codeToSend)
            {
                await _databaseManager.AddTeacherAsync(new Teacher { Id = id, FirstName = firstName, LastName = lastName, Email = email.ToLower(), DateOfBirth = finalDateOnly, PasswordHash = hashedPassword });
                _ = Task.Run(() =>
                {
                    _fileManager.Add($"Teacher with id - {idEntry.Text}, with email - {emailEntry.Text} registered on {DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
                });
                return true;

            }
            else
            {
                await DisplayAlertAsync("Wrong code", "try again", "Ok");
                return false;
                
                
            }
        }
        else
        {
            return false;

        }


    }
    private void SendCodeToEmail(string email, string code)
    {
        _emailSender.Send(email, "Activation Code", $"Your code is {code}");

    }

    private string GenereateRandom4DigitCode()
    {
        Random rand = new Random();
        int code = rand.Next(1000, 9999);
        return code.ToString();
    }
    private void ClearFields()
    {
        idEntry.Text = "";
        firstNameEntry.Text = "";
        lastNameEntry.Text = "";
        dobPicker.Date = DateTime.Today;
        emailEntry.Text = "";
        passwordEntry.Text = "";
        repeatPasswordEntry.Text = "";
        studentRadio.IsChecked = lecturerRadio.IsChecked = false;
        majorEntry.Text = "";
        majorEntry.IsVisible = false;

    }
}
