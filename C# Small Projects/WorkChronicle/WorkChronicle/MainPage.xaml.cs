using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

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
            DateTime startDate = StartDatePicker.Date;
            string cycleInput = CycleEntry.Text;

            if (string.IsNullOrEmpty(cycleInput))
            {
                ResultsLabel.Text = "Please enter a valid date";
                return;
            }

            var cycle = cycleInput.Split('-');
            if (cycle.Length == 0)
            {
                ResultsLabel.Text = "Cycle pattern is empty";
                return;
            }

            var shifts = CalcucateShifts(startDate, cycle);
            int totalHours = CalculateTotalHours(shifts);

            ResultsLabel.Text = $"Total hours: {totalHours}";
            ShiftListView.ItemsSource = shifts;

        }

        private List<string> CalcucateShifts(DateTime startDate, string[] cycle)
        {
            List<string> shifts = new List<string>();
            int cycleLenght=cycle.Length;
            int dayInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            for (int i = 0; i < dayInMonth; i++)
            {
                DateTime currentDay = startDate.AddDays(i);
                var shift = cycle[i % cycleLenght];

                if(shift=="off")
                    continue;
                shifts.Add($"{currentDay:f}: {shift}");
            }

            return shifts;
        }

        private int CalculateTotalHours(List<string> shifts)
        {
            int totalHours = 0;
            foreach (var shift in shifts)
            {
                if (shift.Contains("Day"))
                {
                    totalHours += 12;
                }
                else if (shift.Contains("Night"))
                {
                    totalHours += 12;
                }
            }
            return totalHours;
        }
    }

}
