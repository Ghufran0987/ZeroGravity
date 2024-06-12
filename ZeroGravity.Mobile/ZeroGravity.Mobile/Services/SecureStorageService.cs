using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Services
{
    public class SecureStorageService : ISecureStorageService
    {
        private readonly ILogger _logger;
        public SecureStorageService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<SecureStorageService>() ?? new NullLogger<SecureStorageService>();
        }

        public async Task SaveObject<T>(string key, T value) where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);
                await SecureStorage.SetAsync(key, json);
            }
            catch (Exception e)
            {
                // Possible that device doesn't support secure storage on device.
                _logger.LogError(e, e.Message);
            }
        }

        public async Task SaveString(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception e)
            {
                // Possible that device doesn't support secure storage on device.
                _logger.LogError(e, e.Message);
            }
        }

        public async Task SaveValue<T>(string key, T value) where T : struct
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);
                await SecureStorage.SetAsync(key, json);
            }
            catch (Exception e)
            {
                // Possible that device doesn't support secure storage on device.
                _logger.LogError(e, e.Message);
            }
        }



        public async Task<T> LoadObject<T>(string key) where T : class
        {
            try
            {
                var value = await SecureStorage.GetAsync(key);
                return string.IsNullOrEmpty(value) ? default : JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception e)
            {
                // Possible that device doesn't support secure storage on device.
                _logger.LogError(e, e.Message);
                return default;
            }
        }

        public async Task<string> LoadString(string key)
        {
            try
            {
                return await SecureStorage.GetAsync(key);
            }
            catch (Exception e)
            {
                // Possible that device doesn't support secure storage on device.
                _logger.LogError(e, e.Message);
                return default;
            }
        }

        public async Task<T> LoadValue<T>(string key) where T : struct
        {
            try
            {
                var value = await SecureStorage.GetAsync(key);
                if (value == null)
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception e)
            {
                // Possible that device doesn't support secure storage on device.
                _logger.LogError(e, e.Message);
                return default;
            }
        }

        public async Task<bool> Remove(string key)
        {
            try
            {
                var result = SecureStorage.Remove(key);
                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                // Possible that device doesn't support secure storage on device.
                _logger.LogError(e, e.Message);
                return false;
            }
        }

        public async Task RemoveAll()
        {
            try
            {
                SecureStorage.RemoveAll();
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                // Possible that device doesn't support secure storage on device.
                _logger.LogError(e, e.Message);
            }
        }
    }
}
