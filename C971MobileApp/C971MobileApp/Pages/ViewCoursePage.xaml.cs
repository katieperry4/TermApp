
using C971MobileApp.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace C971MobileApp.Pages;

public partial class ViewCoursePage : ContentPage
{
    private DBService? _dbService;
    private int _courseId;
    private Course _currentCourse;

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
        _currentCourse = currentCourse;
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

    private void PATapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OATapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        throw new NotImplementedException();
    }

}