using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.NewDesign;
using NUnit.Framework;

namespace ECS.Test.Unit
{
    public class FakeTempSensor : ITempSensor
    {
        public int Temp { get; set; }
        public int GetTemp()
        {
            return Temp;
        }
    }

    public class FakeHeater : IHeater
    {
        public bool TurnOnHasBeenCalled;
        public bool TurnOffHasBeenCalled;

        public FakeHeater()
        {
            TurnOffHasBeenCalled = false;
            TurnOnHasBeenCalled = false;
        }
        
        public void TurnOn()
        {
            TurnOnHasBeenCalled = true;
        }

        public void TurnOff()
        {
            TurnOffHasBeenCalled = true;
        }
    }

    public class FakeWindow : IWindow
    {
        public bool WindowIsOpen;
        public bool WindowIsClosed;

        public FakeWindow()
        {
            WindowIsOpen = false;
            WindowIsClosed = false;
        }

        public void Open()
        {
            WindowIsOpen = true;
        }

        public void Close()
        {
            WindowIsClosed = true;
        }
    }

    [TestFixture]
    public class ECSUnitTest
    {
        #region Ctor
        [Test]
        [TestCase(20,20)]
        [TestCase(25, 20)]
        [TestCase(20, 25)]
        public void Ctor_ConstructorIsCalled_WindowThresholdIsLargerOrEqualToHeaterThreshold(int winThr, int heatThr)
        {
            var ctorUut = new NewDesign.ECS(heatThr,winThr,null,null,null);
            Assert.That(ctorUut.GetHeaterThreshold() <= ctorUut.GetWindowThreshold());
        }
        #endregion

        #region Regulate (Heater)
        [Test]
        public void Regulate_TempIsLowerThanThr_HeaterTurnsOn()
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            var uut = new NewDesign.ECS(0,0,fakeTempSensor,fakeHeater,fakeWindow);

            fakeTempSensor.Temp = 20;
            uut.SetHeaterThreshold(25);
            uut.Regulate();
            Assert.That(fakeHeater.TurnOnHasBeenCalled==true);
        }

        [Test]
        public void Regulate_TempIsEqualToThr_HeaterTurnsOff()
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            var uut = new NewDesign.ECS(0, 0, fakeTempSensor, fakeHeater, fakeWindow);

            fakeTempSensor.Temp = 25;
            uut.SetHeaterThreshold(25);
            uut.Regulate();
            Assert.That(fakeHeater.TurnOffHasBeenCalled == true);
        }

        [Test]
        public void Regulate_TempIsHigherThanThr_HeaterTurnsOff()
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            var uut = new NewDesign.ECS(0, 0, fakeTempSensor, fakeHeater, fakeWindow);

            fakeTempSensor.Temp = 30;
            uut.SetHeaterThreshold(25);
            uut.Regulate();
            Assert.That(fakeHeater.TurnOffHasBeenCalled == true);
        }

        #endregion

        #region Regulate (Window)
        [Test]
        public void Regulate_TempIsLowerThanThr_WindowCloses()
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            var uut = new NewDesign.ECS(0, 0, fakeTempSensor, fakeHeater, fakeWindow);

            fakeTempSensor.Temp = 20;
            uut.SetWindowThreshold(25);
            uut.Regulate();
            Assert.That(fakeWindow.WindowIsClosed==true);
        }

        [Test]
        public void Regulate_TempIsEqualToThr_WindowCloses()
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            var uut = new NewDesign.ECS(0, 0, fakeTempSensor, fakeHeater, fakeWindow);

            fakeTempSensor.Temp = 25;
            uut.SetWindowThreshold(25);
            uut.Regulate();
            Assert.That(fakeWindow.WindowIsClosed == true);
        }

        [Test]
        public void Regulate_TempIsHigherThanThr_WindowOpens()
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            var uut = new NewDesign.ECS(0, 0, fakeTempSensor, fakeHeater, fakeWindow);

            fakeTempSensor.Temp = 30;
            uut.SetWindowThreshold(25);
            uut.Regulate();
            Assert.That(fakeWindow.WindowIsOpen== true);
        }
        #endregion

        #region SetHeaterThreshold
        [Test]
        [TestCase(20, 25)]
        [TestCase(25, 25)]
        [TestCase(30, 25)]
        public void SetHeaterThreshold_SetterCalled_WindowThrIsAlwaysEqualToOrGreaterThanHeatThr(int winThr, int heatThr)
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            var uut = new NewDesign.ECS(0, 0, fakeTempSensor, fakeHeater, fakeWindow);

            uut.SetWindowThreshold(winThr);
            uut.SetHeaterThreshold(heatThr);

            Assert.That(uut.GetHeaterThreshold()<=uut.GetWindowThreshold());
        }

        #endregion

        #region SetWindowThreshold

        [Test]
        [TestCase(20, 25)]
        [TestCase(25, 25)]
        [TestCase(30, 25)]
        public void SetWindowThreshold_SetterCalled_WindowThrIsAlwaysEqualToOrGreaterThanHeatThr(int winThr, int heatThr)
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            var uut = new NewDesign.ECS(0, 0, fakeTempSensor, fakeHeater, fakeWindow);

            uut.SetHeaterThreshold(heatThr);
            uut.SetWindowThreshold(winThr);

            Assert.That(uut.GetHeaterThreshold() <= uut.GetWindowThreshold());
        }
        #endregion

        #region GetCurTemp

        [Test]
        public void GetCurTemp_GetCurTempIsCalled_Returns15()
        {
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var fakeTempSensor = new FakeTempSensor();
            fakeTempSensor.Temp = 15;
            var uut = new NewDesign.ECS(0, 0, fakeTempSensor, fakeHeater, fakeWindow);

            Assert.That(uut.GetCurTemp() == 15);
        }
        #endregion

    }
}
