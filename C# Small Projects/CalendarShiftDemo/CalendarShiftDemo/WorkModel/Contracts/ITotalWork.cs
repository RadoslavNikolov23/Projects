using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarShiftDemo.WorkModel.Contracts
{
    public interface ITotalWork
    {
        public abstract static int GetMonthDays(int monthNumber);

        public abstract static int GetMonthHours(int monthNumber);

        public abstract static int GetTotalYearDays();

        public abstract static int GetTotalYearHours();
    }
}
