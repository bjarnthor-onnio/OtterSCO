using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.ConfigService.Interface
{
    public interface IConfigurationService
    {
        Task<T> GetConfigurationAsync<T>(string appName, string section) where T : class;
        Task<Dictionary<string, object>> GetAllConfigurationsAsync(string appName);
        Task<bool> ConfigurationExistsAsync(string appName, string section);
    }
}
