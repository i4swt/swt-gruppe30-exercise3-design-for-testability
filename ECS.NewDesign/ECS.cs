using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.NewDesign
{
    public class ECS
    {
        private int _threshold;
        private readonly ITempSensor _tempSensor;
        private readonly IHeater _heater;

        public ECS(int thr,ITempSensor tempSensor,IHeater heater)
        {
            SetThreshold(thr);
            _tempSensor = tempSensor;
            _heater = heater;
        }

        public void Regulate()
        {
            var t = _tempSensor.GetTemp();
            if (t < _threshold)
                _heater.TurnOn();
            else
                _heater.TurnOff();
        }

        public void SetThreshold(int thr)
        {
            _threshold = thr;
        }

        public int GetThreshold()
        {
            return _threshold;
        }

        public int GetCurTemp()
        {
            return _tempSensor.GetTemp();
        }
    }
}
