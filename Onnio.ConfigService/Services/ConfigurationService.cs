using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onnio.ConfigService.Interface;
using Onnio.ConfigService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Onnio.ConfigService.Services
{

    public class ConfigurationService : IConfigurationService
    {
        private readonly string _configBasePath;

        public ConfigurationService(IConfiguration configuration, ConfigurationServiceOptions options = null)
        {
            options ??= new ConfigurationServiceOptions();

            string executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _configBasePath = options.ConfigBasePath ??
                              configuration["ConfigService:BasePath"] ??
                              Path.Combine(Directory.GetCurrentDirectory(), "ConfigData");

            // Ensure the config directory exists
            if (!Directory.Exists(_configBasePath))
            {
                Directory.CreateDirectory(_configBasePath);
            }
            
        }

        public async Task<T> GetConfigurationAsync<T>(string appName, string section) where T : class
        {
            if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(section))
                throw new ArgumentException("Application name and section must be provided");

            // Load from file
            var appDir = Path.Combine(_configBasePath, appName);
            var configFile = Path.Combine(appDir, $"{section}.json");

            if (!File.Exists(configFile))
            {
                return null;
            }

            var configJson = await File.ReadAllTextAsync(configFile);
            var config = JsonConvert.DeserializeObject<T>(configJson);
            return config;
        }

        public async Task<Dictionary<string, object>> GetAllConfigurationsAsync(string appName)
        {
            if (string.IsNullOrEmpty(appName))
                throw new ArgumentException("Application name must be provided");

            var result = new Dictionary<string, object>();
            var appDir = Path.Combine(_configBasePath, appName);

            if (!Directory.Exists(appDir))
            {
                return result;
            }

            foreach (var file in Directory.GetFiles(appDir, "*.json"))
            {
                var section = Path.GetFileNameWithoutExtension(file);
                var configJson = await File.ReadAllTextAsync(file);
                var config = JsonConvert.DeserializeObject<object>(configJson);
                result[section] = config;
            }

            return result;
        }

        public Task<bool> ConfigurationExistsAsync(string appName, string section)
        {
            if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(section))
                throw new ArgumentException("Application name and section must be provided");

            var appDir = Path.Combine(_configBasePath, appName);
            var configFile = Path.Combine(appDir, $"{section}.json");

            return Task.FromResult(File.Exists(configFile));
        }
    }
}