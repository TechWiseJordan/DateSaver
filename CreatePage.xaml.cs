using Microsoft.VisualBasic;

namespace DateSaver;

public partial class CreatePage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editDateId = 0;
    private DateTime _editDateSaved = DateTime.Today.Date;

    public CreatePage(LocalDbService dbService) // Default constuctor which should NOT be called
    {
        InitializeComponent();
        _dbService = dbService;
    }
    
    public CreatePage(LocalDbService dbService, int id)
    {
        InitializeComponent();
        _dbService = dbService;

        _editDateId = id;
    }
    
    public CreatePage(LocalDbService dbService, int id, ItemTappedEventArgs e)
    {
        InitializeComponent();
        _dbService = dbService;

        var editDate = (Date)e.Item;

        _editDateId = editDate.Id;
        nameEntryField.Text = editDate.DateName;
        dateEntryField.Date = editDate.DateSaved;
        repeatCheckBox.IsChecked = editDate.RepeatDate;
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
                DateSaved = _editDateSaved, 
                RepeatDate = repeatCheckBox.IsChecked               
            });
        }
        else
        {
            // Edit Date

            await _dbService.Update(new Date
            {
                Id = _editDateId,
                DateName = nameEntryField.Text,
                DateSaved = _editDateSaved,
                RepeatDate = repeatCheckBox.IsChecked
            });

            _editDateId = 0;
        }
        await Navigation.PushModalAsync(new MainPage(_dbService));
    }

    private async void deleteBtn_Clicked(object sender, EventArgs e)
    {
        if (_editDateId == 0) // Go home
        {
            await Navigation.PushModalAsync(new MainPage(_dbService));
        }
        else // Delete then go home
        {
            Date deleteDate = await _dbService.GetById(_editDateId);

            await _dbService.Delete(deleteDate);

            await Navigation.PushModalAsync(new MainPage(_dbService));
        }
    }
}