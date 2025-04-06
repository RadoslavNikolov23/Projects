namespace WorkChronicle.Structure.Core.Schedules.Contracts
{
    public interface IEngineStrategy
    {
        Task ApplySchedule(ISchedule<IShift> schedule, ScheduleConfiguration sc, bool isCurrentMonth);
    }
}
