namespace DateSaver;

public partial class MainPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editDateId;
    private DateTime currentDate = DateTime.Now;
    private int countDown;


    public MainPage()
    {
        InitializeComponent();
    }

    public MainPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetupDateListView();   
    }

    private async void SetupDateListView()
    {
        List<Date> resultsFromSQL = await _dbService.GetDates();

        foreach (Date date in resultsFromSQL)
        {
            //Calculate date countdown
            date.CountDown = (date.DateSaved.Date - currentDate.Date).Days;

            //Check if date needs to be repeated
            if (date.CountDown < 0 && date.RepeatDate == true) 
            {
                await _dbService.Update(new Date
                {
                    Id = date.Id,
                    DateName = date.DateName,
                    //Description = date.Description,
                    DateSaved = date.DateSaved.AddYears(1),
                    RepeatDate = date.RepeatDate
                });

                //Recalculate date countdown
                date.CountDown = (date.DateSaved.Date - currentDate.Date).Days;
            }
        }

        listView.ItemsSource = resultsFromSQL;
    }


    private async void newBtn_Clicked(object sender, EventArgs e)
    {

        // Go to create page
        await Navigation.PushModalAsync(new CreatePage(_dbService, _editDateId));

        listView.ItemsSource = await _dbService.GetDates();
    }

    private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var date = (Date)e.Item;
        countDown = (date.DateSaved.Date - currentDate.Date).Days;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":

                await Navigation.PushModalAsync(new CreatePage(_dbService, _editDateId, e));

                break;
            case "Delete":

                await _dbService.Delete(date);
                SetupDateListView();

                break;
        }
    }
}
