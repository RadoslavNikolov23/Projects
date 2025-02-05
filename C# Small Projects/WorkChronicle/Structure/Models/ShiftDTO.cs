using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Structure.Models
{
    public class ShiftDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public string ShiftType { get; set; }
        public bool IsCompensated { get; set; }
    }
}
