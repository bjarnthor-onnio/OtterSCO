using LS.SCO.Interfaces.ErrorHandling;
using LS.SCO.Interfaces.Log;
using LS.SCO.Plugin.Service.Interfaces;
using LS.SCO.Services.Implementations.Validation;

namespace LS.SCO.Plugin.Service.Validation
{
    /// <summary>
    /// Sample base service's validation service
    /// Used to validate general business rules.
    /// </summary>
    public class SampleValidationService : BaseValidationService, ISampleValidationService
    {
        /// <summary>
        /// Default parameter constructor.
        /// </summary>
        /// <param name="logService"></param>
        /// <param name="errorManager"></param>
        public SampleValidationService(ILogManager logService, IErrorManager errorManager) : base(logService, errorManager)
        {

        }
    }
}
