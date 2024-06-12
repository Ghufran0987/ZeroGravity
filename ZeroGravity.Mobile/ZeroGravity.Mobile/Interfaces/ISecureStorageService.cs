using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ISecureStorageService
    {
        /// <summary>
        /// Stores an encrypted object into the <see cref="SecureStorage"/> for the given key.
        /// </summary>
        /// <typeparam name="T">The type of the object to store.</typeparam>
        /// <param name="key">The key of the key/value pair.</param>
        /// <param name="value">The value of the key/value pair.</param>
        Task SaveObject<T>(string key, T value) where T : class;

        /// <summary>
        /// Stores an encrypted string into the <see cref="SecureStorage"/> for the given key.
        /// </summary>
        /// <param name="key">The key of the key/value pair.</param>
        /// <param name="value">The value of the key/value pair.</param>
        Task SaveString(string key, string value);

        /// <summary>
        /// Stores an encrypted value into the <see cref="SecureStorage"/> for the given key.
        /// </summary>
        /// <typeparam name="T">The struct type of the value to store.</typeparam>
        /// <param name="key">The key of the key/value pair.</param>
        /// <param name="value">The value of the key/value pair.</param>
        Task SaveValue<T>(string key, T value) where T : struct;


        /// <summary>
        /// Retrieves an object from the <see cref="SecureStorage"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="key">The key of the key/value pair.</param>
        /// <returns>The decrypted value of the key/value pair as an object of type T.</returns>
        Task<T> LoadObject<T>(string key) where T : class;

        /// <summary>
        /// Retrieves a string from the <see cref="SecureStorage"/>.
        /// </summary>
        /// <param name="key">The key of the key/value pair.</param>
        /// <returns>The decrypted value of the key/value pair as a string.</returns>
        Task<string> LoadString(string key);

        /// <summary>
        /// Retrieves a value from the <see cref="SecureStorage"/>.
        /// </summary>
        /// <typeparam name="T">The struct type of the value to retrieve.</typeparam>
        /// <param name="key">The key of the key/value pair.</param>
        /// <returns>The decrypted value of the key/value pair as a struct of type T.</returns>
        Task<T> LoadValue<T>(string key) where T : struct;


        /// <summary>
        /// Removes the encrypted key/value pair for the given key.
        /// </summary>
        /// <param name="key">The key of the key/value pair</param>
        /// <returns>Returns if the key/value pair was removed.</returns>
        Task<bool> Remove(string key);

        /// <summary>
        /// Removes all the stored encrypted key/value pairs.
        /// </summary>
        /// <returns></returns>
        Task RemoveAll();
    }
}
