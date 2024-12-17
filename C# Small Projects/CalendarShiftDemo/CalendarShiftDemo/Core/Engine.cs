using CalendarShiftDemo.Core.Contacts;
using CalendarShiftDemo.IO;
using CalendarShiftDemo.IO.Contacts;
using CalendarShiftDemo.Models;
using CalendarShiftDemo.Models.Contracts;
using CalendarShiftDemo.Repository;
using CalendarShiftDemo.Repository.Contracts;
using CalendarShiftDemo.WorkModel;
using CalendarShiftDemo.WorkModel.Contracts;
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

            Dictionary<int, IRepository<IShift>> shiftByMonts = new Dictionary<int, IRepository<IShift>>();
            for (int i = 0; i < 12; i++)
                shiftByMonts[i + 1] = new RepositoryShifts();

            writer.WriteLine("Въведи ден и месец, в който си дневна смяна: ");
            writer.Write("Ден: ");
            int inputDate = int.Parse(reader.ReadLine());
            writer.Write("Месец: ");
            int inputMonth = int.Parse(reader.ReadLine());

            DateTime inputDayShift = new DateTime(2024, inputMonth, inputDate, 07, 00, 00);
            DateTime inputNightShift = new DateTime(2024, inputMonth, inputDate+1, 19, 00, 00);

            while (inputDayShift.Year==2024 || inputNightShift.Year==2024)
            {
                IShift shiftDay = new DayShift(inputDayShift);
                IShift shiftNIght = new NightShift(inputNightShift);
                shiftByMonts[inputDayShift.Month].AddShift(shiftDay);
                shiftByMonts[inputDayShift.Month].AddShift(shiftNIght);

                inputDayShift = inputDayShift.AddDays(4);
                inputNightShift = inputNightShift.AddDays(4);
            }

            inputDayShift = new DateTime(2024, inputMonth, inputDate, 07, 00, 00);
            inputNightShift = new DateTime(2024, inputMonth, inputDate +1, 19, 00, 00);
            inputDayShift = inputDayShift.AddDays(-4);
            inputNightShift = inputNightShift.AddDays(-4);

            while (inputDayShift.Year == 2024 || inputNightShift.Year == 2024)
            {
                inputDayShift = inputDayShift.AddDays(-4);
                inputNightShift = inputNightShift.AddDays(-4);
                IShift shiftDay = new DayShift(inputDayShift);
                IShift shiftNIght = new NightShift(inputNightShift);
                shiftByMonts[inputDayShift.Month].AddShift(shiftDay);
                shiftByMonts[inputDayShift.Month].AddShift(shiftNIght);

              
            }

            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<int, IRepository<IShift>> shifts in shiftByMonts)
            {
                int count = 1;
                foreach (IShift shift in shifts.Value.Shifts)
                {
                    sb.AppendLine(shift.ToString());

                    if (count++ % 2 == 0)
                        sb.AppendLine();
                }
            
                int allShift = shifts.Value.ShiftsWorder();
                int allHours = shifts.Value.TimeWorked();
                string month = MonthName(shifts.Key);
                sb.AppendLine($"През месец {month} сте работили общо {allShift} дни и {allHours} часове");
              

                int totalShift = TotalWork.GetMonthDays(shifts.Key);
                int totalHours=TotalWork.GetMonthHours(shifts.Key);
                sb.AppendLine($"През месец {month} общия брои работни дни е {allShift} и {allHours} часове");

                sb.AppendLine();
            }
            writer.WriteText(sb.ToString());



        }

        private string MonthName(int key)
        {
            switch (key)
            {
                case 1: return "Януари";
                case 2: return "Февруари";
                case 3: return "Март";
                case 4: return "Април";
                case 5: return "Май";
                case 6: return "Юни";
                case 7: return "Юли";
                case 8: return "Август";
                case 9: return "Септември";
                case 10: return "Октомви";
                case 11: return "Ноември";
                case 12: return "Декември";
                default: return null;
            }
        }
    }
}
