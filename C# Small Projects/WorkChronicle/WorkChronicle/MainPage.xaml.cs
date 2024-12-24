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

            string[] cycle = cycleInput.Split('-');
            if (cycle.Length == 0)
            {
                ResultsLabel.Text = "Something went wrong";
                return;
            }

            List<string> shifts = CalcucateShifts(startDate, cycle);
            int totalHours = CalculateTotalHours(shifts);

            ResultsLabel.Text = $"Total hours: {totalHours}";
            ShiftListView.ItemsSource = shifts;

        }

        private List<string> CalcucateShifts(DateTime startDate, string[] cycle)
        {
            List<string> shifts = new List<string>();
            int cycleLenght=cycle.Length;
            int dayInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            DateTime currentDay= startDate;

            for (int i=0;i < dayInMonth; i++)
            {
                string shift = cycle[i% cycleLenght];

                if (shift != "Day" && shift != "Night")
                   continue;

                shifts.Add($"{currentDay:f}: {shift}");
                if (shift == "Night")
                {
                    currentDay = currentDay.AddDays(1);
                }
                currentDay = currentDay.AddDays(4);

                if(currentDay.Month != startDate.Month)
                {
                    break;
                }
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
                    totalHours += 13;
                }
            }
            return totalHours;
        }
    }

}
