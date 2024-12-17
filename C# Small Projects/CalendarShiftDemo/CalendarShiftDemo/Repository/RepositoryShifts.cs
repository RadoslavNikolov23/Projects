using CalendarShiftDemo.Models;
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

            List<IShift> dayShifts=this.shifts.Where(s=>s.GetType().Name==nameof(DayShift)).ToList();
            List<IShift> nightShifts=this.shifts.Where(s=>s.GetType().Name==nameof(NightShift)).ToList();

            int dayShiftHours = dayShifts.Count*12;
            int nightShiftHours = nightShifts.Count*13;

            return dayShiftHours+nightShiftHours;
        }

    }
}
