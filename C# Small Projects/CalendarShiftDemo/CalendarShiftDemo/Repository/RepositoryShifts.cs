using CalendarShiftDemo.Models.Contracts;
using CalendarShiftDemo.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarShiftDemo.Repository
{
    public class RepositoryShifts : IRepository<IShift>
    {
        private List<IShift> shifts;

        public RepositoryShifts()
        {
            this.shifts= new List<IShift>();
        }

        public IReadOnlyList<IShift> Shifts { get => this.shifts.AsReadOnly(); }

        public void AddShift(IShift shift)
        {
            this.shifts.Add(shift);
        }

        public bool RemoveShift(IShift shift)
        {
            return this.shifts.Remove(shift);
        }

        public int ShiftsWorder()
        {
            int allShifts = this.shifts.Count;

            return allShifts;
        }

        public int TimeWorked()
        {
            int allHours = this.shifts.Count;
            allHours *= 12;


            return allHours;
        }

    }
}
