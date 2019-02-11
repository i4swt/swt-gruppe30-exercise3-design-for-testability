using System;
using Autofac;

namespace ECS.Refactored
{
    public class Application
    {
        public static IContainer Container;

        public static void Main(string[] args)
        {
            //Rewrote the main function to use dependency injection using Autofac
            //So create container builder and register the types to it
            var builder = new ContainerBuilder();
            builder.RegisterType<ECS>().As<IECS>(); //Notice the parameter less constructor is used to register ECS
            builder.RegisterType<Heater>().As<IHeater>();
            builder.RegisterType<Window>().As<IWindow>();
            builder.RegisterType<TempSensor>().As<ITempSensor>();
            Container = builder.Build();

            //The ECS type is resolved, and the dependencies are injected into it
            var ecs = Container.Resolve<IECS>(); 
            ecs.HeaterThreshold = 40;
            ecs.WindowThreshold = 10;

            ecs.Regulate();
            Console.WriteLine();

            ecs.HeaterThreshold = 50;

            ecs.Regulate();
            Console.WriteLine();

            ecs.WindowThreshold = 25;

            ecs.Regulate();
            Console.WriteLine();
        }
    }
}
