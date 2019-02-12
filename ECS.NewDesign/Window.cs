using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.NewDesign
{
    public interface IWindow
    {
        void Open();
        void Close();
    }

    public class Window : IWindow
    {
        public void Open()
        {
            Console.WriteLine("Window is open");
        }

        public void Close()
        {
            Console.WriteLine("Window is closed");
        }
    }
}
