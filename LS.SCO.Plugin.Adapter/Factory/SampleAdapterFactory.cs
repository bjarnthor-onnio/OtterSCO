using LS.SCO.Interfaces.Adapter;
using LS.SCO.Interfaces.Factories;
using LS.SCO.Plugin.Adapter.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace LS.SCO.Plugin.Adapter.Factory
{
    /// <summary>
    /// Factory that can create instances of Adapter classes
    /// </summary>
    public class SampleAdapterFactory : IAdapterFactory
    {
        private readonly IServiceProvider _services;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SampleAdapterFactory(IServiceProvider services)
        {
            this._services = services;
        }

        /// <summary>
        /// List of adapter instances created.
        /// </summary>
        public List<IPosAdapter> Adapters { get; set; }

        /// <summary>
        /// Creates a collection of adapter instances for multi SCO devices
        /// </summary>
        public IEnumerable<IPosAdapter> CreateAdapterCollection()
        {
            Adapters = new List<IPosAdapter>();

            var adapter = this._services.GetService<IPosAdapter>() as SamplePosAdapter;

            adapter?.SetUpServices(0);

            Adapters.Add(adapter);

            return Adapters;
        }
    }
}