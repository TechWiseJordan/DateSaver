using Microsoft.VisualBasic;

namespace DateSaver;

public partial class CreatePage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editDateId;
    // This is a test comment delete me

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

        _editDateId = id;
        var editDate = (Date)e.Item;

        if (_editDateId != 0)
        {
            _editDateId = editDate.Id;
            nameEntryField.Text = editDate.CustomerName;
            emailEntryField.Text = editDate.Email;
            mobileEntryField.Text = editDate.Mobile;
        }
    }

    private async void saveBtn_Clicked(object sender, EventArgs e)
    {
        if (_editDateId == 0)
        {
            // Add Date

            await _dbService.Create(new Date
            {
                CustomerName = nameEntryField.Text,
                Email = emailEntryField.Text,
                Mobile = mobileEntryField.Text
            });
        }
        else
        {
            // Edit Date

            await _dbService.Update(new Date
            {
                Id = _editDateId,
                CustomerName = nameEntryField.Text,
                Email = emailEntryField.Text,
                Mobile = mobileEntryField.Text
            });

            _editDateId = 0;
        }

        nameEntryField.Text = string.Empty;
        emailEntryField.Text = string.Empty;
        mobileEntryField.Text = string.Empty;

        //listView.ItemsSource = await _dbService.GetDates();
    }
}