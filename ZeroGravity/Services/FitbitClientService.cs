using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ZeroGravity.Db.Models;
using ZeroGravity.Helpers;
using ZeroGravity.Infrastructure;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class FitbitClientService : IFitbitClientService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly FitbitSettings _fitbitSettings;
        private readonly IIntegrationDataService _integrationDataService;

        public FitbitClientService(IIntegrationDataService integrationDataService, IOptions<AppSettings> appSettings)
        {
            _integrationDataService = integrationDataService;
            _appSettings = appSettings;

            _fitbitSettings = _appSettings.Value.FitbitSettings;
        }

        public Task<FitbitAccountDto> GetFitbitAuthenticationInfo(int accountId)
        {
            var fitbitAccountDto = new FitbitAccountDto
            {
                AccountId = accountId
            };

            var callBackUrl = $"{_fitbitSettings.CallbackUrl}/fitbit/callback";

            var scopes = new List<string> {"activity"};


            var authUrl = GenerateFitbitAuthenticationUrl(callBackUrl, scopes, accountId);

            fitbitAccountDto.AuthenticationUrl = authUrl;

            return Task.FromResult(fitbitAccountDto);
        }

        public async Task<bool> GetFitbitUserTokenByCode(FitbitCallbackDto fitbitCallbackParams)
        {
            using var client = new HttpClient();

            var isSucessful = false;

            var combinedString = _fitbitSettings.ClientId + ":" + _fitbitSettings.ClientSecret;

            var base64EncodedSecret = Base64Encode(combinedString);

            client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedSecret);
            //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

            var callBackUrl = $"{_fitbitSettings.CallbackUrl}/fitbit/callback";

            var parameters = new Dictionary<string, string>
            {
                {"client_id", _fitbitSettings.ClientId}, {"grant_type", "authorization_code"},
                {"redirect_uri", callBackUrl}, {"code", fitbitCallbackParams.Code}
            };

            var encodedContent = new FormUrlEncodedContent(parameters);
            try
            {
                var response = await client.PostAsync("https://api.fitbit.com/oauth2/token", encodedContent);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var callbackDto = JsonConvert.DeserializeObject<FitbitCallbackDto>(responseBody);

                if (callbackDto != null)
                    if (int.TryParse(fitbitCallbackParams.State, out var accountId))
                    {
                        var integration =
                            await _integrationDataService.GetIntegrationByNameAsync(IntegrationNameConstants.Fitbit);

                        var savedIntegration =
                            await _integrationDataService.GetLinkedIntegrationByIntegrationIdAsync(accountId,
                                integration.Id);

                        if (savedIntegration != null)
                        {
                            savedIntegration.AccessToken = callbackDto.Access_Token;
                            savedIntegration.RefreshToken = callbackDto.Refresh_Token;
                            savedIntegration.UserId = callbackDto.User_Id;

                            await _integrationDataService.UpdateAsync(savedIntegration);
                        }
                        else
                        {
                            var linkedIntegration = new LinkedIntegration
                            {
                                AccountId = accountId,
                                AccessToken = fitbitCallbackParams.Access_Token,
                                RefreshToken = fitbitCallbackParams.Refresh_Token,
                                UserId = fitbitCallbackParams.User_Id,
                                IntegrationId = integration.Id
                            };

                            await _integrationDataService.AddAsync(linkedIntegration);
                        }
                    }

                isSucessful = true;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine("\nException Caught!");
                Debug.WriteLine("Message :{0} ", e.Message);
            }

            return isSucessful;
        }

        public async Task<FitbitActivityDataDto> GetFitbitActivitiesAsync(int accountId, int integrationId,
            DateTime targetDateTime)
        {
            //try
            //{
                var integration =
                    await _integrationDataService.GetLinkedIntegrationByIntegrationIdAsync(accountId, integrationId);

                if (integration != null)
                {
                    var targetDate = targetDateTime.ToString("yyyy-MM-dd");

                    var url = $"https://api.fitbit.com/1/user/{integration.UserId}/activities/date/{targetDate}.json";

                    var client = new HttpClient();

                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", integration.AccessToken);

                    var response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();

                    var fitbitAccountDto = JsonConvert.DeserializeObject<FitbitActivityDataDto>(responseBody);

                    return fitbitAccountDto;
                }
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine("\nException Caught!");
            //    Debug.WriteLine("Message :{0} ", e.Message);
            //}

            return new FitbitActivityDataDto();
        }

        public async Task<bool> RefreshFitbitToken(int accountId, int integrationId)
        {
            using var client = new HttpClient();

            var isSucessful = false;

            var savedIntegration =
                await _integrationDataService.GetLinkedIntegrationByIntegrationIdAsync(accountId,
                    integrationId);

            if (savedIntegration != null)
            {
                var combinedString = _fitbitSettings.ClientId + ":" + _fitbitSettings.ClientSecret;

                var base64EncodedSecret = Base64Encode(combinedString);

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedSecret);

                var parameters = new Dictionary<string, string>
                {
                    {"grant_type", "refresh_token"}, {"refresh_token", savedIntegration.RefreshToken}
                };

                var encodedContent = new FormUrlEncodedContent(parameters);

                try
                {
                    var response = await client.PostAsync("https://api.fitbit.com/oauth2/token", encodedContent);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var callbackDto = JsonConvert.DeserializeObject<FitbitCallbackDto>(responseBody);

                    if (callbackDto != null)
                    {
                        savedIntegration.AccessToken = callbackDto.Access_Token;
                        savedIntegration.RefreshToken = callbackDto.Refresh_Token;

                        await _integrationDataService.UpdateAsync(savedIntegration);

                        isSucessful = true;
                    }
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine("\nException Caught!");
                    Debug.WriteLine("Message :{0} ", e.Message);
                }
            }

            return isSucessful;
        }

        private string GenerateFitbitAuthenticationUrl(string callBackUrl, List<string> scopes, int accountId)
        {
            var authUrl = "https://www.fitbit.com/oauth2/authorize?response_type=code&client_id=" +
                          _fitbitSettings.ClientId;

            var callBack = new UriBuilder(callBackUrl);

            authUrl = authUrl + "&redirect_uri=" + callBack.Uri;

            authUrl += "&scope=";

            var isFirstScope = true;

            foreach (var scope in scopes)
                if (isFirstScope)
                {
                    authUrl += scope;

                    isFirstScope = false;
                }
                else
                {
                    authUrl = authUrl + "%20" + scope;
                }

            authUrl += "&expires_in=604800";

            authUrl += "&state=" + accountId;

            return authUrl;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}