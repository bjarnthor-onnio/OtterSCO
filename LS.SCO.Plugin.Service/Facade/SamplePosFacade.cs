using AutoMapper;
using LS.SCO.ExternalServices.Interfaces.HardwareStation;
using LS.SCO.Facades.Implementations.Base;
using LS.SCO.Interfaces.ErrorHandling;
using LS.SCO.Interfaces.ExternalServices;
using LS.SCO.Plugin.Service.Interfaces;

namespace LS.SCO.Plugin.Service.Facade
{
    public class SamplePosFacade : BaseSCOServiceFacade<ILSPosServicesClient>, ISamplePosFacade
    {
        public SamplePosFacade(
            IErrorManager errorManager,
            IMapper mapper,
            ILSPosServicesClient posServicesClient,
            IHardwareStationSignalRClient signalRClient,
            IHardwareStationApiClient apiClient) : base(errorManager, mapper, posServicesClient, signalRClient, apiClient)
        {
        }
    }
}