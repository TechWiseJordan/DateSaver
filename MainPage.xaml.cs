using System.Linq;

namespace DateSaver;

public partial class MainPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editDateId;
    private DateTime currentDate = DateTime.Now;


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

    public async void SetupDateListView()
    {
        try
        {
            List<Date> resultsFromSQL = await _dbService.GetDates();


            foreach (Date date in resultsFromSQL)
            {
                // Calculate date countdown
                date.CountDown = (date.DateSaved.Date - currentDate.Date).Days;
            }

            foreach (Date date in resultsFromSQL)
            {
                // Check if date needs to be repeated
                if (date.CountDown < 0 && date.RepeatDate == true) // <-- CountDown is always ZERO here stupid
                {
                    while (date.DateSaved <= currentDate)
                    {
                        date.DateSaved = date.DateSaved.AddYears(1);
                    }

                    await _dbService.Update(new Date
                    {
                        Id = date.Id,
                        DateName = date.DateName,
                        DateSaved = date.DateSaved,
                        RepeatDate = date.RepeatDate
                    });
                }
            }

            // This recalculates the countdown and should really by done in the foreach above
            foreach (Date date in resultsFromSQL)
            {
                // Calculate date countdown
                date.CountDown = (date.DateSaved.Date - currentDate.Date).Days;
            }

            resultsFromSQL.Sort((x, y) => DateTime.Compare(x.DateSaved, y.DateSaved));

            listView.ItemsSource = resultsFromSQL;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }


    private async void newBtn_Clicked(object sender, EventArgs e)
    {
        // Go to create page with no data
        await Navigation.PushModalAsync(new CreatePage(_dbService, _editDateId));

        listView.ItemsSource = await _dbService.GetDates();
    }

    private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        // Go to create page with data
        await Navigation.PushModalAsync(new CreatePage(_dbService, _editDateId, e));
    }

    private async void creditsBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AttributionPage(_dbService));
    }
}