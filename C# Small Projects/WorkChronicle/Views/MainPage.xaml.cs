using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using WorkChronicle.Structure.Core;
using WorkChronicle.Structure.Core.Contracts;

namespace WorkChronicle
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCalculateShiftsClicked(object sender, EventArgs e)
        {
            IEngine engine = new Engine();


            DateTime startDate = StartDatePicker.Date;
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

            await Navigation.PushAsync(new SecondPage(startDate, cycle));
        }

    }

}
