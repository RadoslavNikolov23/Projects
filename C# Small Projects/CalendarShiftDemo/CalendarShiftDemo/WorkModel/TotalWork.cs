using CalendarShiftDemo.WorkModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarShiftDemo.WorkModel
{
    public class TotalWork:ITotalWork
    {
        public TotalWork()
        {
            
        }
        public static int GetMonthDays(int monthNumber)
        {
            string[] month = GetRightMonth(monthNumber);

            return int.Parse(month[1]);
        }

        public static int GetMonthHours(int monthNumber)
        {
            string[] month = GetRightMonth(monthNumber);

            return int.Parse(month[2]);
        }

        public static int GetTotalYearDays()
        {
            string[] month = GetRightMonth(13);

            return int.Parse(month[2]);
        }
        public static int GetTotalYearHours()
        {
            string[] month = GetRightMonth(13);

            return int.Parse(month[3]);
        }

        private static string[] GetRightMonth(int monthNumber)
        {
            string path = @"../../../" + @"/WorkHours/WorkTime2024EN.txt";
            string[] totalMonts = File.ReadAllLines(path);

            return totalMonts[monthNumber].Split("/");
        }
    }
}
