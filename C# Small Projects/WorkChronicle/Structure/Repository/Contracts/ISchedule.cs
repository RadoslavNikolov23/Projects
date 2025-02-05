using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Core.Repository.Contracts
{
    public interface ISchedule<T> where T : class
    {
        public ObservableCollection<T> WorkSchedule { get; set; }

        void AddShift(T shift);

        bool RemoveShift(T shift);

        int IndexOfShift(T shift);

        int TotalShifts();

        public void Sort();

        int TotalWorkHours();
    }
}
