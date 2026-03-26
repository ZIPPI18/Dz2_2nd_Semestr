using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dz2_2nd_Sem
{
    internal class Journal<T> where T : IJournalEntry
    {
        private List<T> entries = new List<T>();
        public void Add(T entry)
        {
            entries.Add(entry);
        }
        public List<T> GetAll()
        {
            return entries;
        }

    }
}
