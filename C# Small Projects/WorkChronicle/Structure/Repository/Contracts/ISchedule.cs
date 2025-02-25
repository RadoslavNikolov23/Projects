namespace WorkChronicle.Core.Repository.Contracts
{
    public interface ISchedule<T> where T : class
    {
       // public IReadOnlyList<T> WorkSchedule { get; }
        public ObservableCollection<T> WorkSchedule { get; }

        void AddShift(T shift);

        bool RemoveShift(T shift);

        int IndexOfShift(T shift);

        int TotalShifts();

        public int TotalCompansatedShifts();

        int TotalWorkHours();
    }
}
