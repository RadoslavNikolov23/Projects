namespace WorkChronicle.Structure.Repository.Contracts
{
    public interface ISchedule<T> where T : class
    {
        public ObservableCollection<T> WorkSchedule { get; }

        Task AddShift(T shift);

        Task RemoveShift(T shift);

        Task<int> IndexOfShift(T shift);

        Task<int> TotalShifts();

        Task<int> TotalCompensatedShifts();

        Task<int> CalculateTotalWorkHours();
    }
}
