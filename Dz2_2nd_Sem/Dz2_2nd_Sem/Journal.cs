using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public void SaveToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (var entry in entries)
                {
                    writer.WriteLine(entry.ToLogLine());
                }
            }
        }
    }
}
