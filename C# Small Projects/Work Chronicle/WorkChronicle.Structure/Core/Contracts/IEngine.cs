namespace WorkChronicle.Structure.Core.Contracts
{
    public interface IEngine<T> where T:class
    {
        Task <T> CalculateShifts (ScheduleConfiguration scheduleConfiguration);
    }
}
