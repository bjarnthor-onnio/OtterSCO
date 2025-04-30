using System.Runtime.InteropServices;

using LS.SCO.Entity.Base;
using LS.SCO.Interfaces.Adapter;
using Microsoft.Extensions.DependencyInjection;
using Onnio.ConfigService.Extensions;
using Onnio.ConfigService.Interface;
using Onnio.ConfigService.Models;
using Onnio.ConfigService.Services;

namespace LS.SCO.Plugin.Adapter.Adapters.Extensions
{
    /// <summary>
    /// static class with input entities methods 
    /// </summary>
    public static class InputProperties
    {
        /// <summary>
        /// Fills the basic input properties for Adapter Identification
        /// </summary>
        /// <param name="input"></param>
        /// <param name="adapter"></param>
        
       

        public static void ConfigureBaseInputProperties(this BaseCachingEntity input, IPosAdapter adapter)
        {
            var sampleAdapter = (SamplePosAdapter)adapter;
            var config = StaticServiceAdapter.GetBcConfig();

            if (input != null && sampleAdapter?.AdapterConfiguration != null)
            {
                input.Token = sampleAdapter?.AdapterConfiguration.Token;
                input.StoreId = sampleAdapter?.AdapterConfiguration.StoreId;
                input.TerminalId = sampleAdapter?.AdapterConfiguration.TerminalId;
                input.StaffId = sampleAdapter?.AdapterConfiguration.StaffId ?? config.StaffId;
            }
        }
    }

}