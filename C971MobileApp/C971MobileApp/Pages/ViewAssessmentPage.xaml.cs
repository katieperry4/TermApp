using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class ViewAssessmentPage : ContentPage
{
    private Assessment _currentAssessment;

    public ViewAssessmentPage(Assessment currentAssessment)
	{
		InitializeComponent();
		_currentAssessment = currentAssessment;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadAssessmentData();
    }

    private void LoadAssessmentData()
    {
        AssessmentNameLabel.Text = _currentAssessment.AssessmentName;
        StartDateLabel.Text = _currentAssessment.FormattedAssessmentStart;
        EndDateLabel.Text = _currentAssessment.FormattedAssessmentEnd;
        NotifLabel.Text = _currentAssessment.StartNotif ? "Enabled" : "Disabled";
    }

    private async void EditAssessmentButton_Clicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new EditAssessmentPage(_currentAssessment));
	}
}