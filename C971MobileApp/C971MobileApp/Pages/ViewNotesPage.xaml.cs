
using C971MobileApp.Models;

namespace C971MobileApp.Pages;

public partial class ViewNotesPage : ContentPage
{
    private DBService? _dbService;
    private int _courseId;

    public ViewNotesPage(int courseId)
	{
		InitializeComponent();
        var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
        _dbService = dbService;
		_courseId = courseId;
		LoadNotes();
	}

    private async void LoadNotes()
    {
        Course currentCourse = await _dbService.GetCourseById(_courseId );
        BindingContext = currentCourse;

    }
}