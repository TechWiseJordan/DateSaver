using System.Linq;

namespace DateSaver;

public partial class MainPage : ContentPage
{
    private readonly LocalDbService _dbService;
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

            resultsFromSQL = CalculateCountDown(resultsFromSQL);

            foreach (Date date in resultsFromSQL)
            {
                // Check if date needs to be repeated
                if (date.CountDown < 0 && date.RepeatDate == true)
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
            resultsFromSQL = CalculateCountDown(resultsFromSQL);

            // Sort the list by date
            resultsFromSQL.Sort((x, y) => DateTime.Compare(x.DateSaved, y.DateSaved));

            listView.ItemsSource = resultsFromSQL;
        }
        catch (Exception e)
        {
            Console.WriteLine("First time startup error");
            Console.WriteLine(e.Message);
        }
    }

    private List<Date> CalculateCountDown(List<Date> dateList)
    {
        foreach (Date date in dateList)
        {
            date.CountDown = (date.DateSaved.Date - currentDate.Date).Days;
        }

        return dateList;
    }



    private async void creditsBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AttributionPage(_dbService));
    }

    private async void newBtn_Clicked(object sender, EventArgs e)
    {
        // Go to create page with no data
        await Navigation.PushModalAsync(new CreatePage(_dbService));

        listView.ItemsSource = await _dbService.GetDates();
    }

    private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        // Go to create page with data
        await Navigation.PushModalAsync(new CreatePage(_dbService, e));
    }
}