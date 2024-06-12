using System;
using System.IO;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.iOS.Services
{
    public class StoragePathServiceIos : IStoragePathService
    {
        public string GetStoragePath()
        {
            var storageDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var storageFilePath = Path.Combine(storageDir);
            return storageFilePath;
        }
    }
}