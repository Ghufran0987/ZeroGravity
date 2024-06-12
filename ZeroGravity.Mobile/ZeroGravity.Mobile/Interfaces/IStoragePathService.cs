namespace ZeroGravity.Mobile.Interfaces
{
    public interface IStoragePathService
    {
        /// <summary>
        /// Gets to 'public external' storage path to write files into.
        /// </summary>
        /// <returns></returns>
        string GetStoragePath();
    }
}
