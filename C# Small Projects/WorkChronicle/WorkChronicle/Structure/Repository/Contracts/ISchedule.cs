using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Core.Repository.Contracts
{
    public interface ISchedule<T> where T : class
    {
        public IReadOnlyCollection<T> WorkSchedule { get; }

        void AddShift(T shift);

        bool RemoveShift(int year, int month, int day);

        int TotalShifts();

        int TotalWorkHours();
    }
}
