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

        public async Task<int> CalculateTotalWorkHours()
        {
            int totalHours = 0;

            foreach (var shift in workSchedule)
            {
                if (shift.IsCompensated == false)
                {
                    totalHours += await CalculteShiftHours(shift);
                }
            }

            return totalHours;
        }

        private Task<int> CalculteShiftHours(IShift shift)
        {
            double totalShiftHours = shift.ShiftHour;

            if (shift.ShiftType == ShiftType.NightShift)
            {
                double startHour = shift.StarTime;
                double endHour = (startHour + (int)shift.ShiftHour) % 24;

                for (int i = (int)startHour; i < startHour + shift.ShiftHour; i++)
                {
                    int currentHour = i % 24;

                    if (currentHour >= 22 || currentHour < 6)
                    {
                        totalShiftHours += 0.143; //Make this an constatnt the formula is 8/7
                    }

                }
            }

            return Task.FromResult((int)totalShiftHours);

        }

    }
}
