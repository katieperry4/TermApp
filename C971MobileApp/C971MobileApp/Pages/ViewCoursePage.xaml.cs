
using C971MobileApp.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace C971MobileApp.Pages;

public partial class ViewCoursePage : ContentPage
{
    private DBService? _dbService;
    private int _courseId;
    private Course _currentCourse;
    private Assessment? _performanceAssessment;
    private Assessment? _objectiveAssessment;

    public ViewCoursePage(int courseId)
	{
		InitializeComponent();
		var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
		_dbService = dbService;
		_courseId = courseId;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadCourseData(_courseId);
        CheckAssessments(_courseId);
    }

    private async void CheckAssessments(int courseId)
    {
        _performanceAssessment = null;
        _objectiveAssessment = null;
        List<Assessment> assessments = await _dbService.GetAssessments(courseId);
        Assessment performanceAssessment = assessments.FirstOrDefault(a => a.AssessmentType == "Performance");
        Assessment objectiveAssessment = assessments.FirstOrDefault(a => a.AssessmentType == "Objective");
        _performanceAssessment = performanceAssessment;
        _objectiveAssessment = objectiveAssessment;
        if(performanceAssessment != null)
        {
            PerformanceAssessmentName.Text = performanceAssessment.AssessmentName;
            PerformanceAssessmentStart.Text = performanceAssessment.FormattedAssessmentStart;
            PerformanceAssessmentEnd.Text = performanceAssessment.FormattedAssessmentEnd;
        }
        else
        {
            PerformanceAssessmentName.Text = "Add Assessment +";
            PerformanceAssessmentStart.Text = "";
            PerformanceAssessmentEnd.Text = "";
        }
        if (objectiveAssessment != null) 
        {
            ObjectiveAssessmentName.Text = objectiveAssessment.AssessmentName;
            ObjectiveAssessmentStart.Text = objectiveAssessment.FormattedAssessmentStart;
            ObjectiveAssessmentEnd.Text = objectiveAssessment.FormattedAssessmentEnd;
        } else
        {
            ObjectiveAssessmentName.Text = "Add Assessment +";
            ObjectiveAssessmentStart.Text = "";
            ObjectiveAssessmentEnd.Text = "";
        }
    }

    private async void LoadCourseData(int courseId)
    {
        Course currentCourse = await _dbService.GetCourseById(courseId);
        _currentCourse = currentCourse;
        CourseNameLabel.Text = currentCourse.CourseName;
        StartDateLabel.Text = currentCourse.FormattedCourseStart;
        EndDateLabel.Text = currentCourse.FormattedCourseEnd;
        StatusLabel.Text = currentCourse.CourseStatus;
        CINameLabel.Text = currentCourse.CIName;
        CIEmailLabel.Text = currentCourse.CIEmail;
        CIPhoneLabel.Text = currentCourse.CIPhone;
        NotesEditor.Text = currentCourse.CourseNotes;
        NotifLabel.Text = currentCourse.StartNotif ? "Enabled" : "Disabled";
    }

    private async void ShareButton_Clicked(object sender, EventArgs e)
    {
        await ShareText(_currentCourse.CourseNotes, _currentCourse.CourseName + " Notes");
    }

    public async Task ShareText(string text, string title)
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = title
        });
    }

    private async void ViewButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ViewNotesPage(_courseId));
    }
    
    private async void EditCourseButton_Clicked(object sender, EventArgs e)
    {
        var editCoursePage = new EditCoursePage(_courseId);
        await Navigation.PushAsync(editCoursePage);
    }

    private async void PATapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (_performanceAssessment == null)
        {
            //add assessment
            await Navigation.PushAsync(new AddAssessmentPage(_courseId, "Performance"));
        }
        else
        {
            //view assessment
            await Navigation.PushAsync(new ViewAssessmentPage(_performanceAssessment));
        }
    }

    private async void OATapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (_objectiveAssessment == null)
        {
            //add assessment
            await Navigation.PushAsync(new AddAssessmentPage(_courseId, "Objective"));
        }
        else
        {
            //view assessment
            await Navigation.PushAsync(new ViewAssessmentPage(_objectiveAssessment));
        }
    }

}