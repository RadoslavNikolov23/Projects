namespace WorkChronicle.Structure.Core.Main.Contracts
{
    public interface IEngine<T> where T:class
    {
        Task <T> CalculateShifts (ScheduleConfiguration scheduleConfiguration);
        Task<ISchedule<IShift>> BlankCalendar();

    }
}
