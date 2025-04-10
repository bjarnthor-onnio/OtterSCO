﻿using System.Runtime.InteropServices;

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

            if (input != null && sampleAdapter?.Configuration != null)
            {
                input.Token = sampleAdapter?.Configuration.Token;
                input.StoreId = sampleAdapter?.Configuration.StoreId;
                input.TerminalId = sampleAdapter?.Configuration.TerminalId;
                input.StaffId = sampleAdapter?.Configuration.StaffId ?? config.StaffId;
            }
        }
    }

}