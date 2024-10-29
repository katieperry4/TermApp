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
            LoadTerms();
        }

        
        private async void GoAddTermPage_Clicked(Object sender, EventArgs e)
        {
            var addTermPage = ((App)Application.Current).ServiceProvider.GetService<AddTermPage>();
            addTermPage.TermAdded += LoadTerms;
            await Navigation.PushAsync(addTermPage);
        }

        public async void LoadTerms()
        {
            Terms.Clear();
            var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
            
            if(dbService != null)
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
    }

}
