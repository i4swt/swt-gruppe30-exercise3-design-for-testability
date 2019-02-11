namespace ECS.Refactored
{
    public class ECS : IECS
    {
        private readonly ITempSensor _tempSensor;
        private readonly IHeater _heater;
        private readonly IWindow _window;
        private int _windowThreshold;
        private int _heaterThreshold;

        public int HeaterThreshold
        {
            get => _heaterThreshold;
            set
            {
                if (value > WindowThreshold)
                    _windowThreshold = value;

                _heaterThreshold = value;
            }
        }

        public int WindowThreshold
        {
            get => _windowThreshold;
            set
            {
                if (value < HeaterThreshold)
                    _heaterThreshold = value;

                _windowThreshold = value;
            }
        }

        //Only added to support use of Autofac (see note in Application.cs)
        public ECS(ITempSensor tempSensor, IHeater heater, IWindow window)
        {
            _tempSensor = tempSensor;
            _heater = heater;
            _window = window;
        }

        public ECS(int heaterThreshold, int windowThreshold, ITempSensor tempSensor, IHeater heater, IWindow window)
        {
            HeaterThreshold = heaterThreshold;
            WindowThreshold = windowThreshold;
            _tempSensor = tempSensor;
            _heater = heater;
            _window = window;
        }

        public void Regulate()
        {
            var currentTemp = _tempSensor.GetTemp();
            AdjustAllEquipment(currentTemp);
        }

        public int GetCurrentTemp()
        {
            return _tempSensor.GetTemp();
        }

        public bool RunSelfTest()
        {
            return _tempSensor.RunSelfTest() && _heater.RunSelfTest() && _window.RunSelfTest();
        }

        private void AdjustAllEquipment(int currentTemp)
        {
            AdjustHeater(currentTemp);
            AdjustWindow(currentTemp);
        }

        private void AdjustHeater(int currentTemp)
        {
            //Turn on heater if temp is too low
            if (currentTemp < HeaterThreshold)
                _heater.TurnOn();
            else
                _heater.TurnOff();
        }
        private void AdjustWindow(int currentTemp)
        {
            //Open window if temp is too high
            if (currentTemp > WindowThreshold)
                _window.OpenWindow();
            else
                _window.CloseWindow();
        }
    }
}
