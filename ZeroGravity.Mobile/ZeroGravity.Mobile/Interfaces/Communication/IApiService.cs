using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZeroGravity.Mobile.Interfaces.Communication
{
    public interface IApiService
    {
        /// <summary>
        /// Performs an asynchronous HTTP Post and returns a single object in a Task of Type Rx.
        /// </summary>
        /// <typeparam name="Tx">The type of data.</typeparam>
        /// <param name="url">The url to send data to.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="cancellationToken">The cancellation token to abort the request.</param>
        /// <returns>A single object in a Task of Type Rx.</returns>
        Task<Rx> PostJsonAsyncRx<Tx, Rx>(string url, Tx data, CancellationToken cancellationToken = new CancellationToken(), JsonSerializerSettings serializerSettings = null);

        /// <summary>
        /// Performs an asynchronous HTTP Post.
        /// </summary>
        /// <typeparam name="Tx">The type of data.</typeparam>
        /// <param name="url">The url to send data to.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="cancellationToken">The cancellation token to abort the request.</param>
        /// <returns>A single object in a Task of Type Rx.</returns>
        Task PostJsonAsync<Tx>(string url, Tx data, CancellationToken cancellationToken = new CancellationToken());

        Task<Rx> PostRx<Rx>(string url, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Performs an asynchronous HTTP Get and returns a single object in a Task of Type Rx.
        /// </summary>
        /// <typeparam name="Rx">The data type to be returned.</typeparam>
        /// <param name="url">The url to send data to.</param>
        /// <returns></returns>
        Task<Rx> GetSingleJsonAsync<Rx>(string url, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Performs an asynchronous HTTP Get and returns a list of objects in a Task of Type Rx.
        /// </summary>
        /// <typeparam name="Rx">The data type to be returned.</typeparam>
        /// <param name="url">The url to send data to.</param>
        /// <returns></returns>
        Task<List<Rx>> GetAllJsonAsync<Rx>(string url, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Performs an asynchronous HTTP Put.
        /// </summary>
        /// <typeparam name="Tx">The type of data being passed.</typeparam>
        /// <typeparam name="Rx">The type of data to be returned.</typeparam>
        /// <param name="url">The url to send data to.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="cancellationToken">The cancellation token to abort the request.</param>
        /// <returns>A single object in a Task of Type Rx.</returns>
        Task<Rx> PutJsonAsyncRx<Tx, Rx>(string url, Tx data, CancellationToken cancellationToken = new CancellationToken(), JsonSerializerSettings serializerSettings = null);

        /// <summary>
        /// Performs an asynchronous HTTP Put.
        /// </summary>
        /// <typeparam name="Tx">The type of data being passed.</typeparam>
        /// <param name="url">The url to send data to.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="cancellationToken">The cancellation token to abort the request.</param>
        Task PutJsonAsync<Tx>(string url, Tx data, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Performs an asynchronous HTTP Delete and returns a single object in a Task of Type Rx.
        /// </summary>
        /// <typeparam name="Rx"></typeparam>
        /// <param name="url">The url to send data to.</param>
        /// <param name="cancellationToken">The cancellation token to abort the request.</param>
        /// <returns></returns>
        Task<Rx> DeleteAsyncRx<Rx>(string url, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Performs an asynchronous HTTP Delete.
        /// </summary>
        /// <param name="url">The url to send data to.</param>
        /// <param name="cancellationToken">The cancellation token to abort the request.</param>
        /// <returns></returns>
        Task DeleteAsync(string url, CancellationToken cancellationToken = new CancellationToken());
    }
}
