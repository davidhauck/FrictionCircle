using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FrictionCircle.Dependencies;
using FrictionCircle.Models;
using Android.Hardware;
using FrictionCircle.Droid.Dependencies;

[assembly: Xamarin.Forms.Dependency (typeof(AndroidAccelerometerDependency))]
namespace FrictionCircle.Droid.Dependencies
{
    public class AndroidAccelerometerDependency : Java.Lang.Object, IAccelerometerDependency, ISensorEventListener
    {
        public event EventHandler<AccelerometerEvent> AccelerationChanged;

        private SensorManager _sensorManager;
        public void StartListening()
        {
            _sensorManager = (SensorManager)Application.Context.GetSystemService(Context.SensorService);
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Ui);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            //Ignore
        }

        public void OnSensorChanged(SensorEvent e)
        {
            var accelerometerEvent = new AccelerometerEvent();
            accelerometerEvent.XAcc = e.Values[0] / 9.81f;
            accelerometerEvent.YAcc = e.Values[1] / 9.81f;
            accelerometerEvent.ZAcc = e.Values[2] / 9.81f;
            AccelerationChanged?.Invoke(this, accelerometerEvent);
        }
    }
}