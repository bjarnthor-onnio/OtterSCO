using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.ConfigService.Interface;
using Onnio.ConfigService.Models;
using Onnio.ConfigService.Services;
using Newtonsoft.Json;

namespace Onnio.ConfigService.Extensions
{
    public class StaticServiceAdapter
    {
        //get config from config file
        public static BcConfig GetBcConfig()
        {

            string jsonString = File.ReadAllText("C:\\ProgramData\\LS Retail\\LS Self-Checkout Connector\\Plugins\\Config\\BcConfig.json");

            // Deserialize the JSON string
            var config = JsonConvert.DeserializeObject<BcConfig>(jsonString);
            return config;
        }

    }
}
