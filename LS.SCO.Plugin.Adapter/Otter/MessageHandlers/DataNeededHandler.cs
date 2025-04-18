﻿using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using static System.Net.Mime.MediaTypeNames;

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
            bool foundPaymentMethod = false;
            if (msg.result.back && _otterState.Api_Active_Payment_Method is not null)
            {
                payment goPayment = new payment();
                goPayment.id = _otterState.Api_MessageId;
                goPayment.result = new paymentResult();
                goPayment.result.successful = true;
                _otterState.Api_Active_Payment_Method = null;
                _otterProtocolHandler.SendMessage(goPayment);
                _otterState.Api_MessageId = null;
                return;
            }
            //TODO: Create a handler pattern for the different payment methods

            if (_otterState.Api_Active_Payment_Method == "Netgiro_TT")
            {
                dataNeeded.@params.clearScreen = true;
                dataNeeded.id = _otterState.Api_MessageId_Payment;
                _otterProtocolHandler.SendMessage(dataNeeded);

                var result = _adapter.PayForCurrentTransactionExternal("Netgiro_TT", msg.result.data.Replace("-", ""), false).Result;

                if (result.ErrorList.Count() > 0)
                {
                    dataNeeded.@params = new dataNeededParams();
                    dataNeeded.@params.operatorMode = false;
                    dataNeeded.@params.titleText = "Netgiro";
                    dataNeeded.@params.keyPad = true;
                    dataNeeded.@params.deviceError = false;
                    dataNeeded.@params.keyPadInputMask = 4;
                    dataNeeded.@params.keyPadPattern = "######-####";
                    dataNeeded.@params.minimalInputLength = 10;
                    dataNeeded.@params.exitButton = 1;
                    dataNeeded.@params.scannerEnabled = true;
                    dataNeeded.@params.instructionsText = result.ErrorList.First().ErrorMessage;
                    dataNeeded.error = result.ErrorList.First().ErrorMessage;
                    dataNeeded.id = _otterState.Api_MessageId_Payment;
                    _otterProtocolHandler.SendMessage(dataNeeded);

                    return;
                }
                foundPaymentMethod = true;
                dataNeeded.@params.clearScreen = true;
                dataNeeded.id = _otterState.Api_MessageId_Payment;
                _otterProtocolHandler.SendMessage(dataNeeded);
                _otterState.Api_MessageId_Payment = null;

            }
            if (_otterState.Api_Active_Payment_Method == "22")
            {
                var result = _adapter.PayForCurrentTransactionExternal("22", msg.result.data.Replace("-", "")).Result;

                if (result.ErrorList.Count() > 0)
                {
                    dataNeeded.@params = new dataNeededParams();
                    dataNeeded.@params.operatorMode = false;
                    dataNeeded.@params.titleText = "Pei";
                    dataNeeded.@params.keyPad = true;
                    dataNeeded.@params.keyPadPattern = "####-####-####";
                    dataNeeded.@params.minimalInputLength = 12;
                    dataNeeded.@params.deviceError = false;
                    dataNeeded.@params.exitButton = 1;
                    dataNeeded.@params.instructionsText = result.ErrorList.First().ErrorMessage;
                    dataNeeded.error = result.ErrorList.First().ErrorMessage;
                    dataNeeded.id = _otterState.Api_MessageId_Payment;
                    _otterProtocolHandler.SendMessage(dataNeeded);

                    return;
                }
                dataNeeded.@params.clearScreen = true;
                dataNeeded.id = _otterState.Api_MessageId_Payment;
                _otterProtocolHandler.SendMessage(dataNeeded);
                _otterState.Api_MessageId_Payment = null;
                foundPaymentMethod = true;
            }
            if (_otterState.Api_Active_Payment_Method == "40")
            {
                var result = _adapter.PayForCurrentTransactionExternal("40", msg.result.data).Result;

                if (result.ErrorList.Count() > 0)
                {
                    //TODO: Skítamix þarf að laga í BC
                    string error = result.ErrorList.First().ErrorMessage;
                    string errorMessage = "Villa við greiðslu";

                    if (error.Contains("Card is not found"))
                    {
                        errorMessage = "Kortið fannst ekki";
                    }
                    if (error.Contains("is not enough for total amount"))
                    {
                        errorMessage = $"Ekki næg innegn á korti.";
                    }

                    dataNeeded.@params = new dataNeededParams();
                    dataNeeded.@params.operatorMode = false;
                    dataNeeded.@params.titleText = "Villa";
                    dataNeeded.@params.keyPad = false;
                    dataNeeded.@params.deviceError = false;
                    dataNeeded.@params.exitButton = 1;
                    dataNeeded.@params.instructionsText = errorMessage;
                    dataNeeded.error = errorMessage;
                    dataNeeded.id = _otterState.Api_MessageId_Payment;
                    _otterProtocolHandler.SendMessage(dataNeeded);
                    return;
                }
                dataNeeded.@params.clearScreen = true;
                dataNeeded.id = _otterState.Api_MessageId_Payment;
                _otterProtocolHandler.SendMessage(dataNeeded);
                foundPaymentMethod = true;
                _otterState.Api_MessageId_Payment = null;
            }
            if(!foundPaymentMethod)
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.exitPayment
                {
                    result = new exitPaymentResult
                    {
                        successful = false,
                        message = "Misræmi í greiðsluháttarskilgreiningum. Hafið samband við þjónustuborð."
                    },
                    id = _otterState.Api_MessageId
                });
                return;
            }

            var postOutput = _adapter.FinishTransactionAsync().Result;
            
            if (postOutput.ErrorList.Count() > 0)
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.exitPayment
                {
                    result = new exitPaymentResult
                    {
                        successful = false,
                        message = "Ekki tókst að klára færslu, hætt við bókun. Greiðsla hefur verið kláruð, vinsamlega hafið samband við þjónustuborð."
                    },
                    id = _otterState.Api_MessageId
                });

            }
            else
            {

                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                {
                    result = new paymentResult
                    {
                        successful = true,
                        amount = (int)_otterState.Pos_BalanceAmount * 100
                    },
                    id = _otterState.Api_MessageId
                });

                _otterEventsManager.sendTransactionFinish();
                _otterState.Reset();

            }
        }
    }
}