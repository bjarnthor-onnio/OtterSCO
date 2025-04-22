using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerTech.Otter.Client.Interface
{
    public interface IOtterState
    {
        void Reset();
        Enum StatesEnum { get; set; }
    }
}
