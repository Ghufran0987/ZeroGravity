using System;
using System.Globalization;
using System.Linq;
using Android.Content;
using Android.Hardware;
using Android.Runtime;
using Xamarin.Forms;
using ZeroGravity.Mobile.Droid.Services;
using ZeroGravity.Mobile.Interfaces;
using Application = Android.App.Application;
using Object = Java.Lang.Object;

[assembly: Dependency(typeof(StepCounterService))]

namespace ZeroGravity.Mobile.Droid.Services
{
    public class StepCounterService : Object, IStepCounterService, ISensorEventListener
    {
        private SensorManager sManager;

        public new void Dispose()
        {
            sManager.UnregisterListener(this);
            sManager.Dispose();
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            Console.WriteLine("OnAccuracyChanged called");
        }

        public void OnSensorChanged(SensorEvent e)
        {
            Console.WriteLine(e.ToString());

            if (e.Values != null && int.TryParse(e.Values[0].ToString(CultureInfo.InvariantCulture), out int stepValue))
            {
                Steps = stepValue;
            }
        }

        public int Steps { get; set; }

        public void InitSensorService()
        {
            sManager = Application.Context.GetSystemService(Context.SensorService) as SensorManager;
            sManager?.RegisterListener(this, sManager.GetDefaultSensor(SensorType.StepCounter), SensorDelay.Normal);
        }

        public int GetSteps()
        {
            return Steps;
        }

        public bool IsAvailable()
        {
            return 
                Application.Context.PackageManager != null && Application.Context.PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureSensorStepCounter) 
                                                           && Application.Context.PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureSensorStepDetector);
        }
        public void StopSensorService()
        {
            sManager.UnregisterListener(this);
        }
    }
}