using Microsoft.Maui.Controls.Internals;
using System.Linq;

namespace DateSaver;

public partial class MainPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private DateTime currentDate = DateTime.Now;
    private DateTime tempDate;


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
                switch (date.CountDown, date.RepeatDate, date.TrackAge)
                {
                    // Repeat, Track
                    case ( < 0, true, true):
                        while (date.DateSaved <= currentDate)
                        {
                            date.DateSaved = date.DateSaved.AddYears(1);
                            date.Age = CalculateAge(date.OriginalDateSaved);
                        }

                        await _dbService.Update(new Date
                        {
                            Id = date.Id,
                            DateName = date.DateName,
                            DateSaved = date.DateSaved,
                            OriginalDateSaved = date.OriginalDateSaved,
                            RepeatDate = date.RepeatDate,
                            Age = date.Age,
                            TrackAge = date.TrackAge
                        });

                        break;

                        // Repeat, Track
                    case ( > 0, true, true):
                        date.Age = CalculateAge(date.OriginalDateSaved);

                        break;

                    // Repeat, Do not Track
                    case ( < 0, true, false):
                        while (date.DateSaved <= currentDate)
                        {
                            date.DateSaved = date.DateSaved.AddYears(1);
                        }

                        await _dbService.Update(new Date
                        {
                            Id = date.Id,
                            DateName = date.DateName,
                            DateSaved = date.DateSaved,
                            OriginalDateSaved = date.OriginalDateSaved,
                            RepeatDate = date.RepeatDate,
                            Age = date.Age,
                            TrackAge = date.TrackAge
                        });

                        break;

                    // Do not Repeat, Track
                    case ( < 0, false, true):
                        date.Age = CalculateAge(date.OriginalDateSaved);

                        await _dbService.Update(new Date
                        {
                            Id = date.Id,
                            DateName = date.DateName,
                            DateSaved = date.DateSaved,
                            OriginalDateSaved = date.OriginalDateSaved,
                            RepeatDate = date.RepeatDate
                        });

                        break;

                    // Do not Repeat, Do not Track. Do nothing
                    case ( < 0, false, false):
                        break;

                    // Date has not come to pass. Do Nothing
                    case ( > 0, false, false):
                        break;

                    // Default error state
                    default:
                        Console.WriteLine("MainPage switch error");
                        break;
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

    private int CalculateAge(DateTime date)
    {
        int age;
        DateTime temp = new DateTime(currentDate.Year, date.Month, date.Day);

        if (currentDate.Date < temp.Date)
        {
            age = (currentDate.Year - date.Year) - 1;
        }
        else
        {
            age = (currentDate.Year - date.Year);
        }

            return age;
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