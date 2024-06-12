using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Prism.Events;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Exceptions.Messages;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Services.Communication
{
    public class ApiService : IApiService
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IEventAggregator _eventAggregator;
        private const string _contentType = "application/json";

        public ApiService(ILoggerFactory loggerFactory, ITokenService tokenService, IEventAggregator eventAggregator)
        {
            _tokenService = tokenService;
            _logger = loggerFactory?.CreateLogger<ApiService>() ?? new NullLogger<ApiService>();
            _eventAggregator = eventAggregator;
        }

        #region HTTP Post

        public async Task<Rx> PostJsonAsyncRx<Tx, Rx>(string url, Tx data, CancellationToken cancellationToken = new CancellationToken(), JsonSerializerSettings serializerSettings = null)
        {
            var json = serializerSettings != null ? JsonConvert.SerializeObject(data, serializerSettings) : JsonConvert.SerializeObject(data);
            Debug.WriteLine("URL: " + url + " JSON :" + json);
            var jsonBin = Encoding.UTF8.GetBytes(json);

            var result = await PostRxInternal<Rx>(url, cancellationToken, _contentType, jsonBin, serializerSettings);
            return result;
        }

        public async Task<Rx> PostRx<Rx>(string url, CancellationToken cancellationToken = new CancellationToken())
        {
            var rx = await PostRxInternal<Rx>(url, cancellationToken);
            return rx;
        }

        private async Task<Rx> PostRxInternal<Rx>(string url, CancellationToken cancellationToken, string contentType = "", byte[] data = null, JsonSerializerSettings serializerSettings = null)
        {
            var response = await PostInternal(url, cancellationToken, contentType, data);
            var responseString = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("URL: " + url + " JSON :" + responseString);
            return serializerSettings == null
                ? JsonConvert.DeserializeObject<Rx>(responseString)
                : JsonConvert.DeserializeObject<Rx>(responseString, serializerSettings);
        }

        private async Task<HttpResponseMessage> PostInternal(string url, CancellationToken cancellationToken, string contentType = "", byte[] data = null)
        {
            HttpContent httpContent = null;
            if (data != null)
            {
                httpContent = new ByteArrayContent(data);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            }

            var client = CreateClient();

            string token = await _tokenService.GetJsonWebToken();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var refreshToken = await _tokenService.GetRefreshToken();

            if (refreshToken != null)
            {
                // add refreshToken to header
                client.DefaultRequestHeaders.Add("Cookie", $"refreshToken={refreshToken.Token}");
            }

            var response = await client.PostAsync(url, httpContent, cancellationToken);
            await ExtractRefreshTokenFromCookie(response);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ErrorMessage error = new ErrorMessage();

                if (content != null)
                {
                    error = JsonConvert.DeserializeObject<ErrorMessage>(content);
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _eventAggregator.GetEvent<RefreshTokenEvent>().Publish();
                }
                else
                {
                    _logger.LogError($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {error.Message}");
                    throw new HttpException($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}", $"{error.Message}");
                }
                _logger.LogError($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {error.Message}");
            }

            return response;
        }

        public async Task PostJsonAsync<Tx>(string url, Tx data, CancellationToken cancellationToken = new CancellationToken())
        {
            var json = JsonConvert.SerializeObject(data);
            var jsonBin = Encoding.UTF8.GetBytes(json);
            Debug.WriteLine("URL: " + url + " JSON :" + json);
            await Post(url, cancellationToken, _contentType, jsonBin);
        }

        private async Task Post(string url, CancellationToken cancellationToken, string contentType = "", byte[] data = null)
        {
            await PostInternal(url, cancellationToken, contentType, data);
        }

        #endregion HTTP Post

        #region HTTP Get

        public async Task<Rx> GetSingleJsonAsync<Rx>(string url, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var response = await GetInternal(url, cancellationToken);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(Rx);
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (!String.IsNullOrEmpty(result))
                    {
                        Debug.WriteLine("URL: " + url + " JSON :" + result);
                        var deserialized = JsonConvert.DeserializeObject<Rx>(result);
                        return deserialized;
                    }
                    else
                    {
                        return default(Rx);
                    }
                }
            }
            catch (Exception ex)
            {
                return default(Rx);
            }
        }

        public async Task<List<Rx>> GetAllJsonAsync<Rx>(string url, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var response = await GetInternal(url, cancellationToken);
                var result = await response.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(result))
                {
                    Debug.WriteLine("URL: " + url + " JSON :" + result);
                    var deserialized = JsonConvert.DeserializeObject<List<Rx>>(result);
                    return deserialized;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<HttpResponseMessage> GetInternal(string url, CancellationToken cancellationToken)
        {
            var client = CreateClient();

            string token = await _tokenService.GetJsonWebToken();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(url, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return response;
            }
            else if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ErrorMessage error = new ErrorMessage();

                if (content != null)
                {
                    Debug.WriteLine("URL: " + url + " JSON :" + content);
                    error = JsonConvert.DeserializeObject<ErrorMessage>(content);
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _eventAggregator.GetEvent<RefreshTokenEvent>().Publish();
                }
                else
                {
                    _logger.LogError($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {error.Message}");
                    throw new HttpException($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}", $"{error.Message}");
                }
                _logger.LogError($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {error.Message}");
            }

            return response;
        }

        #endregion HTTP Get

        #region HHTP Put

        public async Task<Rx> PutJsonAsyncRx<Tx, Rx>(string url, Tx data, CancellationToken cancellationToken = new CancellationToken(), JsonSerializerSettings serializerSettings = null)
        {
            var json = serializerSettings != null ? JsonConvert.SerializeObject(data, serializerSettings) : JsonConvert.SerializeObject(data);
            var jsonBin = Encoding.UTF8.GetBytes(json);

            Debug.WriteLine("URL: " + url + " JSON :" + json);

            var result = await PutRx<Rx>(url, cancellationToken, _contentType, jsonBin, serializerSettings);

            return result;
        }

        private async Task<Rx> PutRx<Rx>(string url, CancellationToken cancellationToken, string contentType = "", byte[] data = null, JsonSerializerSettings serializerSettings = null)
        {
            var response = await PutInternal(url, cancellationToken, contentType, data);

            var responseString = await response.Content.ReadAsStringAsync();

            return serializerSettings == null
                ? JsonConvert.DeserializeObject<Rx>(responseString)
                : JsonConvert.DeserializeObject<Rx>(responseString, serializerSettings);
        }

        private async Task<HttpResponseMessage> PutInternal(string url, CancellationToken cancellationToken, string contentType = "", byte[] data = null)
        {
            var client = CreateClient();

            string token = await _tokenService.GetJsonWebToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent httpContent = new ByteArrayContent(data);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await client.PutAsync(url, httpContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ErrorMessage error = new ErrorMessage();
                _logger.LogError($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {error.Message}");

                if (content != null)
                {
                    error = JsonConvert.DeserializeObject<ErrorMessage>(content);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _eventAggregator.GetEvent<RefreshTokenEvent>().Publish();
                }
                else
                {
                    _logger.LogError($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {error.Message}");
                    throw new HttpException($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}", $"{error.Message}");
                }
                _logger.LogError($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {error.Message}");
            }

            return response;
        }

        public async Task PutJsonAsync<Tx>(string url, Tx data, CancellationToken cancellationToken = new CancellationToken())
        {
            var json = JsonConvert.SerializeObject(data);
            var jsonBin = Encoding.UTF8.GetBytes(json);
            Debug.WriteLine("URL: " + url + " JSON :" + json);

            await Put(url, cancellationToken, _contentType, jsonBin);
        }

        private async Task Put(string url, CancellationToken cancellationToken, string contentType = "", byte[] data = null)
        {
            await PutInternal(url, cancellationToken, contentType, data);
        }

        #endregion HHTP Put

        #region HTTP Delete

        public async Task<Rx> DeleteAsyncRx<Rx>(string url, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await DeleteRxInternal<Rx>(url, cancellationToken);
            return result;
        }

        private async Task<Rx> DeleteRxInternal<Rx>(string url, CancellationToken cancellationToken)
        {
            var response = await DeleteInternal(url, cancellationToken);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("URL: " + url + " JSON :" + result);

            var deserialized = JsonConvert.DeserializeObject<Rx>(result);
            return deserialized;
        }

        public async Task DeleteAsync(string url, CancellationToken cancellationToken = new CancellationToken())
        {
            await DeleteInternal(url, cancellationToken);
        }

        private async Task<HttpResponseMessage> DeleteInternal(string url, CancellationToken cancellationToken)
        {
            var client = CreateClient();

            string token = await _tokenService.GetJsonWebToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("URL: " + url + " JSON :" + content);

                ErrorMessage error = new ErrorMessage();

                if (content != null)
                {
                    error = JsonConvert.DeserializeObject<ErrorMessage>(content);
                }

                _logger.LogError($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {error.Message}");
                throw new HttpException($"Http Response code: {response.StatusCode}, Reason: {response.ReasonPhrase}", $"{error.Message}");
            }

            return response;
        }

        #endregion HTTP Delete

        private HttpClient CreateClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();

            clientHandler.ServerCertificateCustomValidationCallback = delegate (HttpRequestMessage message, X509Certificate2 certificate2, X509Chain arg3, SslPolicyErrors sslErrors)
            {
#if DEBUG
                return true;
#else
                if (sslErrors != SslPolicyErrors.None)
                {
                    _logger.LogInformation($"{message.Method.Method}:{sslErrors}");
                    return false;
                }
                else
                {
                    return true;
                }
#endif
            };

            var client = new HttpClient(clientHandler);

            // use device language for now
            var deviceLanguage = CultureInfo.CurrentCulture.Name;

            // ToDo: wenn die Sprache auch in der App geändert werden können soll,
            // ToDo: muss der Accept-Language Header auf die vom Benutzer gewählte Sprache gesetzt werden
            client.DefaultRequestHeaders.Add("Accept-Language", deviceLanguage);

            return client;
        }

        private async Task ExtractRefreshTokenFromCookie(HttpResponseMessage responseMessage)
        {
            IEnumerable<string> cookies = responseMessage.Headers.SingleOrDefault(p => p.Key == "Set-Cookie").Value;
            var refreshTokenString = cookies?.FirstOrDefault(_ => _.Contains("refreshToken"));
            if (refreshTokenString != null)
            {
                string[] separators = { "refreshToken=", ";", "expires=", ";" };

                try
                {
                    var spliced = refreshTokenString.Split(separators, StringSplitOptions.None);
                    var refreshToken = spliced[1];
                    var expirationString = spliced[3];

                    _logger.LogDebug($"Extracted a new refreshToken: {refreshToken}");

                    var expirationDateTime = DateTime.Parse(expirationString);
                    RefreshToken rT = new RefreshToken { Token = refreshToken, Expiration = expirationDateTime };
                    await _tokenService.AddOrUpdateRefreshToken(rT);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                }
            }
        }
    }
}