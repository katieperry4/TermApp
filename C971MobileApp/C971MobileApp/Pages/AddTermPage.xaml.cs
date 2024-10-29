using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class AddTermPage : ContentPage
{
	private readonly DBService _dbService;
	public event Action TermAdded;
	public AddTermPage(DBService dbService)
	{
		InitializeComponent();
		_dbService = dbService;
	}

	private async void AddTermButton_Clicked(Object sender, EventArgs e)
	{
		await _dbService.AddTerm(new Term
		{
			TermName = termNameField.Text,
			TermStart = startDateField.Date,
			TermEnd = endDateField.Date
		});
		TermAdded?.Invoke();
		await Navigation.PopAsync();
	}

	private async void CancelButton_Clicked(Object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}