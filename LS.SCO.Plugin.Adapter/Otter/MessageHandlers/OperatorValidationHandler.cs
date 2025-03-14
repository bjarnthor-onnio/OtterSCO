using LS.SCO.Entity.DTO.SCOService.StaffLogon;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class OperatorValidationHandler : MessageHandler
    {
        public OperatorValidationHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, 
            SamplePosAdapter samplePosAdapter) : base(otterState, otterProtocolHandler, manager, samplePosAdapter){}

        public override void Handle(object message)
        {
            var operationValidation = (Otter.Models.FromSCO.operatorValidation)message;
            _otterState.Api_MessageId_Operator = operationValidation.id;

            
            //_logService.LogDebug(null, $"Operator key: Value {operatonValidation.@params.key}");

            var res = _adapter.StaffLogon(operationValidation.@params.operatorId, operationValidation.@params.password);
            if (res.ErrorList?.Count() > 0)
            {
                Console.WriteLine("################   ERROR DURING OPERATOR VALIDATION");
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.operatorValidation
                {
                    result = new Otter.Models.FromPOS.operatorValidationResult()
                    {
                        operatorId = res.StaffId,
                        message = res.ErrorList.First().ErrorMessage,
                        successful = false
                    },
                    id = _otterState.Api_MessageId_Operator
                });
            }
            else
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.operatorValidation
                {
                    result = new Otter.Models.FromPOS.operatorValidationResult()
                    {
                        operatorId = operationValidation.@params.operatorId,
                        level = "2",//TODO
                        message = "",
                        successful = true
                    },
                    id = _otterState.Api_MessageId_Operator
                });
            }
        }
    }
}