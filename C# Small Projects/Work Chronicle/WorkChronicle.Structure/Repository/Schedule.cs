namespace WorkChronicle.Structure.Repository
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
            workSchedule.Add(shift);
            return Task.CompletedTask;
        }

        public Task RemoveShift(IShift shiftToRemove)
        {
            workSchedule.Remove(shiftToRemove);
            return Task.CompletedTask;
        }

        public Task<int> IndexOfShift(IShift shift)
        {
            return Task.FromResult(workSchedule.IndexOf(shift));
        }

        public Task<int> TotalShifts()
        {
            return Task.FromResult(workSchedule.Count);
        }

        public Task<int> TotalCompensatedShifts()
        {
            return Task.FromResult(workSchedule.Count(s => s.IsCompensated == true));
        }

        public async Task<int> CalculateTotalWorkHours()
        {
            int totalHours = 0;

            foreach (var shift in workSchedule.Where(s=>s.ShiftType!=ShiftType.RestDay))
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

            double startHour = shift.StarTime;
            double endHour = (startHour + (int)shift.ShiftHour) % 24;

            if (startHour < endHour)
            {
                for (int i = (int)startHour; i < startHour + shift.ShiftHour; i++)
                {
                    int currentHour = i % 24;

                    if (currentHour >= 22 || currentHour < 6)
                    {
                        totalShiftHours += GetNightShiftMultiplier(currentHour);
                    }
                }
            }
            else
            {
                for (int i = (int)startHour; i < 24; i++)
                {
                    if (i >= 22 || i < 6)
                    {
                        totalShiftHours += GetNightShiftMultiplier(i);
                    }
                }

                for (int i = 0; i < endHour; i++)
                {
                    totalShiftHours += GetNightShiftMultiplier(i);
                }
            }
            return Task.FromResult((int)totalShiftHours);
        }

        private double GetNightShiftMultiplier(int currentHour)
        {
            return currentHour >= 22 || currentHour < 6 ? 0.143 : 0.0; //Make the 0.143 a const
        }

    }
}
