using LS.SCO.Interfaces.Adapter;
using LS.SCO.Interfaces.Factories;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LS.SCO.Plugin.Adapter.Factory
{
    /// <summary>
    /// Factory that can create instances of Adapter classes
    /// </summary>
    public class SampleAdapterFactory : IAdapterFactory
    {
        private readonly IAdapterConfigurationManager _configurationManager;
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SampleAdapterFactory(IAdapterConfigurationManager configurationManager, IServiceScopeFactory scopeFactory)
        {
            this._configurationManager = configurationManager;
            this._scopeFactory = scopeFactory;
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

            for (int i = 0; i < this._configurationManager.Configuration.Count; i++)
            {
                var adapter = CreateScopedAdapter();

                adapter?.SetUpServices(i);

                Adapters.Add(adapter);
            }

            return Adapters;
        }
        private ISamplePosAdapter CreateScopedAdapter()
        {
            var result = default(ISamplePosAdapter);

            if (this._scopeFactory != null)
            {
                var scope = this._scopeFactory.CreateScope();

                var adapter = scope.ServiceProvider.GetRequiredService<ISamplePosAdapter>();

                adapter.Scope = scope;

                result = adapter;
            }

            return result;
        }
    }
}