using Microsoft.Maui.Controls;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Structure.Core;
using WorkChronicle.Structure.Core.Contracts;
using WorkChronicle.Structure.Database;
using WorkChronicle.Structure.Models;
using WorkChronicle.Core.Repository;
using System.ComponentModel;
using WorkChronicle;


namespace WorkChronicle
{
    public partial class MainPage : INotifyPropertyChanged
    {
        private readonly Schedule schedule;
        private readonly IEngine engine;


        public MainPage(Schedule schedule, IEngine engine)
        {
            InitializeComponent();
            BindingContext = this;
            this.schedule = schedule;
            this.engine = engine;
        }


        private async void ViewSavedSchedules(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScheduleView(this.schedule));
            //await Shell.Current.GoToAsync($"ScheduleView");
        }

        private async void PickNewSchedule(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PickerDateView(this.schedule,engine));
          //  await Shell.Current.GoToAsync($"PickerDateView");

        }

        private void ExitApp(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }


    }

}
