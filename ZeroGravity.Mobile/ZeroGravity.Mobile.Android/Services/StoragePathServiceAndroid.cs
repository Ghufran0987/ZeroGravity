using System;
using System.IO;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Logging.Helper;

namespace ZeroGravity.Mobile.Droid.Services
{
    public class StoragePathServiceAndroid : IStoragePathService
    {
        public string GetStoragePath()
        {
#if DEBUG
            // external storage - accessible by all apps, the user, and possibly other devices (requires permissions)
            var externalStoragePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments);
            var logFilePath = Path.Combine(externalStoragePath?.AbsolutePath ?? throw new InvalidOperationException(), LoggingServiceHelper.ParrentFolderName, LoggingServiceHelper.ChildFolderName);
            return logFilePath;

#else
            // internal storage - can be accessed only by the application or the operating system
            // and is not accessible from PC
            var internalAppStoragePath = Xamarin.Essentials.FileSystem.AppDataDirectory;
            var internalLogFilePath = Path.Combine(internalAppStoragePath, LoggingServiceHelper.ParrentFolderName, LoggingServiceHelper.ChildFolderName);
            return internalLogFilePath;
#endif
        }
    }
}