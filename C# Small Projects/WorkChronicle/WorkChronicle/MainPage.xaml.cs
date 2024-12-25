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



        private void OnCalculateShiftsClicked(object sender, EventArgs e)
        {
            IEngine engine = new Engine();


            DateTime startDate = StartDatePicker.Date;
            string cycleInput = CycleEntry.Text;
            if (string.IsNullOrEmpty(cycleInput))
            {
                ResultsLabel.Text = "Please enter a valid date";
                return;
            }

            string[] cycle = cycleInput.Split('-');
            if (cycle.Length == 0)
            {
                ResultsLabel.Text = "Something went wrong";
                return;
            }

            

            List<string> shifts = engine.CalculateShifts(startDate, cycle);
            int totalHours = engine.CalculateTotalHours(shifts);

            ResultsLabel.Text = $"Total hours: {totalHours}";
            ShiftListView.ItemsSource = shifts;


        }

        
    }

}
