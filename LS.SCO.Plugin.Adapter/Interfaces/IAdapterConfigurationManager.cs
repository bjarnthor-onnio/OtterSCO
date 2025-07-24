using LS.SCO.Entity.Adapters;

namespace LS.SCO.Plugin.Adapter.Interfaces
{
    /// <summary>
    /// Access the Adapter's configurations
    /// </summary>
    public interface IAdapterConfigurationManager
    {
        /// <summary>
        /// Configuration instance
        /// </summary>
        List<BaseAdapterConfiguration> Configuration { get; set; }
    }
}