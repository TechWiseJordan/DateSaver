namespace DateSaver;

public partial class MainPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editDateId;

    public MainPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        Task.Run(async () => listView.ItemsSource = await _dbService.GetDates());
    }

    private async void newBtn_Clicked(object sender, EventArgs e)
    {

        // Go to create page
        //await Shell.Current.GoToAsync("CreatePage");


        if (_editDateId == 0)
        {
            // Add Customer

            await _dbService.Create(new Date
            {
                CustomerName = nameEntryField.Text,
                Email = emailEntryField.Text,
                Mobile = mobileEntryField.Text
            });
        }
        else
        {
            // Edit Customer

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

        listView.ItemsSource = await _dbService.GetDates();
    }

    private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var date = (Date)e.Item;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":

                _editDateId = date.Id;
                nameEntryField.Text = date.CustomerName;
                emailEntryField.Text = date.Email;
                mobileEntryField.Text = date.Mobile;

                break;
            case "Delete":

                await _dbService.Delete(date);
                listView.ItemsSource = await _dbService.GetDates();

                break;
        }
    }
}
