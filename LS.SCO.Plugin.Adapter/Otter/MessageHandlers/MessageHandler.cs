using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    public abstract class MessageHandler
    {
        protected readonly OtterState _otterState;
        protected readonly OtterProtocolHandler _otterProtocolHandler;
        protected readonly SamplePosAdapter _adapter;
        protected readonly OtterEventsManager _otterEventsManager;

        public MessageHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager,SamplePosAdapter samplePosAdapter)
        {
            _otterState = otterState;
            _otterEventsManager = manager;
            _otterProtocolHandler = otterProtocolHandler;
            _adapter = samplePosAdapter;
        }

        public abstract void Handle(object message);

        // Factory method to get appropriate handler
        public static MessageHandler GetHandler(object message, OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter)
        {
            return message switch
            {
                Otter.Models.FromSCO.laneIdleState => new LaneIdleHandler(otterState, otterProtocolHandler, manager,samplePosAdapter),
                Otter.Models.FromSCO.Init => new InitHandler(otterState, otterProtocolHandler, manager,samplePosAdapter),
                Otter.Models.FromSCO.operatorValidation => new OperatorValidationHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),
                Otter.Models.FromSCO.product => new ProductHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),
                Otter.Models.FromSCO.cancelProduct => new CancelProductHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),
                Otter.Models.FromSCO.cancelTransaction => new CancelTransHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),
                Otter.Models.FromSCO.dataNeeded => new DataNeededHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),
                Otter.Models.FromSCO.payment => new PaymentHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),
                Otter.Models.FromSCO.exitPayment => new ExitPaymentHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),
                Otter.Models.FromSCO.goPayment => new GoPaymentHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),
                Otter.Models.FromSCO.shutdown => new ShutdownHandler(otterState, otterProtocolHandler, manager, samplePosAdapter),

                _ => new DefaultHandler(otterState, otterProtocolHandler, manager,samplePosAdapter)
            };
        }
    }
}
