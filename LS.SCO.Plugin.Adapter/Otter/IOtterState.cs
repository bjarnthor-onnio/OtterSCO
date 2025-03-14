using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.SCO.Plugin.Adapter.Otter
{
    public interface IOtterState
    {
        void Reset();
        Enum StatesEnum { get; set; }
    }
}
