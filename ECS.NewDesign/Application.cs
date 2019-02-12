using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.NewDesign
{
    class Application
    {
        static void Main(string[] args)
        {
            var ecs = new ECS(28,25,new TempSensor(), new Heater(),new Window());

            ecs.Regulate();

            ecs.SetWindowThreshold(20);

            ecs.Regulate();
        }
    }
}
