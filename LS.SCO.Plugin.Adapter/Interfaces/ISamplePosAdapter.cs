using LS.SCO.Entity.Adapters;
using LS.SCO.Entity.DTO.SCOService.GetItemDetails;
using LS.SCO.Entity.DTO.SCOService.StartTransaction;
using LS.SCO.Interfaces.Adapter;
using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.CurrentTransaction;
using LS.SCO.Entity.Base;
using GetItemDetailsOutputDto = LS.SCO.Entity.DTO.SCOService.GetItemDetails.GetItemDetailsOutputDto;
using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Entity.DTO.SCOService.VoidTransaction;
using LS.SCO.Entity.DTO.SCOService.VoidItem;
using LS.SCO.Entity.DTO.SCOService.CancelActiveTransaction;
using LS.SCO.Entity.DTO.SCOService.StaffLogon;
using LS.SCO.Entity.DTO.SCOService.CalculateBasket;




namespace LS.SCO.Plugin.Adapter.Interfaces
{
    /// <summary>
    /// Interface for Sample POS Adapter
    /// </summary>
    public interface ISamplePosAdapter : IPosAdapter
    {
        
        BaseAdapterConfiguration AdapterConfiguration { get; set; }
        Task<GetItemDetailsOutputDto> GetItemDetailAsync(GetItemDetailsInputDto item);
        Task<StartTransactionOutputDto> StartTransactionAsync();
        Task<AddToTransOutputDto> AddToTransaction(AddToTransInputDto input);
        Task<GetCurrentTransactionOutputDto> GetCurrentTransaction();
        Task<AddToTransOutputDto> PayForCurrentTransactionExternal(string tenderType, decimal? amount, string customerId = "", bool skipPaymentLine = true, string confirmationCode = "");
        Task<BaseOutputEntity> PayForCurrentTransaction(decimal amount, string tenderType);
        Task<FinishTransactionOutputDto> FinishTransactionAsync();
        Task<VoidTransactionOutputDto> VoidTransactionAsync();
        Task<VoidItemOutputDto> VoidItemAsync(string itemCode, string lineNo);
        Task<CancelActiveTransactionOutputDto> CancelActiveTransactionAsync();
        StaffLogonOutputDto StaffLogon(string operatorId, string password);
        bool LobicoTrigger(string receptId);
        Task<CalculateBasketOutputDto> CalculateTotals();



    }
}