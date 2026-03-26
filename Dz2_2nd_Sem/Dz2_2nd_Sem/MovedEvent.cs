using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dz2_2nd_Sem
{
    internal class MovedEvent : IJournalEntry
    {
        public string FromShelf { get; set; }
        public int FromSlot { get; set; }
        public string ToShelf { get; set; }
        public int ToSlot { get; set; }
        public string ItemName { get; set; }

        public MovedEvent(string fromShelf, int fromSlot, string toShelf, int toSlot, string itemName)
        {
            FromShelf = fromShelf;
            FromSlot = fromSlot;
            ToShelf = toShelf;
            ToSlot = toSlot;
            ItemName = itemName;
        }

        public string ToLogLine()
        {
            return $"MOVED|{FromShelf}|{FromSlot}|{ToShelf}|{ToSlot}|{ItemName}";
        }

        public string ToScreenLine()
        {
            return $"[Перемещение] '{ItemName}' из {FromShelf}[{FromSlot}] в {ToShelf}[{ToSlot}]";
        }

        static public MovedEvent FromLogLine(string line)
        {
            string[] p = line.Split('|');
            return new MovedEvent(p[1], int.Parse(p[2]), p[3], int.Parse(p[4]), p[5]);
        }
    }
}

