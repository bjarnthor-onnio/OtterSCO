using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.Items;
using LS.SCO.Entity.Model.Toshiba.POS;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Extensions;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using LS.SCO.Plugin.Adapter.Otter.Models.FromSCO;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class ChangeProductQuantityHandler : MessageHandler
    {
        public ChangeProductQuantityHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter)
           : base(otterState, otterProtocolHandler, manager, samplePosAdapter) { }
        public async override void Handle(object message)
        {
            var msg = message as Otter.Models.FromSCO.changeProductQuantity;
            _otterState.Api_MessageId = msg.id;
            //Cancel product in LS Central and otter
            var cancelProduct = await _adapter.VoidItemAsync(msg.@params.barcode, msg.@params.productId.ToString());
            _otterProtocolHandler.SendMessage(new Otter.Models.FromSCO.cancelProduct
            {
                @params = new cancelProductParams
                {
                    productId = msg.@params.productId,
                    barcode = msg.@params.barcode
                },
                id = new Guid().ToString()
            });
            //Add product with new quantity in LS Central and otter
            var addItem = _adapter.AddItemToTransaction(msg.@params.barcode, "", _otterState.Pos_TransactionId, msg.@params.quantity, false).Result;
            SaleItemDto saleItem = addItem.Transaction.SaleItems?.OrderBy(x => x.LineNr).Last();

            addProduct addProduct = ProductHelper.PopulateAddProduct(saleItem);
            _otterProtocolHandler.SendMessage(addProduct);

            //Send changeProductQuantity response
            _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.changeProductQuantity
            {
                id = _otterState.Api_MessageId,
                result = new changeProductQuantityResult
                {
                    successful = true,
                }
            });
            _otterState.Api_MessageId = null;
            _otterEventsManager.sendTotals(addItem.Transaction.BalanceAmountWithTax, addItem.Transaction.NetAmountWithTax);

           

            
            return;
        }
    }
}
