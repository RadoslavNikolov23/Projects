namespace WorkChronicle.Structure.Core.Contracts
{
    public interface IEngine<T> where T:class
    {
        Task <T>  CalculateShifts(DateTime startDate, string[] cycle, string firstShift);

        int CalculateTotalHours(T schedule);
    }
}
