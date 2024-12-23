using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Logic.Repository.Contracts
{
    public interface IRepository<T> where T : class
    {
        public IReadOnlyList<T> Shifts { get; }

        public void AddShift(T shift);

        public bool RemoveShift(T shift);

        public int ShiftsWorder();

        public int TimeWorked();
    }
}
