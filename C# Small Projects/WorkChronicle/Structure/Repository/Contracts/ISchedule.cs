namespace WorkChronicle.Core.Repository.Contracts
{
    public interface ISchedule<T> where T : class
    {
       // public IReadOnlyList<T> WorkSchedule { get; }
        public ObservableCollection<T> WorkSchedule { get; }

        Task AddShift(T shift);

        Task RemoveShift(T shift);

        Task<int> IndexOfShift(T shift);

        Task<int> TotalShifts();

        Task<int> TotalCompansatedShifts();

        Task<int> TotalWorkHours();
    }
}
