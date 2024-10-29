namespace C971MobileApp
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; }
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            ServiceProvider = serviceProvider;
            MainPage = new NavigationPage(serviceProvider.GetRequiredService<MainPage>());
        }
    }
}
