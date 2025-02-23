namespace WorkChronicle.Structure.Core.Contracts
{
    public interface IEngine
    {
        Schedule CalculateShifts(DateTime startDate, string[] cycle);

        int CalculateTotalHours(Schedule schedule);
    }
}
