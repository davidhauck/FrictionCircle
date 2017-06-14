using AForge.Math;
using FrictionCircle.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FrictionCircle.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Vector3 _carRightVector;
        private Vector3 _carForwardVector;

        private Vector3 _gravityVector;
        private Queue<Vector3> _lastVectors;

        const int NumVectorsToSmooth = 5;

        private float _xAcc;

        public float XAcc
        {
            get { return _xAcc; }
            set
            {
                _xAcc = value;
                RaisePropertyChanged();
            }
        }

        private float _yAcc;

        public float YAcc
        {
            get { return _yAcc; }
            set
            {
                _yAcc = value;
                RaisePropertyChanged();
            }
        }

        private IAccelerometerDependency _accelerometerService;

        public MainViewModel()
        {
            _lastVectors = new Queue<Vector3>();

            _accelerometerService = DependencyService.Get<IAccelerometerDependency>();
            _accelerometerService.AccelerationChanged += OnAccelerationChanged;
            _accelerometerService.StartListening();
        }

        private void OnAccelerationChanged(object sender, Models.AccelerometerEvent e)
        {
            if (_lastVectors.Count > NumVectorsToSmooth)
                _lastVectors.Dequeue();
            _lastVectors.Enqueue(new Vector3(e.XAcc, e.YAcc, e.ZAcc));

            Vector3 actualAcc = GetAverage(_lastVectors);
            actualAcc.X = actualAcc.X - _gravityVector.X;
            actualAcc.Y = actualAcc.Y - _gravityVector.Y;
            actualAcc.Z = actualAcc.Z - _gravityVector.Z;

            float angleToRight = AngleBetweenVectors(actualAcc, _carRightVector);
            float magnitudeRight = (float)Math.Cos(angleToRight) * actualAcc.Norm;

            float angleToStraight = AngleBetweenVectors(actualAcc, _carForwardVector);
            float magnitudeStraight = (float)Math.Cos(angleToStraight) * actualAcc.Norm;

            XAcc = -magnitudeRight;
            YAcc = magnitudeStraight;
        }

        public ICommand CalibrateCommand
        {
            get
            {
                return new Command(() =>
                {
                    _gravityVector = GetAverage(_lastVectors);
                    
                    Vector3 phoneForwardVector = new Vector3(0, 0, 1);
                    _carRightVector = Vector3.Cross(_gravityVector, phoneForwardVector);
                    _carForwardVector = Vector3.Cross(_carRightVector, _gravityVector);
                });
            }
        }

        private float AngleBetweenVectors(Vector3 vec1, Vector3 vec2)
        {
            return (float)Math.Acos(Vector3.Dot(vec1, vec2) / (vec1.Norm * vec2.Norm));
        }

        private Vector3 GetAverage(IEnumerable<Vector3> vectors)
        {
            float x = vectors.Average(v => v.X);
            float y = vectors.Average(v => v.Y);
            float z = vectors.Average(v => v.Z);
            return new Vector3(x, y, z);
        }
    }
}
