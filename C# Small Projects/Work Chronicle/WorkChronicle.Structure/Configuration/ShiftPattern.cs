namespace WorkChronicle.Structure.Configuration
{
    public class ShiftPattern
    {
        public bool Is24Dayshift { get; set; }
        public bool IsDayDayShift { get; set; }
        public bool IsDayNightShift { get; set; }
        public bool IsDayNightNightShift { get; set; }

        public ShiftPattern()
        {
            Is24Dayshift = false;
            IsDayDayShift = false;
            IsDayNightShift = false;
            IsDayNightNightShift = false;
        }

        public Task ChechTheShiftPattern(string[] cycle)
        {
            if (this.Is24Dayshift = cycle.Length == 1)
                return Task.CompletedTask;

            if (this.IsDayDayShift = cycle.Length == 2 && cycle[1] == "Day")
                return Task.CompletedTask;

            if (this.IsDayNightShift = cycle.Length == 2 && cycle[1] == "Night")
                return Task.CompletedTask;

            if (this.IsDayNightNightShift = cycle.Length == 3)
                return Task.CompletedTask;

            return Task.CompletedTask;
        }
    }
}
