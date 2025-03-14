using LS.SCO.Interfaces.Services.Validation;

namespace LS.SCO.Plugin.Service.Validation
{
    /// <summary>
    /// Sample adapter validation service
    /// </summary>
    public class SampleAdapterValidationService : IAdapterValidationService
    {

        /// <summary>
        /// Validates the adapter configuration
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ValidateAdapterConfiguration(int position = 0)
        {
            return true;
        }
    }
}
