using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using Onnio.PaymentService.Services;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class PaymentHandler : MessageHandler
    {
        public PaymentHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager,
            SamplePosAdapter samplePosAdapter) : base(otterState, otterProtocolHandler, manager, samplePosAdapter) { }

        public async override void Handle(object message)
        {
            var msg = (Otter.Models.FromSCO.payment)message;
            _otterState.Api_MessageId = msg.id;
            if (_otterState.Pos_BalanceAmount == 0)
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                {
                    result = new paymentResult
                    {
                        successful = true,
                        amount = 0
                    },
                    id = _otterState.Api_MessageId
                });
                _otterState.Api_MessageId = null;
                var postOutput = await _adapter.FinishTransactionAsync();
                if (postOutput.ErrorList?.Count() > 0)
                {
                    Console.WriteLine("################   ERROR DURING Post Transaction");
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                    {
                        result = new paymentResult
                        {
                            successful = false,
                            message = "Ekki tókst að klára færslu, vinsamlega hafið samband við þjónustuborð."
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
                        message = "Ekki tókst að hefja greiðslu, vinsamlega hafið samband við þjónustuborð."
                    },
                    id = _otterState.Api_MessageId
                });
                _otterState.Api_MessageId = null;
                return;
            }

            var pay = _adapter.PayForCurrentTransaction((long)_otterState.Pos_BalanceAmount, msg.@params.type).Result;

            if (pay.ErrorList?.Count() > 0)
            {
                Console.WriteLine("################   ERROR DURING Send EFT request");
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                {
                    result = new paymentResult
                    {
                        successful = false,
                        message = pay.ErrorList.First().ErrorMessage
                    },
                    id = _otterState.Api_MessageId
                });
            }
            else
            {
                //Payment successfull
                //TODO check if whole amount was paid
                var postOutput = await _adapter.FinishTransactionAsync();

                if (postOutput.ErrorList?.Count() > 0)
                {
                    Console.WriteLine("################   ERROR DURING Post Transaction");
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                    {
                        result = new paymentResult
                        {
                            successful = false,
                            message = "Ekki tókst að klára færslu, vinsamlega hafið samband við þjónustuborð."
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
                    _otterState.Api_MessageId = null;

                    _otterEventsManager.sendTransactionFinish();
                }
            }
        }
        private async void ExternalPayments(Message message)
        {
            var msg = (Otter.Models.FromSCO.payment)message;

            if (msg.@params.type == "18" || msg.@params.type.ToLower().Contains("netgiro") || msg.@params.type.ToLower().Contains("netgiró"))
            {

                var dataNeeded = new dataNeeded();
                dataNeeded.@params = new dataNeededParams();
                dataNeeded.@params.operatorMode = false;
                dataNeeded.@params.titleText = "Netgíró";
                dataNeeded.@params.instructionsText = "Sláðu inn símanúmer";
                dataNeeded.@params.keyPad = true;
                dataNeeded.@params.deviceError = false;
                dataNeeded.@params.keyPadInputMask = 4;
                dataNeeded.@params.keyPadPattern = "###-####";
                dataNeeded.@params.minimalInputLength = 6;
                dataNeeded.@params.exitButton = 1;

                dataNeeded.id = Guid.NewGuid().ToString();
                _otterState.Api_MessageId_Payment = dataNeeded.id;
                _otterState.Api_Active_Payment_Method = msg.@params.type;
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
                _otterState.Api_Active_Payment_Method = "Pei";
                _otterProtocolHandler.SendMessage(dataNeeded);

                return;
            }
            if (msg.@params.type == "40")
            {
                var dataNeeded = new dataNeeded();
                dataNeeded.@params = new dataNeededParams();
                dataNeeded.@params.operatorMode = false;
                dataNeeded.@params.titleText = "Gjafakort";
                dataNeeded.@params.instructionsText = "Skannaðu strikamerki eða sláðu inn kortanúmer";
                dataNeeded.@params.keyPad = true;
                dataNeeded.@params.deviceError = false;
                dataNeeded.@params.minimalInputLength = 6;
                dataNeeded.@params.exitButton = 1;
                dataNeeded.@params.scannerEnabled = true;

                dataNeeded.id = Guid.NewGuid().ToString();
                _otterState.Api_MessageId_Payment = dataNeeded.id;
                _otterState.Api_Active_Payment_Method = "40";
                _otterProtocolHandler.SendMessage(dataNeeded);

                return;
            }
            if (msg.@params.type == "23")
            {

                var result = _adapter.PayForCurrentTransactionExternal("23").Result;
                if (result.Success)
                {
                    var postOutput = await _adapter.FinishTransactionAsync();

                    if (postOutput.ErrorList?.Count() > 0)
                    {
                        Console.WriteLine("################   ERROR DURING Post Transaction");
                        _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                        {
                            result = new paymentResult
                            {
                                successful = false,
                                message = "Ekki tókst að bóka færslu en greiðsla hefur verið kláruð. Hafið samband við starfsmann."
                            },
                            id = _otterState.Api_MessageId
                        });
                        _otterState.Api_MessageId = null;
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

                    }
                    return;

                }
                else
                {
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                    {
                        result = new paymentResult
                        {
                            successful = false,
                            message = result.ErrorList.First().ErrorMessage
                        },
                        id = _otterState.Api_MessageId
                    });
                    _otterState.Api_MessageId = null;
                }

            }
            else
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.payment
                {
                    result = new paymentResult
                    {
                        successful = false,
                        message = $"Payment method not supported, check central cofiguration. Data received: {msg.@params.type}"
                    },
                    id = _otterState.Api_MessageId
                });
                _otterState.Api_MessageId = null;
            }
        }
    }
}