using System;

namespace ECS.Refactored
{
    public class Window : IWindow
    {
        public void OpenWindow()
        {
            Console.WriteLine("Window is open");
        }

        public void CloseWindow()
        {
            Console.WriteLine("Window is closed");
        }

        public bool RunSelfTest()
        {
            return true;
        }
    }
}