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
		bool checkInput = CheckInput();
		if (checkInput)
		{
			await _dbService.AddTerm(new Term
			{
				TermName = termNameField.Text,
				TermStart = startDateField.Date,
				TermEnd = endDateField.Date
			});
			TermAdded?.Invoke();
			await Navigation.PopAsync();
		} else
		{
            await DisplayAlert("Validation Error", "Ensure all fields are full and dates are correct", "OK");
            return;
        }
	}

    private bool CheckInput()
    {
        if(string.IsNullOrWhiteSpace(termNameField.Text) || endDateField.Date < startDateField.Date) { return false; }
		return true;
    }

    private async void CancelButton_Clicked(Object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}