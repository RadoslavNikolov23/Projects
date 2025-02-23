namespace WorkChronicle.Core.Repository
{
    public class Schedule : ISchedule<IShift>
    {
        private ObservableCollection<IShift> workSchedule;

        public Schedule()
        {
            this.workSchedule = new ObservableCollection<IShift>();
        }

        public ReadOnlyObservableCollection<IShift> WorkSchedule { get => new ReadOnlyObservableCollection<IShift>(this.workSchedule); }

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

        public int TotalCompansatedShifts()
        {
            return this.workSchedule.Count(s => s.isCompensated == true);
        }

        public int TotalWorkHours()
        {
            int totalHours = 0;

            foreach (var shift in workSchedule)
            {
                if(shift.isCompensated == false)
                {
                    totalHours += shift.Hour;
                }
            }

            return totalHours;
        }

    }
}
