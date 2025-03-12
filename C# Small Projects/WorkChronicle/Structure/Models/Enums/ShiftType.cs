namespace WorkChronicle.Structure.Models.Enums
{
    public enum ShiftType
    {
        DayShift = 0,
        NightShift = 1
    }

    public static class ShiftTypeExtensions
    {
        public static Task<bool> TryParseShiftType(this string shiftString, out ShiftType shiftType)
        {
            return Task.FromResult(Enum.TryParse(shiftString, out shiftType));
        }
    }
}
