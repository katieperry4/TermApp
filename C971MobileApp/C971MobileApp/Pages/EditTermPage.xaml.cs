using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class EditTermPage : ContentPage
{
    private int _termId;
    private DBService? _dbService;
    private CourseNotificationService? _courseNotificationService;
    private AssessmentNotificationService? _assessmentNotificationService;

    public event Action ChangeMade;

    public EditTermPage(int termId)
	{
		InitializeComponent();
        var courseNotificationService = ((App)Application.Current).ServiceProvider.GetService<CourseNotificationService>();
        var assessmentNotificationService = ((App)Application.Current).ServiceProvider.GetService<AssessmentNotificationService>();
        _courseNotificationService = courseNotificationService;
        _assessmentNotificationService = assessmentNotificationService;

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

			if(string.IsNullOrWhiteSpace(currentTerm.TermName) || currentTerm.TermEnd < currentTerm.TermStart)
			{
                await DisplayAlert("Validation Error", "Ensure all fields are full and dates are correct", "OK");
                return;

            } else
			{
                await _dbService.UpdateTerm(currentTerm);
                ChangeMade?.Invoke();
                await Navigation.PopAsync();
            }

            
        }
	}

	public async void DeleteButton_Clicked(Object sender, EventArgs e)
	{
        var confirmDelete = await DisplayAlert("Confirm", "Are you sure you wish to delete this Term?", "Yes", "No");

        if (_termId != null && confirmDelete)
        {
			List<Course> termCourses = await _dbService.GetAllCourses(_termId);
			foreach(Course course in termCourses)
			{
                List<Assessment> associatedAssessments = await _dbService.GetAssessments(course.CourseID);
                foreach (Assessment assessment in associatedAssessments)
                {
                    await _dbService.DeleteAssessment(assessment);
                    await _assessmentNotificationService.CancelAssessmentNotification(assessment.AssessmentId);
                }
                await _courseNotificationService.CancelCourseNotification(course.CourseID);
                await _dbService.DeleteCourse(course);
			}

            Term currentTerm = await _dbService.GetTermById(_termId);
            await _dbService.DeleteTerm(currentTerm);
			//currentTerm = null;
           
            await Navigation.PopToRootAsync();
        }
    }
}