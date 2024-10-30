
using C971MobileApp.Models;
using System.Collections.ObjectModel;

namespace C971MobileApp.Pages;

public partial class ViewTermPage : ContentPage
{
    private int _termId;
    private readonly DBService? _dbService;
    public ObservableCollection<Course> Courses { get; set; } = new();

    public ViewTermPage(int termId)
	{
		InitializeComponent();
        BindingContext = this;
		_termId = termId;
		var dbService = ((App)Application.Current).ServiceProvider.GetService<DBService>();
		_dbService = dbService;

		
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadTermData(_termId);
    }

    private async void LoadTermData(int termId)
    {
        Term term  = await _dbService.GetTermById(termId);

        TermNameLabel.Text = term.TermName;
        StartDateLabel.Text = term.FormattedTermStart;
        EndDateLabel.Text = term.FormattedTermEnd;
        LoadCourses();
    }

    private async void LoadCourses()
    {
        Courses.Clear();
        List<Course> courses = await _dbService.GetAllCourses(_termId);
        foreach (Course course in courses)
        {
            Courses.Add(course);
        }
    }

    private async void EditTermButton_Clicked(Object sender, EventArgs e)
    {
        var editTermPage = new EditTermPage(_termId);
        editTermPage.ChangeMade += () => LoadTermData(_termId);
        await Navigation.PushAsync(editTermPage);

    }

    public async void AddCourseButton_Clicked(Object sender, EventArgs e)
    {
        var addCoursePage = new AddCoursePage(_termId);

        addCoursePage.ChangeMade += () => LoadCourses();
        await Navigation.PushAsync(addCoursePage);
    }

    private async void OnCourseTapped(Object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}