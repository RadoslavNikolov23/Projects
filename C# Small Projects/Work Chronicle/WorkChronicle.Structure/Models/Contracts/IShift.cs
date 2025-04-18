﻿namespace WorkChronicle.Structure.Models.Contracts
{
    public interface IShift
    {
        public ShiftType ShiftType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public double StarTime { get; set; }
        public double ShiftHour { get; set; }
        public bool IsCompensated { get; set; }
        public bool IsCurrentMonth { get; set; }
        public Color BackgroundColor { get; set; }

        public DateTime GetDateShift();


    }
}
