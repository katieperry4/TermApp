using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class AddAssessmentPage : ContentPage
{
    private AssessmentNotificationService? _notifService;
    private DBService? _dbService;
    private int _courseId;
    private string _assessmentType;

    public AddAssessmentPage(int courseId, string assessmentType)
    {
        InitializeComponent();
        var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
        var notifService = ((App)Application.Current).ServiceProvider.GetService<AssessmentNotificationService>();
        _notifService = notifService;
        _dbService = dbService;
        _courseId = courseId;

        _assessmentType = assessmentType;
        DisplayAssessmentType();
    }

    private void DisplayAssessmentType()
    {
        AssessmentTypeLabel.Text = "Add " + _assessmentType + " assessment";
    }

    private async void AddAssessmentButton_Clicked(object sender, EventArgs e)
    {
        bool checkInputs = CheckInputs();
        if (checkInputs)
        {
            int assessmentId = await _dbService.AddAssessment(new Assessment
            {
                AssessmentName = AssessmentNameField.Text,
                AssessmentStart = StartDateField.Date,
                AssessmentEnd = EndDateField.Date,
                CourseId = _courseId,
                AssessmentType = _assessmentType,
                StartNotif = StartNotifBox.IsChecked
            });
            Assessment currentAssessment = await _dbService.GetAssessmentById(assessmentId);
            if (StartNotifBox.IsChecked)
            {
                await _notifService.ScheduleAssessmentNotification(currentAssessment);
            }
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Validation Error", "Please enter an assessment name and ensure dates are correct", "OK");
        }
    }

    private bool CheckInputs()
    {
        if (string.IsNullOrWhiteSpace(AssessmentNameField.Text) || EndDateField.Date < StartDateField.Date) { return false; }
        return true;
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}