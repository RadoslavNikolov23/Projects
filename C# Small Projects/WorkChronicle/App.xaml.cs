using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Structure.Database;
using WorkChronicle.Structure.Models;

namespace WorkChronicle
{
    public partial class App : Application
    {

        public App(AppShell appShell)
        {
            InitializeComponent();
            MainPage = appShell;

        }


    }
}
