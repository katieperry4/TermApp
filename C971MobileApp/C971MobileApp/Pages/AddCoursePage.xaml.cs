
namespace C971MobileApp.Pages;

public partial class AddCoursePage : ContentPage
{
    internal event Action ChangeMade;
    private int _termId;

    public AddCoursePage(int termId)
	{
		InitializeComponent();
		_termId = termId;
	}

	private async void AddCourseButton_Clicked(Object sender, EventArgs e)
	{
		throw new NotImplementedException();
	}
    public async void CancelButton_Clicked(Object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}