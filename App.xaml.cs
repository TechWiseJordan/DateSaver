namespace DateSaver
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            //CreatePage = new NavigationPage(new CreatePage());
        }
    }
}
