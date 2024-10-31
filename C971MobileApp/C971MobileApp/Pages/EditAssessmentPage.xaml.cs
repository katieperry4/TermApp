using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class EditAssessmentPage : ContentPage
{
    private Assessment _currentAssessment;
    private AssessmentNotificationService? _notifService;
    private DBService? _dbService;

    public EditAssessmentPage(Assessment currentAssessment)
    {
        InitializeComponent();
        _currentAssessment = currentAssessment;
        var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
        var notifService = ((App)Application.Current).ServiceProvider.GetService<AssessmentNotificationService>();
        _notifService = notifService;
        _dbService = dbService;
        LoadAssessmentData();
    }

    private void LoadAssessmentData()
    {
        AssessmentNameField.Text = _currentAssessment.AssessmentName;
        StartDateField.Date = _currentAssessment.AssessmentStart;
        EndDateField.Date = _currentAssessment.AssessmentEnd;
        if (_currentAssessment.StartNotif)
        {
            StartNotifBox.IsChecked = true;
        }
        else
        {
            StartNotifBox.IsChecked = false;
        }
    }

    private bool CheckInputs()
    {
        if (string.IsNullOrWhiteSpace(AssessmentNameField.Text) || EndDateField.Date < StartDateField.Date) { return false; }
        return true;
    }
    private async void EditAssessmentButton_Clicked(object sender, EventArgs e)
    {
        bool checkInputs = CheckInputs();
        if (checkInputs)
        {
            _currentAssessment.AssessmentName = AssessmentNameField.Text;
            _currentAssessment.AssessmentStart = StartDateField.Date;
            _currentAssessment.AssessmentEnd = EndDateField.Date;
            _currentAssessment.StartNotif = StartNotifBox.IsChecked;

            await _dbService.UpdateAssessment(_currentAssessment);
            if (StartNotifBox.IsChecked && _currentAssessment.StartNotif == true)
            {
                await _notifService.UpdateAssessmentNotification(_currentAssessment);
            }
            //if there wasn't any scheduled, but they want one now, schedule new notifs
            else if (StartNotifBox.IsChecked && _currentAssessment.StartNotif == false)
            {

                await _notifService.ScheduleAssessmentNotification(_currentAssessment);
            }
            //if they previously wanted notifs, but now don't, cancel previously scheduled notifs
            else if (!StartNotifBox.IsChecked && _currentAssessment.StartNotif == true)
            {
                await _notifService.CancelAssessmentNotification(_currentAssessment.AssessmentId);
            }

            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Validation Error", "Ensure all fields are full and dates are correct", "OK");
            return;
        }
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void deleteButton_Clicked(object sender, EventArgs e)
    {
        var confirmDelete = await DisplayAlert("Confirm", "Are you sure you wish to delete this Assessment?", "Yes", "No");

        if (_currentAssessment != null && confirmDelete)
        {
            await _dbService.DeleteAssessment(_currentAssessment);
            if (_currentAssessment.StartNotif == true)
            {
                await _notifService.CancelAssessmentNotification(_currentAssessment.AssessmentId);

            }

            var navigationStack = Navigation.NavigationStack;
            Navigation.RemovePage(navigationStack[navigationStack.Count - 2]);
            await Navigation.PopAsync();

        }
    }
}