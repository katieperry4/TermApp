
using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class EditCoursePage : ContentPage
{
    private int _courseId;
    private CourseNotificationService? _courseNotificationService;
    private AssessmentNotificationService? _assessmentNotificationService;
    private DBService? _dbService;
    private Course _currentCourse;

    public EditCoursePage(int courseId)
    {
        InitializeComponent();
        _courseId = courseId;
        var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
        var courseNotificationService = ((App)Application.Current).ServiceProvider.GetService<CourseNotificationService>();
        var assessmentNotificationService = ((App)Application.Current).ServiceProvider.GetService<AssessmentNotificationService>();
        _courseNotificationService = courseNotificationService;
        _assessmentNotificationService = assessmentNotificationService;
        _dbService = dbService;
        LoadCourseInfo(_courseId);


        //BindingContext = _currentCourse;
    }

    private async void LoadCourseInfo(int courseId)
    {
        Course currentCourse = await _dbService.GetCourseById(courseId);
        _currentCourse = currentCourse;
        BindingContext = _currentCourse;
        if (_currentCourse.StartNotif == true)
        {
            StartNotifBox.IsChecked = true;
        }
        else
        {
            StartNotifBox.IsChecked = false;
        }

    }

    private async void cancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();

    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        int termId = _currentCourse.TermId;
        bool checkInput = CheckInput();
        if (checkInput)
        {
            int selectedIndex = StatusPicker.SelectedIndex;

            _currentCourse.TermId = termId;
            _currentCourse.CourseName = CourseNameLabel.Text;
            _currentCourse.CourseStart = StartDateField.Date;
            _currentCourse.CourseEnd = EndDateField.Date;
            _currentCourse.CourseStatus = StatusPicker.SelectedItem?.ToString();
            _currentCourse.CIName = CINameField.Text;
            _currentCourse.CIEmail = CIEmailField.Text;
            _currentCourse.CIPhone = CIPhoneField.Text;
            _currentCourse.CourseNotes = NotesField.Text;
            _currentCourse.StartNotif = StartNotifBox.IsChecked;
            _currentCourse.EndNotif = false;

            int newCourseId = await _dbService.UpdateCourse(_currentCourse);

            //update notifications, ex. edited course start, need to edit notifs
            if (StartNotifBox.IsChecked && _currentCourse.StartNotif == true)
            {
                Course newCourse = await _dbService.GetCourseById(newCourseId);
                await _courseNotificationService.UpdateCourseNotification(newCourse);
            }
            //if there wasn't any scheduled, but they want one now, schedule new notifs
            else if (StartNotifBox.IsChecked && _currentCourse.StartNotif == false)
            {
                Course newCourse = await _dbService.GetCourseById(newCourseId);
                await _courseNotificationService.ScheduleCourseNotification(newCourse);
            }
            //if they previously wanted notifs, but now don't, cancel previously scheduled notifs
            else if (!StartNotifBox.IsChecked && _currentCourse.StartNotif == true)
            {
                await _courseNotificationService.CancelCourseNotification(newCourseId);
            }

            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Validation Error", "Ensure all fields are full and dates are correct", "OK");
            return;
        }
    }

    private async void deleteButton_Clicked(object sender, EventArgs e)
    {
        var confirmDelete = await DisplayAlert("Confirm", "Are you sure you wish to delete this Course?", "Yes", "No");
        if (confirmDelete)
        {
            List<Assessment> associatedAssessments = await _dbService.GetAssessments(_courseId);
            if (_currentCourse != null)
            {
                await _dbService.DeleteCourse(_currentCourse);
                foreach (Assessment assessment in associatedAssessments)
                {
                    await _dbService.DeleteAssessment(assessment);
                    await _assessmentNotificationService.CancelAssessmentNotification(assessment.AssessmentId);
                }
                await _courseNotificationService.CancelCourseNotification(_courseId);
                var navigationStack = Navigation.NavigationStack;
                Navigation.RemovePage(navigationStack[navigationStack.Count - 2]);
                await Navigation.PopAsync();

            }
        }
        return;
    }

    private bool CheckInput()
    {
        if (string.IsNullOrWhiteSpace(CourseNameLabel.Text) ||
            string.IsNullOrWhiteSpace(CINameField.Text) ||
            string.IsNullOrWhiteSpace(CIEmailField.Text) ||
            string.IsNullOrWhiteSpace(CIPhoneField.Text)
            )
        {
            return false;
        }
        return true;
    }


    
}