using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class PaymentHandler : MessageHandler
    {
        public PaymentHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, 
            SamplePosAdapter samplePosAdapter) : base(otterState, otterProtocolHandler, manager, samplePosAdapter){}

        public async override void Handle(object message)
        {
            var msg = (Otter.Models.FromSCO.payment)message;
            _otterState.Api_MessageId_Payment = msg.id;

            if (msg.@params.amount == 0)
            {
                var postOutput = await _adapter.FinishTransactionAsync();
                if (postOutput.ErrorList?.Count() > 0)
                {
                    Console.WriteLine("################   ERROR DURING Post Transaction");
                }
                else
                {

                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                    {
                        result = new paymentResult
                        {
                            successful = true,
                        },
                        id = _otterState.Api_MessageId
                    });
                    _otterState.Api_MessageId = null;

                    _otterEventsManager.sendTransactionFinish();

                }
            }

            if (msg.@params.type != "3")
            {
                ExternalPayments(msg);
                return;
            }

            var prePay = await _adapter.PreparePaymentAsync();

            if (prePay.ErrorList?.Count() > 0)
            {
                Console.WriteLine("################   ERROR DURING prepare payment");
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                {
                    result = new paymentResult
                    {
                        successful = false,
                        message = "Failed to Prepare Payment at LS Central"
                    },
                    id = _otterState.Api_MessageId
                });
                _otterState.Api_MessageId = null;
                return;
            }
            

            //_logService.MethodStart("Payment starts");

            _otterState.Api_MessageId = msg.id;

            //todo send clear screen
            //TODO for testing purposes we empty transaction, need to check if paid full amount

            //var _currTransaction = await _adapter.GetCurrentTransaction();


            var pay = _adapter.PayForCurrentTransaction((long)_otterState.Pos_BalanceAmount, msg.@params.type).Result;

            if (pay.ErrorList?.Count() > 0)
            {
                Console.WriteLine("################   ERROR DURING Send EFT request");
            }
            else
            {
                //Payment successfull
                //TODO check if whole amount was paid

                var postOutput = await _adapter.FinishTransactionAsync();


                if (postOutput.ErrorList?.Count() > 0)
                {
                    Console.WriteLine("################   ERROR DURING Post Transaction");
                }
                else
                {

                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                    {
                        result = new paymentResult
                        {
                            successful = true,
                        },
                        id = _otterState.Api_MessageId
                    });
                    _otterState.Api_MessageId = null;

                    _otterEventsManager.sendTransactionFinish();

                }
            }
        }
        private void ExternalPayments(Message message)
        {
            var msg = (Otter.Models.FromSCO.payment)message;
            if (msg.@params.type == "netgiro")
            {

                var dataNeeded = new dataNeeded();
                dataNeeded.@params = new dataNeededParams();
                dataNeeded.@params.operatorMode = false;
                dataNeeded.@params.titleText = "Netgiro";
                dataNeeded.@params.instructionsText = "Sláðu inn símanúmer";
                dataNeeded.@params.keyPad = true;
                dataNeeded.@params.deviceError = false;
                dataNeeded.@params.keyPadInputMask = 4;
                dataNeeded.@params.keyPadPattern = "###-####";
                dataNeeded.@params.minimalInputLength = 6;
                dataNeeded.@params.exitButton = 1;

                dataNeeded.id = Guid.NewGuid().ToString();
                _otterState.Api_MessageId = message.id;
                _otterState.Api_Active_Payment_Method = "Netgiro";
                _otterProtocolHandler.SendMessage(dataNeeded);
                return;
            }
            if (msg.@params.type == "pei")
            {
                var dataNeeded = new dataNeeded();
                dataNeeded.@params = new dataNeededParams();
                dataNeeded.@params.operatorMode = false;
                dataNeeded.@params.titleText = "Pei";
                dataNeeded.@params.instructionsText = "Skannaðu strikamerki";
                dataNeeded.@params.keyPad = false;
                dataNeeded.@params.deviceError = false;
                dataNeeded.@params.minimalInputLength = 6;
                dataNeeded.@params.exitButton = 1;
                dataNeeded.@params.scannerEnabled = true;

                dataNeeded.id = Guid.NewGuid().ToString();
                _otterState.Api_MessageId_Payment = dataNeeded.id;
                _otterState.Api_MessageId = message.id;
                _otterState.Api_Active_Payment_Method = "Pei";
                _otterProtocolHandler.SendMessage(dataNeeded);
                return;
            }
            if (msg.@params.type == "")
            {                 
                //TODO - handle 
            }
        }
    }
}