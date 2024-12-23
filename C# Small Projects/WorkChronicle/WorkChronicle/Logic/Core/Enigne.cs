
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChronicle.Logic.Core.Contacts;
using WorkChronicle.Logic.IO;
using WorkChronicle.Logic.IO.Contacts;
using WorkChronicle.Logic.Models;
using WorkChronicle.Logic.Models.Contacts;
using WorkChronicle.Logic.Repository;
using WorkChronicle.Logic.Repository.Contracts;
using WorkChronicle.Logic.WorkModel;

namespace WorkChronicle.Logic.Core
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
        public void Run(DateTime datePicked)
        {

            Dictionary<int, IRepository<IShift>> shiftByMonts = new Dictionary<int, IRepository<IShift>>();
            for (int i = 0; i < 12; i++)
                shiftByMonts[i + 1] = new RepositoryShifts();

            int inputDate = datePicked.Day;
            int inputMonth = datePicked.Month;


            DateTime inputDayShift = new DateTime(2024, inputMonth, inputDate, 07, 00, 00);
            DateTime inputNightShift = new DateTime(2024, inputMonth, inputDate + 1, 19, 00, 00);


            while (true)
            {
                IShift shiftDay = new DayShift(inputDayShift);
                IShift shiftNIght = new NightShift(inputNightShift);
                shiftByMonts[inputDayShift.Month].AddShift(shiftDay);
                shiftByMonts[inputDayShift.Month].AddShift(shiftNIght);

                inputDayShift = inputDayShift.AddDays(4);
                inputNightShift = inputNightShift.AddDays(4);
                if (inputDayShift.Year != 2024 || inputNightShift.Year != 2024) break;

            }

            inputDayShift = new DateTime(2024, inputMonth, inputDate, 07, 00, 00);
            inputNightShift = new DateTime(2024, inputMonth, inputDate, 19, 00, 00);


            while (true)
            {
                inputDayShift = inputDayShift.AddDays(-4);
                inputNightShift = inputNightShift.AddDays(-3);

                if (inputDayShift.Year != 2024 || inputNightShift.Year != 2024) break;
                IShift shiftDay = new DayShift(inputDayShift);
                IShift shiftNIght = new NightShift(inputNightShift);
                shiftByMonts[inputDayShift.Month].AddShift(shiftDay);
                shiftByMonts[inputDayShift.Month].AddShift(shiftNIght);


            }

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, IRepository<IShift>> shifts in shiftByMonts)
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
                int totalHours = TotalWork.GetMonthHours(shifts.Key);
                sb.AppendLine($"През месец {month} общия брои работни дни е {totalShift} и {totalHours} часове");

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
                case 10: return "Октомври";
                case 11: return "Ноември";
                case 12: return "Декември";
                default: return null;
            }
        }
    }
}
