using C971MobileApp.Pages;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace C971MobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<DBService>();
            builder.Services.AddSingleton<CourseNotificationService>();
            builder.Services.AddSingleton<AssessmentNotificationService>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddTermPage>();
            builder.Services.AddTransient<ViewTermPage>();
            builder.Services.AddTransient<EditTermPage>();
            builder.Services.AddTransient<AddCoursePage>();
            builder.Services.AddTransient<ViewCoursePage>();
            builder.Services.AddTransient<EditCoursePage>();
            builder.Services.AddTransient<ViewNotesPage>();
            builder.Services.AddTransient<AddAssessmentPage>();
            builder.Services.AddTransient<ViewAssessmentPage>();
            builder.Services.AddTransient<EditAssessmentPage>();


            

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            var dbService = app.Services.GetService<DBService>();
            Task.Run(async() => await dbService?.InitializeDB()).Wait();

            return app;
            //return builder.Build();
        }
    }
}
