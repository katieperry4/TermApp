
using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class ViewTermPage : ContentPage
{
    private int _termId;
    private readonly DBService? _dbService;

    public ViewTermPage(int termId)
	{
		InitializeComponent();
		_termId = termId;
		var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
		_dbService = dbService;

		LoadTermData(termId);
	}

    private async void LoadTermData(int termId)
    {
        Term term  = await _dbService.GetTermById(termId);

        TermNameLabel.Text = term.TermName;
    }
}