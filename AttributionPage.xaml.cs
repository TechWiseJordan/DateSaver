namespace DateSaver;

public partial class AttributionPage : ContentPage
{
    private readonly LocalDbService _dbService;

    public AttributionPage(LocalDbService dbService)
	{
		InitializeComponent();
        _dbService = dbService;
    }

    private async void backBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage(_dbService));
    }
}