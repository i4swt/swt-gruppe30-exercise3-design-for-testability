namespace ECS.Refactored.UnitTests.Fakes
{
    public class FakeTempSensor : ITempSensor
    {
        private bool _isPassingSelfTest;
        private int _fixedCurrentTemp;

        public FakeTempSensor(bool isPassingSelfTest)
        {
            _isPassingSelfTest = isPassingSelfTest;
        }

        public FakeTempSensor(int fixedCurrentTemp)
        {
            _fixedCurrentTemp = fixedCurrentTemp;
        }

        public bool RunSelfTest()
        {
            return _isPassingSelfTest;
        }

        public int GetTemp()
        {
            return _fixedCurrentTemp;
        }
    }
}