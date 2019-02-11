namespace ECS.Refactored
{
    public interface IHeater : ISelfTest
    {
        void TurnOff();
        void TurnOn();
    }
}