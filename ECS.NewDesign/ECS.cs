using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.NewDesign
{
    public class ECS
    {
        private int _heaterThreshold;
        private int _windowThreshold;
        private readonly ITempSensor _tempSensor;
        private readonly IHeater _heater;
        private readonly IWindow _window;

        public ECS(int heaterThr,int windowThr,ITempSensor tempSensor,IHeater heater, IWindow window)
        {
            SetHeaterThreshold(heaterThr);
            SetWindowThreshold(windowThr);
            _tempSensor = tempSensor;
            _heater = heater;
            _window = window;
        }

        public void Regulate()
        {
            var t = _tempSensor.GetTemp();
            AdjustHeater(t);
            AdjustWindow(t);
        }

        private void AdjustHeater(int temp)
        {
            if (temp < _heaterThreshold)
                _heater.TurnOn();
            else
                _heater.TurnOff();
        }

        private void AdjustWindow(int temp)
        {
            if (temp > _windowThreshold)
                _window.Open();
            else
                _window.Close();
        }

        public void SetHeaterThreshold(int thr)
        {
            if(thr>_windowThreshold)
                SetWindowThreshold(thr);

            _heaterThreshold = thr;
        }

        public void SetWindowThreshold(int thr)
        {
            if(thr<_heaterThreshold)
                SetHeaterThreshold(thr);

            _windowThreshold = thr;
        }

        public int GetHeaterThreshold()
        {
            return _heaterThreshold;
        }

        public int GetWindowThreshold()
        {
            return _windowThreshold;
        }

        public int GetCurTemp()
        {
            return _tempSensor.GetTemp();
        }
    }
}
