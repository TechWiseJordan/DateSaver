using Microsoft.VisualBasic;

namespace DateSaver;

public partial class CreatePage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editDateId;
    private DateTime _editDateSaved;

    public CreatePage(LocalDbService dbService) // Default constuctor which should NOT be called
    {
        InitializeComponent();
        _dbService = dbService;
        //Task.Run(async () => listView.ItemsSource = await _dbService.GetDates());
    }
    
    public CreatePage(LocalDbService dbService, int id)
    {
        InitializeComponent();
        _dbService = dbService;
        //Task.Run(async () => listView.ItemsSource = await _dbService.GetDates());

        _editDateId = id;

        if (_editDateId != 0)
        {

        }
    }
    
    public CreatePage(LocalDbService dbService, int id, ItemTappedEventArgs e)
    {
        InitializeComponent();
        _dbService = dbService;

        //_editDateId = id;
        var editDate = (Date)e.Item;

        /*
        if (_editDateId != 0)
        {
            _editDateId = editDate.Id;
            nameEntryField.Text = editDate.CustomerName;
            emailEntryField.Text = editDate.Email;
            mobileEntryField.Text = editDate.Mobile;
        }
        */

        _editDateId = editDate.Id;
        nameEntryField.Text = editDate.DateName;
        descriptionEntryField.Text = editDate.Description;
        dateEntryField.Date = editDate.DateSaved;
    }

    private void DateSelected(object sender, DateChangedEventArgs e)
    {
        _editDateSaved = dateEntryField.Date;
    }

    private async void saveBtn_Clicked(object sender, EventArgs e)
    {
        if (_editDateId == 0)
        {
            // Add Date

            await _dbService.Create(new Date
            {
                DateName = nameEntryField.Text,
                Description = descriptionEntryField.Text,
                DateSaved = _editDateSaved
            });
        }
        else
        {
            // Edit Date

            await _dbService.Update(new Date
            {
                Id = _editDateId,
                DateName = nameEntryField.Text,
                Description = descriptionEntryField.Text,
                DateSaved = _editDateSaved
            });

            _editDateId = 0;
        }

        nameEntryField.Text = string.Empty;
        descriptionEntryField.Text = string.Empty;
        //dateEntryField.Date = string.Empty;

        await Navigation.PushModalAsync(new MainPage(_dbService));

        //listView.ItemsSource = await _dbService.GetDates();
    }
}