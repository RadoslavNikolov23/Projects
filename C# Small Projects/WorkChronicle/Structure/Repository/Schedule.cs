using System.Diagnostics;

namespace WorkChronicle.Core.Repository
{
    public class Schedule : ISchedule<IShift>
    {
        private ObservableCollection<IShift> workSchedule;

        public Schedule()
        {
            workSchedule= new ObservableCollection<IShift>();
        }

        public ObservableCollection<IShift> WorkSchedule { get => workSchedule; }

        public Task AddShift(IShift shift)
        {
            this.workSchedule.Add(shift);
            return Task.CompletedTask;
        }

        public Task RemoveShift(IShift shiftToRemove)
        {
            this.workSchedule.Remove(shiftToRemove);
            return Task.CompletedTask;
        }

        public Task<int> IndexOfShift(IShift shift)
        {
            return Task.FromResult(this.workSchedule.IndexOf(shift));
        }

        public Task<int> TotalShifts()
        {
            return Task.FromResult(this.workSchedule.Count);
        }

        public Task<int> TotalCompansatedShifts()
        {
            return Task.FromResult(this.workSchedule.Count(s => s.IsCompensated == true));
        }

        public Task<int> TotalWorkHours()
        {
            int totalHours = 0;

            foreach (var shift in workSchedule)
            {
                if(shift.IsCompensated == false)
                {
                    totalHours += shift.Hour;
                }
            }

            return Task.FromResult(totalHours);
        }

    }
}
