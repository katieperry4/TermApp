
using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class AddCoursePage : ContentPage
{
    internal event Action ChangeMade;

    private DBService? _dbService;
    private int _termId;
    private CourseNotificationService _courseNotificationService;

    public AddCoursePage(int termId)
	{
		InitializeComponent();
		var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
		var courseNotificationService = ((App)Application.Current).ServiceProvider.GetService<CourseNotificationService>();
        _dbService = dbService;
        _termId = termId;
        _courseNotificationService = courseNotificationService;
	}

    private async Task<bool> CheckInput()
    {
        if (string.IsNullOrWhiteSpace(CourseNameField.Text)||
            string.IsNullOrWhiteSpace(CINameField.Text) ||
            string.IsNullOrWhiteSpace(CIEmailField.Text) ||
            string.IsNullOrWhiteSpace(CIPhoneField.Text) || EndDateField.Date < StartDateField.Date
            )
        {
            await DisplayAlert("Validation Error", "Ensure all fields are full and dates are correct", "OK");

            return false;
        }
        return true;
    }



	private async void AddCourseButton_Clicked(Object sender, EventArgs e)
	{
        bool checkCourseCount = await CheckCourseCount();
        bool checkInput = await CheckInput();
        if (checkInput && checkCourseCount)
        {
            int selectedIndex = StatusPicker.SelectedIndex;
            int newCourseId = await _dbService.AddCourse(new Course
            {
                TermId = _termId,
                CourseName = CourseNameField.Text,
                CourseStart = StartDateField.Date,
                CourseEnd = EndDateField.Date,
                CourseStatus = (string)StatusPicker.ItemsSource[selectedIndex],
                CIName = CINameField.Text,
                CIEmail = CIEmailField.Text,
                CIPhone = CIPhoneField.Text,
                CourseNotes = NotesField.Text,
                StartNotif = StartNotifBox.IsChecked,
                EndNotif = false
            });
            if(StartNotifBox.IsChecked)
            {
                Course newCourse = await _dbService.GetCourseById(newCourseId);
                await _courseNotificationService.ScheduleCourseNotification(newCourse);
            }

            await Navigation.PopAsync();
        }
        else
        {
            return;
        }
	}

    private async Task<bool> CheckCourseCount()
    {
        List<Course> courseList = await _dbService.GetAllCourses(_termId);
        if(courseList.Count >= 6)
        {
            await DisplayAlert("Error", "You cannot have more than six courses per term.", "OK");

            return false;
        }
        return true;
    }

    public async void CancelButton_Clicked(Object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}