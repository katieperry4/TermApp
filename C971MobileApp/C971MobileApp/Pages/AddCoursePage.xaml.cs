
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

    private bool CheckInput()
    {
        if (string.IsNullOrWhiteSpace(CourseNameField.Text)||
            string.IsNullOrWhiteSpace(CINameField.Text) ||
            string.IsNullOrWhiteSpace(CIEmailField.Text) ||
            string.IsNullOrWhiteSpace(CIPhoneField.Text)
            )
        {
            return false;
        }
        return true;
    }

	private async void AddCourseButton_Clicked(Object sender, EventArgs e)
	{
        bool checkInput = CheckInput();
        if (checkInput)
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
                _courseNotificationService.ScheduleCourseNotification(newCourse);
            }

            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Validation Error", "Ensure all fields are full", "OK");
            return;
        }
	}
    public async void CancelButton_Clicked(Object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}