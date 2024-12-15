using CalendarShiftDemo.Core.Contacts;
using CalendarShiftDemo.IO;
using CalendarShiftDemo.IO.Contacts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarShiftDemo.Core
{
    public class Engine : IEngine
    {
        private IReader reader;
        private IWriter writer;
        public Engine()
        {
            reader = new Reader();
            writer = new Writer();
        }
        public void Run()
        {
         

            //List<DateTime> yearDate = new List<DateTime>();
            //DateTime dateTime = new DateTime(2025,01,01);

            //do
            //{
            //    yearDate.Add(dateTime);
            //    dateTime=dateTime.AddDays(1);
            //} while (dateTime.Year == 2025);


            string inputDate = string.Empty;
            string inputMonth= string.Empty;
            writer.WriteLine("Въведи ден и месец, в който си дневна смяна: ");
            writer.Write("Ден: ");
            inputDate = reader.ReadLine();    
            writer.Write("Месец: ");
            inputMonth = reader.ReadLine();
            DateTime workDayShigt = new DateTime(2025, int.Parse(inputMonth), int.Parse(inputDate));
            DateTime workNightShigt = new DateTime(2025, int.Parse(inputMonth), (int.Parse(inputDate))+1);

            List<string> workDays= new List<string>();

            while(workDayShigt.Year==2025 && workNightShigt.Year == 2025)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Работиш дневни смени на: {workDayShigt}");
                sb.AppendLine($"Работиш нощни смени на: {workNightShigt}");
                sb.AppendLine();
                workDays.Add(sb.ToString());
                workDayShigt = workDayShigt.AddDays(4);
                workNightShigt = workNightShigt.AddDays(4);
            }

            writer.WriteLine(string.Join("", workDays.ToArray()));

        }
    }
}
