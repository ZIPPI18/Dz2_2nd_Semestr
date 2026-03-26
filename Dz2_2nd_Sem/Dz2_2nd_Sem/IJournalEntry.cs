using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dz2_2nd_Sem
{
    internal interface IJournalEntry
    {
        string ToLogLine();
        string ToScreenLine();
    }
}