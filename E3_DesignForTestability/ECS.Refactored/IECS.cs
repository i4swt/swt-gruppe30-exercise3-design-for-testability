namespace ECS.Refactored
{
    public interface IECS
    {
        int HeaterThreshold { get; set; }
        int WindowThreshold { get; set; }

        int GetCurrentTemp();
        void Regulate();
        bool RunSelfTest();
    }
}