using System;

namespace ZeroGravity.Mobile.Contract.Constants
{
    public static class Common
    {
#if !DEBUG

        // Production/Release
        // public static string ServerUrl = "https://d0d33ae3113a.ngrok.io";
        public static string ServerUrl = "https://miboko-api.azurewebsites.net";

      // public static string ServerUrl = "https://miboko-api-dev.azurewebsites.net";

#else
        // Debug

        // public static string ServerUrl = "https://d0d33ae3113a.ngrok.io";
      public static string ServerUrl = "https://miboko-api.azurewebsites.net";
        // public static string ServerUrl = "https://miboko-api-dev.azurewebsites.net";

#endif

        public static DateTime MetabolicHealthTrackingHistorySelectedDate = DateTime.MinValue.Date;
    }
}