using Microsoft.Extensions.DependencyInjection;

namespace WorkChronicle
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
