namespace DateSaver
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("CreatePage", typeof(CreatePage));
        }
    }
}
