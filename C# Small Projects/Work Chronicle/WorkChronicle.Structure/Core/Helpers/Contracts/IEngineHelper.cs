namespace WorkChronicle.Structure.Core.Helpers.Contracts
{
    public interface IEngineHelper<T> where T : ISchedule<IShift>
    {
        Task<T> AddShiftsToSchedule(ISchedule<IShift> schedule, ScheduleConfiguration sc, IShift dayShift, IShift? nightShift, int daysBetweenShiftsCycle);

        Task<T> GenerateMonthSchedule(ISchedule<IShift> schedule,ScheduleConfiguration sc);

        Task<ISchedule<IShift>> GenerateBlankCalendar(ISchedule<IShift> schedule);
    }
}
