using Microsoft.VisualBasic;

namespace WorkChronicle
{
    public partial class MainPage : ContentPage
    {
        private int currentMonth;
        private int currentYear;

        public MainPage()
        {
            InitializeComponent();
            currentMonth = DateTime.Now.Month;
            currentYear = DateTime.Now.Year;
            UpdateCalendar();
        }

        private void UpdateCalendar()
        {
            YearLabel1.Text = this.currentYear.ToString();
            MonthLabel1.Text = new DateTime(this.currentYear, this.currentMonth, 1).ToString("MMMM");
            BuildCalender(this.currentMonth, this.currentYear);
        }

        private void OnPreviousYearCliked(object sender, EventArgs e)
        {
            this.currentYear--;
            UpdateCalendar();
        }

        private void OnNextYearCliked(object sender, EventArgs e)
        {
            this.currentYear++;
            UpdateCalendar();
        }

        private void OnPreviuosMonth(object sender, EventArgs e)
        {
            if (this.currentMonth == 1)
            {
                this.currentMonth = 12;
                this.currentYear--;
            }
            else
            {
                this.currentMonth--;
            }
            UpdateCalendar();
        }

        private void OnNextMonth(object sender, EventArgs e)
        {
            if (this.currentMonth == 12)
            {
                this.currentMonth = 1;
                this.currentYear++;
            }
            else
            {
                this.currentMonth++;
            }
            UpdateCalendar();
            UpdateCalendar();
        }

        private void BuildCalender(int month, int year)
        {
            CalenderGrid.Children.Clear();
            CalenderGrid.RowDefinitions.Clear();
            CalenderGrid.ColumnDefinitions.Clear();

            for (int col = 0; col < 7; col++)
                CalenderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            string[] daysofWeek = { "Пон", "Втор", "Сряд", "Четв", "Пет", "Съб", "Нед", };

            for (int i = 0; i < daysofWeek.Length; i++)
            {
                var dayLabel = new Label
                {
                    Text = daysofWeek[i],
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                Grid.SetRow(dayLabel, 0);
                Grid.SetColumn(dayLabel, i);
                CalenderGrid.Children.Add(dayLabel);

                DateTime firstDate = new DateTime(year, month, 1);
                int dayInMonth = DateTime.DaysInMonth(year, month);
                int dayOfWeek = (int)firstDate.DayOfWeek;

                int totalRows = (int)Math.Ceiling((dayOfWeek + dayInMonth) / 7.0);
                for (int row = 1; row <= totalRows; row++)
                    CalenderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });


                int day = 1;
                for (int row = 1; row <= totalRows; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        if (row == 1 && col < dayOfWeek || day > dayInMonth)
                            continue;

                        Button dayButton = new Button
                        {
                            Text = day.ToString(),
                            FontSize = 14,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        };

                        dayButton.Clicked += (s, e) =>
                        {
                            SelectedDateLabel.Text = $"Selected Date: {new DateTime(year, month, int.Parse(dayButton.Text)):f}";
                        };

                        Grid.SetRow(dayButton, row);
                        Grid.SetColumn(dayButton, col);
                        CalenderGrid.Children.Add(dayButton);
                        day++;
                    }
                }
            }
        }

    }

}
