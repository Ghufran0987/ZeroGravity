using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Models.StreamContent;

namespace ZeroGravity.Services
{
    public class ExternalApiTokenService : IExternalApiTokenService
    {
        private readonly IMemoryCache cache;
        private readonly ShowHeroesSettings _settings;

        public ExternalApiTokenService(IMemoryCache cache, IOptions<AppSettings> appSettings)
        {
            this.cache = cache;
            _settings = appSettings.Value.ShowHeroesSettings;
        }

        public string FetchToken()
        {
            string token;

            // if cache doesn't contain an entry called TOKEN error handling mechanism is mandatory
            if (!cache.TryGetValue("TOKEN", out token))
            {
                TokenModel tokenmodel = GetTokenFromApiAsync().Result;

                // keep the value within cache for
                // given amount of time
                // if value is not accessed within the expiry time
                // delete the entry from the cache
                var options = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(
                              TimeSpan.FromSeconds(tokenmodel.ExpiresIn));

                // TODO Add other value as well
                cache.Set("TOKEN", tokenmodel.AccessToken, options);
                token = tokenmodel.AccessToken;
            }

            return token;
        }

        private async Task<TokenModel> GetTokenFromApiAsync()
        {
            var kvpList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type",_settings.grant_type),
                new KeyValuePair<string, string>("username",_settings.username),
                new KeyValuePair<string, string>("password",_settings.password),
                new KeyValuePair<string, string>("client_id",_settings.client_id),
                new KeyValuePair<string, string>("client_secret",_settings.client_secret),
                new KeyValuePair<string, string>("scope",_settings.scope),
                new KeyValuePair<string, string>("grant_type",_settings.grant_type),
            };
            FormUrlEncodedContent rqstBody = new FormUrlEncodedContent(kvpList);
            var url = $"{_settings.BaseUrl}{"oauth/token"}";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage resp = await httpClient.PostAsync(url, rqstBody); //rqstBody is HttpContent
                    if (resp != null && resp.Content != null)
                    {
                        var jsonData = await resp.Content.ReadAsStringAsync();
                        var contractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        };
                        var settings = new JsonSerializerSettings()
                        {
                            ContractResolver = contractResolver
                        };
                        var result = JsonConvert.DeserializeObject<TokenModel>(jsonData, settings);
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
        }
    }
}