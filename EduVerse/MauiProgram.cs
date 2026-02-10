using EduVerse.Data;
using EduVerse.Data.Implementations;
using EduVerse.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduVerse
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddHttpClient<IAPIManager, APIManager>();
            builder.Services.AddScoped<IDatabaseManager, DatabaseManager>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddSingleton<IFileManager>(provider =>
            {
                string logPath = Path.Combine(FileSystem.AppDataDirectory, "Logs.txt");
                return new FileManager { FilePath = logPath };
            });
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "EduVerse.db");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            //if (File.Exists(dbPath))
            //{
            //    File.Delete(dbPath);
            //}
#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Ensure SQLite database is created
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            return app;
        }
    }
}
