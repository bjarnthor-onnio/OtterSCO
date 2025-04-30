using LS.SCO.Entity.Constants;
using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.CancelActiveTransaction;
using LS.SCO.Entity.DTO.SCOService.GetItemDetails;
using LS.SCO.Entity.DTO.SCOService.Items;
using LS.SCO.Entity.DTO.SCOService.StaffLogon;
using LS.SCO.Helpers.Extensions;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;
using LS.SCO.Plugin.Adapter.Otter;
using LS.SCO.Plugin.Adapter.Otter.Models;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using Microsoft.AspNetCore.Mvc;
using LS.SCO.Entity;
using LS.SCO.Entity.DTO.SCOService.VoidTransaction;
using LS.SCO.Entity.Extensions;
using LS.SCO.Entity.DTO.SCOService.VoidItem;
using Microsoft.VisualBasic;
using LS.SCO.Plugin.Adapter.Otter.MessageHandlers;

namespace LS.SCO.Plugin.Adapter.Adapters
{
    public partial class SamplePosAdapter
    {
        protected OtterProtocolHandler _otterProtocolHandler;
        protected OtterEventsManager _manager;
        protected OtterState _otterState;

        public void ConnectoToOtterSCO()
        {
            this._otterProtocolHandler = new OtterProtocolHandler(_logService, _logService,_configService);
            this._otterState = new OtterState();
            this._manager = new OtterEventsManager(_logService, _logService, this._otterState, this._otterProtocolHandler);

            _otterProtocolHandler.ConnectToSCO("127.0.0.1", 9000); //TODO - get from config or database
            _otterProtocolHandler.OnMessageReceived += OnScoMessageReceived;
            _otterProtocolHandler.Disconnected += OnScoDisconnected;

        }

        private void OnScoDisconnected()
        {
            _logService.LogInfo($"SCO disconnected");
            _otterState.Reset();
            //TODO restart connection to Otter SCO

        }

        private void OnScoMessageReceived(Message message)
        {
            MessageHandler.GetHandler(message, _otterState, _otterProtocolHandler, _manager, this).Handle(message);
        }

    }
}
