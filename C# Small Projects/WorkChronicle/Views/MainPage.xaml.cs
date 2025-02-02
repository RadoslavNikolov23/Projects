using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Structure.Core;
using WorkChronicle.Structure.Core.Contracts;

namespace WorkChronicle
{
    public partial class MainPage : ContentPage
    {
        public ISchedule<IShift> schedule { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCalculateShiftsClicked(object sender, EventArgs e)
        {
            DateTime startDate = StartDatePicker.Date;

            if (WorkSchedulePicker.SelectedIndex==-1)
            {
                ResultsLabel.Text = "Please select a work schedule first.";
                return;
            }

            string selectedSchedule= WorkSchedulePicker.Items[WorkSchedulePicker.SelectedIndex];

            if (string.IsNullOrEmpty(selectedSchedule))
            {
                ResultsLabel.Text = "Please select a work schedule first.";
                return;
            }

            string[] cycle = selectedSchedule.Split('-');
            if (cycle.Length == 0)
            {
                ResultsLabel.Text = "Something went wrong";
                return;
            }

            IEngine engine = new Engine();
            this.schedule = engine.CalculateShifts(startDate, cycle);
            ObservableCollection<IShift> CompensatedShifts = new ObservableCollection<IShift>();
            await Navigation.PushAsync(new SecondPage(startDate, schedule, CompensatedShifts));
        }

        private async void OnLoadScheduleClicked(object sender, EventArgs e)
        {
            string savedSchedule=Preferences.Get("WorkSchedule", string.Empty);

            if(!string.IsNullOrEmpty(savedSchedule))
            {
                var loadedShift = JsonSerializer.Deserialize<List<IShift>>(savedSchedule);   ///TODO see this why it does not WORK!!!!!!!!!!!!!

                foreach (var shift in loadedShift)
                {
                    this.schedule.AddShift(shift);
                }


                ObservableCollection<IShift> CompensatedShifts = new ObservableCollection<IShift>();

                await Navigation.PushAsync(new SecondPage(DateTime.Now, this.schedule, CompensatedShifts));
            }
            else
            {
                ResultsLabel.Text = "No saved schedule found.";
                return; //With or Without this line, the code will work the same way.
            }

        }

        private async void OnExitButtonClicked(object sender, EventArgs e)
        {
            Application.Current.Quit();

        }
    }

}
