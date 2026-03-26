using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dz2_2nd_Sem
{
    internal class PlacedEvent : IJournalEntry
    {
        public string Shelf { get; set; }
        public int Slot { get; set; }
        public string ItemName { get; set; }

        public PlacedEvent(string shelf, int slot, string itemName)
        {
            this.Shelf = shelf;
            this.Slot = slot;
            this.ItemName = itemName;
        }

        public string ToLogLine()
        {
            return $"PLACED|{Shelf}|{Slot}|{ItemName}";
        }

        public string ToScreenLine()
        {
            return $"[Размещение] '{ItemName}' на полку {Shelf} в слот {Slot}";
        }

        static public PlacedEvent FromLogLine(string line)
        {
            string[] p = line.Split('|');
            return new PlacedEvent(p[1], int.Parse(p[2]), p[3]);
        }
    }
}
