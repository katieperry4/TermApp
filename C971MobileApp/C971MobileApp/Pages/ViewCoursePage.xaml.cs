
using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class ViewCoursePage : ContentPage
{
    private DBService? _dbService;
    private int _courseId;

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
    }

    private async void LoadCourseData(int courseId)
    {
        Course currentCourse = await _dbService.GetCourseById(courseId);
        CourseNameLabel.Text = currentCourse.CourseName;
        StartDateLabel.Text = currentCourse.FormattedCourseStart;
        EndDateLabel.Text = currentCourse.FormattedCourseEnd;
        StatusLabel.Text = currentCourse.CourseStatus;
        CINameLabel.Text = currentCourse.CIName;
        CIEmailLabel.Text = currentCourse.CIEmail;
        CIPhoneLabel.Text = currentCourse.CIPhone;
        NotesEditor.Text = currentCourse.CourseNotes;
        if(currentCourse.StartNotif == true)
        {
            StartNotifBox.IsChecked = true;
        } else
        {
            StartNotifBox.IsChecked = false;
        }
    }

    private void ShareButton_Clicked(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ViewButton_Clicked(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
    
    private void EditCourseButton_Clicked(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}