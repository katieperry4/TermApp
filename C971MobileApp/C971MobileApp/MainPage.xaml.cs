using C971MobileApp.Models;
using C971MobileApp.Pages;
using System.Collections.ObjectModel;

namespace C971MobileApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        public ObservableCollection<Term> Terms { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            
        }

        protected  override void OnAppearing()
        {
            base.OnAppearing();
            LoadTerms();
        }

        private async void GoAddTermPage_Clicked(Object sender, EventArgs e)
        {
            var addTermPage = ((App)Application.Current).ServiceProvider.GetService<AddTermPage>();
            //addTermPage.TermAdded += LoadTerms;
            await Navigation.PushAsync(addTermPage);
        }

        public async void LoadTerms()
        {
            Terms.Clear();
            var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();

            if (dbService != null)
            {
                List<Term> termsList = await dbService.GetAllTerms();
                foreach (var term in termsList)
                {
                    Terms.Add(term);
                }
            }
            else
            {
                Console.WriteLine("DBService not available");
            }

        }
        private async void OnTermTapped(Object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var tapGesture = frame.GestureRecognizers.OfType<TapGestureRecognizer>().FirstOrDefault();
            if (tapGesture != null) {
                var termId = tapGesture.CommandParameter;
                if (termId is int termIdInt)
                {
                    await Navigation.PushAsync(new ViewTermPage(termIdInt));

                }
                else
                {
                    Console.WriteLine("Invalid ID type");
                }
            }

            
        }
    }

}
