using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BridgeMonitor.Models
{
    public class fermeture
    {
        public List<Bridge> BridgesBefore { get; set; }
        public List<Bridge> BridgesAfter { get; set; }
    }
}
