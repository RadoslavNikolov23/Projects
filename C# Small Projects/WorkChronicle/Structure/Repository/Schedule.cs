using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChronicle.Core.Models;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository.Contracts;

namespace WorkChronicle.Core.Repository
{
    public class Schedule : ISchedule<IShift>
    {
        private ObservableCollection<IShift> workSchedule;

        public Schedule()
        {
            this.workSchedule = new ObservableCollection<IShift>();
        }
        public IReadOnlyCollection<IShift> WorkSchedule { get => this.workSchedule.AsReadOnly(); }

        public void AddShift(IShift shift)
        {
            this.workSchedule.Add(shift);
        }

        public bool RemoveShift(IShift shiftToRemove)
        {
            return this.workSchedule.Remove(shiftToRemove);
        }

        public int IndexOfShift(IShift shift)
        {
           return this.workSchedule.IndexOf(shift);
        }

        public int TotalShifts()
        {
            return this.workSchedule.Count;
        }

        public int TotalWorkHours()
        {
            int totalHours= this.workSchedule.Sum(s => s.Hour);
            return totalHours;
        }

        public void Sort()
        {
            List<IShift> sortedList = this.workSchedule.OrderBy(s => s.WorkShift.Year)
                .ThenBy(s=>s.WorkShift.Month)
                .ThenBy(s=>s.WorkShift.Day)
                .ThenBy(s=>s.WorkShift.Hour).ToList();
            this.workSchedule = new ObservableCollection<IShift>(sortedList);
        }
    }
}
