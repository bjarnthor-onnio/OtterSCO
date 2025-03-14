using LS.SCO.Entity.Base;
using LS.SCO.Entity.DTO.SCOService.GetItemDetails;
using LS.SCO.Entity.Extensions;
using LS.SCO.Interfaces.Factories;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Controllers.Models;
using Microsoft.AspNetCore.Mvc;

namespace LS.SCO.Plugin.Adapter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SampleController : ControllerBase
    {
        private readonly SamplePosAdapter _samplePosAdapter;

        public SampleController(IAdapterFactory adapterFactory)
        {
            this._samplePosAdapter = (SamplePosAdapter)adapterFactory.Adapters[0];
        }

        [HttpGet("item/{id}")]
        public async ValueTask<IActionResult> GetItemDetails(string id)
        {
            var result = await this._samplePosAdapter.GetItemDetailAsync(new GetItemDetailsInputDto { ItemCode = id });

            return this.ValidateResult(result);
        }

        [HttpGet("getCurrentTransaction")]
        public async ValueTask<IActionResult> GetCurrentTransaction()
        {
            var result = await this._samplePosAdapter.GetCurrentTransaction();

            return this.ValidateResult(result);
        }

        [HttpPost("payForCurrentTransaction")]
        public async ValueTask<IActionResult> PayForCurrentTransaction([FromBody] PayForCurrentTransactionInput input)
        {
            var result = await this._samplePosAdapter.PayForCurrentTransaction(input.Value, input.TenderType);

            return this.ValidateResult(result);
        }

        [HttpPost("payForCurrentTransactionExternal")]
        public async ValueTask<IActionResult> PayForCurrentTransactionExternal([FromBody] PayForCurrentTransactionInput input)
        {
            var result = await this._samplePosAdapter.PayForCurrentTransactionExternal(input.Value, input.TenderType, input.CustomerId);

            return this.ValidateResult(result);
        }

        [HttpPost("addItem")]
        public async ValueTask<IActionResult> AddItemToTransaction([FromBody] GetItemDetailsInput tem)
        {
            var result = await this._samplePosAdapter.AddItemToTransaction(tem.ItemId,"");

            return this.ValidateResult(result);
        }

        [HttpPost("createTransaction")]
        public async ValueTask<IActionResult> CreateTransaction()
        {
            var result = await this._samplePosAdapter.StartTransactionAsync();

            return this.ValidateResult(result);
        }

        [HttpPost("finishCurrentTransaction")]
        public async ValueTask<IActionResult> FinishCurrTransaction()
        {
            var result = await this._samplePosAdapter.FinishTransactionAsync();

            return this.ValidateResult(result);
        }

        [HttpPost("voidTransaction")]
        public async ValueTask<IActionResult> VoidTransaction()
        {
            var result = await this._samplePosAdapter.VoidTransactionAsync();

            return this.ValidateResult(result);
        }

        [HttpPost("cancelActiveTransaction")]
        public async ValueTask<IActionResult> CancelActiveTransaction()
        {
            var result = await this._samplePosAdapter.CancelActiveTransactionAsync();

            return this.ValidateResult(result);
        }

        private IActionResult ValidateResult<T>(T result) where T : BaseOutputEntity
        {
            // If valid, return OK status with the result
            if (result.IsValid())
            {
                return Ok(result);
            }

            // If invalid, create a ProblemDetails object and return it with a 400 status code
            var problemDetails = new ProblemDetails
            {
                Status = 400,  // Bad Request
                Title = "Validation Failed",
                Detail = result?.ErrorList.FirstOrDefault()?.ToString() ?? "Unknown error"
            };

            return new ObjectResult(problemDetails) { StatusCode = 400 };

            //return result.IsValid() ? Ok(result) : this.Problem(result.ErrorList.FirstOrDefault().ToString());
        }
    }
}
