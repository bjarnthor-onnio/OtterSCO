using LS.SCO.Entity.Constants;
using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.CancelActiveTransaction;
using LS.SCO.Entity.DTO.SCOService.GetItemDetails;
using LS.SCO.Entity.DTO.SCOService.Items;
using LS.SCO.Entity.DTO.SCOService.StaffLogon;
using LS.SCO.Helpers.Extensions;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;
using Microsoft.AspNetCore.Mvc;
using LS.SCO.Entity;
using LS.SCO.Entity.DTO.SCOService.VoidTransaction;
using LS.SCO.Entity.Extensions;
using LS.SCO.Entity.DTO.SCOService.VoidItem;
using Microsoft.VisualBasic;

namespace LS.SCO.Plugin.Adapter.Adapters
{
    public partial class SamplePosAdapter
    {

        public void ConnectoToSco()
        {
            _otterService.ConnectToSCO("127.0.0.1", 9000);
            _otterService.OnMessageReceived += OnScoMessageReceived;
            _otterService.Disconnected += OnScoDisconnected;
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
