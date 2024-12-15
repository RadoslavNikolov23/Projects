using CalendarShiftDemo.Core.Contacts;
using CalendarShiftDemo.IO;
using CalendarShiftDemo.IO.Contacts;
using CalendarShiftDemo.Models;
using CalendarShiftDemo.Models.Contracts;
using CalendarShiftDemo.Repository;
using CalendarShiftDemo.Repository.Contracts;
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

            string inputDate = string.Empty;
            string inputMonth = string.Empty;


            writer.WriteLine("Въведи ден и месец, в който си дневна смяна: ");
            writer.Write("Ден: ");
            inputDate = reader.ReadLine();
            writer.Write("Месец: ");
            inputMonth = reader.ReadLine();

            IRepository<IShift> shifts= new RepositoryShifts();


            DateTime workDayShigt = new DateTime(2024, int.Parse(inputMonth), int.Parse(inputDate), 07, 00, 00);
            DateTime workNightShigt = new DateTime(2024, int.Parse(inputMonth), (int.Parse(inputDate)) + 1, 19, 00, 00);

            DayShift dayShift = new DayShift(workDayShigt);
            NightShift nightShift = new NightShift(workNightShigt);

            shifts.AddShift(dayShift);
            shifts.AddShift(nightShift);



            while (true)
            {

                workDayShigt = workDayShigt.AddDays(4);
                workNightShigt = workNightShigt.AddDays(4);

                if (workDayShigt.Year == 2025 || workNightShigt.Year == 2025) break;

                shifts.AddShift(new DayShift(workDayShigt));
                shifts.AddShift(new NightShift(workNightShigt));
            }

            int count = 1;
            foreach(IShift shift in shifts.Shifts)
            {
                writer.WriteLine(shift.ToString());

                if (count++ % 2 == 0)
                    writer.WriteLine("\n");
                
            }

            int allShift = shifts.ShiftsWorder();
            int allHours = shifts.TimeWorked();

            writer.WriteLine($"Работил си общо {allShift} смени и общо {allHours} часове");



        }
    }
}
