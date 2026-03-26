using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dz2_2nd_Sem
{
    internal class TakenEvent : IJournalEntry
    {
        public string Shelf { get; set; }
        public int Slot { get; set; }
        public string ItemName { get; set; }

        public TakenEvent(string shelf, int slot, string itemName)
        {
            this.Shelf = shelf;
            this.Slot = slot;
            this.ItemName = itemName;
        }
        public string ToLogLine()
        {
            return $"TAKEN|{Shelf}|{Slot}|{ItemName}";
        }
        public string ToScreenLine()
        {
            return $"[Изъятие] '{ItemName}' с полки {Shelf} из слота {Slot}";
        }

        static public TakenEvent FromLogLine(string line)
        {
            string[] p = line.Split('|');
            return new TakenEvent(p[1], int.Parse(p[2]), p[3]);
        }
    }
}
