using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace ZeroGravity.Mobile.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.            
            try
            {

                UIApplication.Main(args, null, "AppDelegate");
                // Sentry.SentrySdk.CaptureMessage("hEllo");
                // Crashes.GenerateTestCrash();
                // throw new Exception();
            }
            catch (Exception ex)
            {
                // Sentry.SentrySdk.CaptureException(ex);
                Crashes.TrackError(ex);
            }
          
        }
    }
}
