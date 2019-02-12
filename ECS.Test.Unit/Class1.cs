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

    [TestFixture]
    public class ECSUnitTest
    {
        private FakeTempSensor ts;
        private FakeHeater fh;
        private NewDesign.ECS uut;

        [SetUp]
        public void SetUpTests()
        {
            ts=new FakeTempSensor();
            fh=new FakeHeater();
            uut = new NewDesign.ECS(10, ts, fh);
        }

        [Test]
        [TestCase(8,ExpectedResult = true)]
        [TestCase(12,ExpectedResult = false)]
        public bool Regulate_TestForTurnOnHasBeenCalled(int temp)
        {
            ts.Temp = temp;
            uut.Regulate();
            return fh.TurnOnHasBeenCalled;
        }

        [Test]
        [TestCase(8, ExpectedResult = false)]
        [TestCase(12, ExpectedResult = true)]
        public bool Regulate_TestForTurnOffHasBeenCalled(int temp)
        {
            ts.Temp = temp;
            uut.Regulate();
            return fh.TurnOffHasBeenCalled;
        }

        [Test]
        [TestCase(-15,ExpectedResult = -15)]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(15, ExpectedResult = 15)]
        public int GetCurTemp_ReturnTemp(int temp)
        {
            ts.Temp = temp;
            return uut.GetCurTemp();
        }
    }
}
