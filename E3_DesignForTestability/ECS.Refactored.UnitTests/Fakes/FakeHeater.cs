namespace ECS.Refactored.UnitTests.Fakes
{
    public class FakeHeater : IHeater
    {
        public bool HeaterIsTurnedOff { get; private set; }
        public bool HeaterIsTurnedOn { get; private set; }
        private bool _isPassingSelfTest;

        public FakeHeater()
        {
            
        }

        public FakeHeater(bool isPassingSelfTest)
        {
            _isPassingSelfTest = isPassingSelfTest;
        }

        public bool RunSelfTest()
        {
            return _isPassingSelfTest;
        }

        public void TurnOff()
        {
            HeaterIsTurnedOff = true;
        }

        public void TurnOn()
        {
            HeaterIsTurnedOn = true;
        }
    }
}