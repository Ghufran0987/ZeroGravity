namespace ZeroGravity.Mobile.Contract.Constants
{
    public static class VideoUploadSettings
    {
        public static double MaxVideoUploadSizeInMb = 300;

        public static string AzureBlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=miboko;AccountKey=eH6P6eTfXGmkDl/ESsDo7Eymsebx2sXP58K4CTgN37owAeYVfkbqcQmx6LQQwKD/OJ12Zc4rdJhe+V1svTWfaA==;EndpointSuffix=core.windows.net";

        // only small characters allowed!
        public static string AzureBlobStorageContainerName = "user-video";

        public static string MetaDataVideoTitleKey = "videotitle";
        public static string MetaDataVideoDescriptionKey = "videodescription";
        public static string MetaDataVideoLanguageKey = "videolanguage";
    }
}