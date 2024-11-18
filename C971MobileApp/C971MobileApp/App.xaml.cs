using Plugin.LocalNotification;

namespace C971MobileApp
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; }
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            LocalNotificationCenter.Current.RequestNotificationPermission();
            ServiceProvider = serviceProvider;
            MainPage = new NavigationPage(serviceProvider.GetRequiredService<MainPage>());
        }
    }
}
