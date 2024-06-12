using System;
using CoreMotion;
using Foundation;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.iOS.Services
{
    public class StepManagerService : IStepCounterService
    {
        private DateTime _resetTime;
        private CMPedometer _cmPedometer;
        //private NSOperationQueue _queue;
        //private CMStepCounter _stepCounter;
        public delegate void DailyStepCountChangedEventHandler(NSNumber stepCount);

        public void ForceUpdate()
        {
            //If the last reset date wasn't today then we should update this.
            if (_resetTime.Date.Day != DateTime.Now.Date.Day)
            {
                _resetTime = DateTime.Today; //Forces update as the day may have changed.
            }

            NSDate sMidnight = Helpers.DateHelpers.DateTimeToNSDate(_resetTime);

            //_queue = _queue ?? NSOperationQueue.CurrentQueue;

            //if (_stepCounter == null)
            //    _stepCounter = new CMStepCounter();

            //_stepCounter.QueryStepCount(sMidnight, NSDate.Now, _queue, DailyStepQueryHandler);

            if (_cmPedometer == null)
                _cmPedometer = new CMPedometer();
      
            _cmPedometer.QueryPedometerData(sMidnight, NSDate.Now, DailyStepQueryHandler);
        }

        public void StartCountingFrom(DateTime date)
        {
            _resetTime = date;
            ForceUpdate();
        }

        //private void DailyStepQueryHandler(nint stepCount, NSError error)
        //{
        //    DailyStepCountChanged?.Invoke(stepCount);

        //    Steps = (int) stepCount;
        //}

        private void DailyStepQueryHandler(CMPedometerData cmPedometerData, NSError error)
        {
            DailyStepCountChanged?.Invoke(cmPedometerData.NumberOfSteps);

            Steps = (int)cmPedometerData.NumberOfSteps;
        }

        private void Updater(nint stepCount, NSDate date, NSError error)
        {
            NSDate sMidnight = Helpers.DateHelpers.DateTimeToNSDate(_resetTime);
            _cmPedometer.QueryPedometerData(sMidnight, NSDate.Now, DailyStepQueryHandler);
        }

        public event DailyStepCountChangedEventHandler DailyStepCountChanged;

        public int Steps { get; set; }

        public int GetSteps()
        {
            NSDate sMidnight = Helpers.DateHelpers.DateTimeToNSDate(_resetTime);

            _cmPedometer.QueryPedometerData(sMidnight, NSDate.Now, DailyStepQueryHandler);

            return Steps;
        }

        public bool IsAvailable()
        {
            return CMPedometer.IsStepCountingAvailable;
        }

        public void InitSensorService()
        {
            ForceUpdate();
        }

        public void StopSensorService()
        {
            //throw new NotImplementedException();
        }
    }
}