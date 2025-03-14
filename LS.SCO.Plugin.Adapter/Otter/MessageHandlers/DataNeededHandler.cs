﻿using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class DataNeededHandler : MessageHandler
    {
        public DataNeededHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter adapter)
            : base(otterState, otterProtocolHandler, manager, adapter) { }

        public override void Handle(object message)
        {
            var msg = message as Otter.Models.FromSCO.dataNeeded;
            dataNeeded dataNeeded = new dataNeeded();
            dataNeeded.@params = new dataNeededParams();

            if (msg.result.back && _otterState.Api_Active_Payment_Method is not null)
            {
                payment goPayment = new payment();
                goPayment.id = _otterState.Api_MessageId_Payment;
                goPayment.result = new paymentResult();
                goPayment.result.successful = true;
                _otterState.Api_Active_Payment_Method = null;
                _otterProtocolHandler.SendMessage(goPayment);
                _otterState.Api_MessageId = null;
                return;
            }
            if (_otterState.Api_Active_Payment_Method == "Netgiro")
            {
                var result = _adapter.PayForCurrentTransactionExternal((int)_otterState.Pos_BalanceAmount, "1", msg.result.data.Replace("-","")).Result;

                if (result.ErrorList.Count() > 0)
                {
                    dataNeeded.@params = new dataNeededParams();
                    dataNeeded.@params.operatorMode = false;
                    dataNeeded.@params.titleText = "Netgiro";
                    dataNeeded.@params.keyPad = true;
                    dataNeeded.@params.deviceError = false;
                    dataNeeded.@params.keyPadInputMask = 4;
                    dataNeeded.@params.keyPadPattern = "###-####";
                    dataNeeded.@params.minimalInputLength = 6;
                    dataNeeded.@params.exitButton = 1;
                    dataNeeded.@params.instructionsText = result.ErrorList.First().ErrorMessage;
                    dataNeeded.error = result.ErrorList.First().ErrorMessage;
                    dataNeeded.id = _otterState.Api_MessageId_Payment;
                    _otterProtocolHandler.SendMessage(dataNeeded);
                    return;
                }

                var postOutput = _adapter.FinishTransactionAsync().Result;
                dataNeeded.@params.clearScreen = true;
                dataNeeded.id = _otterState.Api_MessageId_Payment;
                _otterProtocolHandler.SendMessage(dataNeeded);
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.exitPayment
                {
                    result = new exitPaymentResult
                    {
                        successful = true,
                    },
                    id = _otterState.Api_MessageId
                });
                _otterState.Api_MessageId = null;
                _otterEventsManager.sendTransactionFinish();
                _otterState.Reset();
                
            }
        }
    }
}