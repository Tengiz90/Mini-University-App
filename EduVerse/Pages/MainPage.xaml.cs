using EduVerse.Data.Implementations;
using EduVerse.Data.Interfaces;

namespace EduVerse.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly IEmailSender _emailSender;
        private readonly IAPIManager _apiManager;
        private readonly IFileManager _fileManager;
        public MainPage(IDatabaseManager databaseManager, IAPIManager apiManager, IEmailSender emailSender, IFileManager fileManager)
        {
            _databaseManager = databaseManager;
            _emailSender = emailSender;
            _apiManager = apiManager;
            _fileManager = fileManager;
            InitializeComponent();
        }
        private async void OnLoginClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.IsEnabled = false;
                await Navigation.PushAsync(new LoginPage(_databaseManager, _apiManager, _emailSender, _fileManager));
                btn.IsEnabled = true;
            }

        }

        private async void OnRegisterClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.IsEnabled = false;
                await Navigation.PushAsync(new RegisterPage(_databaseManager, _apiManager, _emailSender, _fileManager));
                btn.IsEnabled = true;
            }
        }

    }
}
