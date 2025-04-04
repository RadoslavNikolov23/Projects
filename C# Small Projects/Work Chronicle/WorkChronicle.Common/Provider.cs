namespace WorkChronicle.Structure.Core
{
    public class Provider
    {
        public static KeyValuePair<int, string[]> GetMonthHoursTotal(DateTime startDate)
        {
            if (startDate.Year == 2024)
            {
                return WorkHoursByYear.Year2024(startDate.Month); 
            }
            else if (startDate.Year == 2025)
            {
                return WorkHoursByYear.Year2025(startDate.Month); 
            }

            return new KeyValuePair<int, string[]>();
        }


        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "Unknown";
            }
        }

    }
}
