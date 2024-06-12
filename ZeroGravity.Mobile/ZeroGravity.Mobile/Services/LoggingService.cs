using System;
using System.Diagnostics;
using System.IO;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Logging.Helper;

namespace ZeroGravity.Mobile.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly string _logFilePath;

        public LoggingService(IStoragePathService storagePathService)
        {
            _logFilePath = storagePathService.GetStoragePath();
        }

        public void Log(string text)
        {
            var logPath = CreateFullLogFilePathname();

            CreateLogFile();

            try
            {
                // open file asynchronous (or create file, if it doesn't exist)
                // and add provided string
                // and close the file
                File.AppendAllText(logPath, text);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Creates the folder structure and an empty file (if no file exists).
        /// Also handles backing up logfiles to a backup file
        /// when the current logfile's size exceeds the <see cref="LoggingServiceHelper.MaxLogFileSize"/>.
        /// </summary>
        private void CreateLogFile()
        {
            var logFileFullPath = CreateFullLogFilePathname();
            var backupFileFullPath = CreateFullBackupLogFilePathname();

            try
            {
                bool dirExists = Directory.Exists(_logFilePath);

                if (!dirExists)
                {
                    // create folder structure
                    Directory.CreateDirectory(_logFilePath);
                }


                bool logFileExists = File.Exists(logFileFullPath);

                if (!logFileExists)
                {
                    // create empty logfile
                    using (FileStream fs = File.Create(logFileFullPath))
                    {
                        fs.Close();
                    }
                }


                var logfileInfo = new FileInfo(logFileFullPath);

                // check size
                var fileSize = logfileInfo.Length;

                if (fileSize >= LoggingServiceHelper.MaxLogFileSize)
                {
                    bool backupFileExists = File.Exists(backupFileFullPath);

                    if (backupFileExists)
                    {
                        File.Delete(backupFileFullPath);
                    }

                    File.Move(logFileFullPath, backupFileFullPath);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        private string CreateFullLogFilePathname()
        {
            return Path.Combine(_logFilePath, LoggingServiceHelper.LogFileName);
        }

        private string CreateFullBackupLogFilePathname()
        {
            return Path.Combine(_logFilePath, LoggingServiceHelper.BackupLogFileName);
        }
    }
}
