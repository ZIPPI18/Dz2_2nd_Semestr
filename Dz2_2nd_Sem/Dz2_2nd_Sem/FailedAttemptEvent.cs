using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dz2_2nd_Sem
{
    internal class FailedAttemptEvent : IJournalEntry
    {
        public string shelf { get; set; }
        public int slot { get; set; }
        public string cause { get; set; }
        public string action { get; set; }
        public FailedAttemptEvent(string shelf, int slot, string cause, string action) 
        { 
            this.shelf = shelf;
            this.slot = slot;
            this.cause = cause;
            this.action = action;
        }

        public string ToLogLine()
        {
            return $"Failed|{action}|{shelf}|{slot}|{cause}";
        }

        public string ToScreenLine()
        {
            return $"Неудача | {action} | полка '{shelf}' слот '{slot}' | причина: {cause}";
        }

        static public FailedAttemptEvent FromLogLine(string line)
        {
            string[] p = line.Split('|');
            return new FailedAttemptEvent(p[2], int.Parse(p[3]), p[4], p[1]);
        }
    }
}
