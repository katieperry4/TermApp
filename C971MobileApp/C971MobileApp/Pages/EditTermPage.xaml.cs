using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class EditTermPage : ContentPage
{
    private int _termId;
    private DBService? _dbService;
	public event Action ChangeMade;

    public EditTermPage(int termId)
	{
		InitializeComponent();
		_termId = termId;
        var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
		_dbService = dbService;
        LoadTerm();
	}

	public async void LoadTerm()
	{
		if (_termId != null)
		{
			Term currentTerm = await _dbService.GetTermById(_termId);
			termNameField.Text = currentTerm.TermName;
			startDateField.Date = currentTerm.TermStart;
			endDateField.Date = currentTerm.TermEnd;

		}
	}

	public async void CancelButton_Clicked(Object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	public async void EditTermButton_Clicked(Object sender, EventArgs e)
	{
		if (_termId != null)
		{
			Term currentTerm = await _dbService.GetTermById(_termId);
			currentTerm.TermName = termNameField.Text.Trim();
			currentTerm.TermStart = startDateField.Date;
			currentTerm.TermEnd = endDateField.Date;

			await _dbService.UpdateTerm(currentTerm);
			ChangeMade?.Invoke();
            await Navigation.PopAsync();
        }
	}

	public async void DeleteButton_Clicked(Object sender, EventArgs e)
	{
        if (_termId != null)
        {
            Term currentTerm = await _dbService.GetTermById(_termId);
            await _dbService.DeleteTerm(currentTerm);
            ChangeMade?.Invoke();
            await Navigation.PopAsync();
        }
    }
}