using System;

namespace ECS.Refactored
{
    internal class TempSensor : ITempSensor
    {
        private Random _generator = new Random();

        public int GetTemp()
        {
            return _generator.Next(-5, 45);
        }

        public bool RunSelfTest()
        {
            return true;
        }
    }
}