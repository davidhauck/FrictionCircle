using FrictionCircle.Dependencies;
using FrictionCircle.iOS.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;
using FrictionCircle.Models;
using CoreMotion;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(iOSAccelerometerDependency))]
namespace FrictionCircle.iOS.Dependencies
{
    public class iOSAccelerometerDependency : IAccelerometerDependency
    {
        private CMMotionManager _manager;

        public event EventHandler<AccelerometerEvent> AccelerationChanged;

        public void StartListening()
        {
            _manager = new CMMotionManager();
            _manager.StartAccelerometerUpdates(NSOperationQueue.CurrentQueue, (data, error) =>
            {
                AccelerometerEvent e = new AccelerometerEvent
                {
                    XAcc = (float)data.Acceleration.X,
                    YAcc = (float)data.Acceleration.Y,
                    ZAcc = -(float)data.Acceleration.Z,
                };
                AccelerationChanged?.Invoke(this, e);
            });
        }
    }
}
