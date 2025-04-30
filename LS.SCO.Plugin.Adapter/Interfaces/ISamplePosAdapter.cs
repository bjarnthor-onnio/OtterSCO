using LS.SCO.Entity.Adapters;
using LS.SCO.Interfaces.Adapter;

namespace LS.SCO.Plugin.Adapter.Interfaces
{
    /// <summary>
    /// Interface for Sample POS Adapter
    /// </summary>
    public interface ISamplePosAdapter : IPosAdapter
    {
        /// <summary>
        /// Class id, used to udenfity different SCO devices connected to the SCO Connector
        /// </summary>
        BaseAdapterConfiguration AdapterConfiguration { get; set; }
    }
}