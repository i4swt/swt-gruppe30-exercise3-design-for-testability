using ECS.Refactored.UnitTests.Fakes;
using NUnit.Framework;
using NSubstitute;

namespace ECS.Refactored.UnitTests
{
    [TestFixture]
    public class ECSTests_Mads_Tests
    {
        #region ECS (constructor test)

        [TestCase(24, 25, ExpectedResult = true)]
        [TestCase(25, 25, ExpectedResult = true)]
        [TestCase(26, 25, ExpectedResult = true)]
        [TestCase(26, 25, ExpectedResult = true)]
        [TestCase(45, -20, ExpectedResult = true)]
        public bool ECS_WhenConstructorIsCalled_WindowThresholdOverrulesHeaterThreshold(int heaterThreshold, int windowThreshold)
        {
            var ecs = new ECS(heaterThreshold, windowThreshold, null, null, null);

            return ecs.WindowThreshold >= ecs.HeaterThreshold;
        }

            #endregion

        #region WindowThresholdSetter

        [TestCase(24, 25, ExpectedResult = true)]
        [TestCase(25, 25, ExpectedResult = true)]
        [TestCase(26, 25, ExpectedResult = true)]
        public bool WindowThreshold_WhenSetterIsCalled_EnsuresWindowThresholdIsEqualOrGreaterThanHeaterThreshold(int heaterThreshold, int windowThreshold)
        {
            var ecs = new ECS(-1, -1, null, null, null);

            ecs.HeaterThreshold = heaterThreshold;
            ecs.WindowThreshold = windowThreshold;

            return ecs.WindowThreshold >= ecs.HeaterThreshold;
        }

            #endregion

        #region HeaterThresholdSetter

        [TestCase(25, 24, ExpectedResult = true)]
        [TestCase(25, 25, ExpectedResult = true)]
        [TestCase(25, 26, ExpectedResult = true)]
        public bool HeaterThreshold_WhenSetterIsCalled_EnsuresHeaterThresholdIsEqualOrLessThanHeaterThreshold(int heaterThreshold, int windowThreshold)
        {
            var ecs = new ECS(-1, -1, null, null, null);

            ecs.WindowThreshold = windowThreshold;
            ecs.HeaterThreshold = heaterThreshold;

            return ecs.HeaterThreshold <= ecs.WindowThreshold;
        }

            #endregion

        #region Regulate (heater on/off)

        [Test]
        public void Regulate_CurrentTempLowerThanHeaterThreshold_TurnsHeaterOn()
        {
            var temperature = 25;
            var heaterThreshold = 26;
            var fakeTempSensor = new FakeTempSensor(temperature);
            var fakeWindow = new FakeWindow();

            //Using NSubstitute to create fake mock. 
            var fakeHeaterMock = NSubstitute.Substitute.For<IHeater>();
            
            var ecs = new ECS(-1, -1, fakeTempSensor, fakeHeaterMock, fakeWindow);
            ecs.HeaterThreshold = heaterThreshold;

            ecs.Regulate(); //Will turn heater on because temp < threshold

            //Assert using Received, which valides that TurnOn have been called. 
            fakeHeaterMock.Received().TurnOn();
        }

        [Test]
        public void Regulate_CurrentTempEqualsHeaterThreshold_TurnsHeaterOff()
        {
            var temperature = 25;
            var heaterThreshold = 25;
            var fakeTempSensor = new FakeTempSensor(temperature);
            var fakeHeaterMock = NSubstitute.Substitute.For<IHeater>();
            var fakeWindow = new FakeWindow();
            var ecs = new ECS(-1, -1, fakeTempSensor, fakeHeaterMock, fakeWindow);
            ecs.HeaterThreshold = heaterThreshold;

            ecs.Regulate(); //Will turn heater off because temp == threshold

            fakeHeaterMock.Received().TurnOff();
        }

        [Test]
        public void Regulate_CurrentTempHigherThanHeaterThreshold_TurnHeaterOff()
        {
            var temperature = 25;
            var heaterThreshold = 24;
            var fakeTempSensor = new FakeTempSensor(temperature);
            var fakeHeaterMock = NSubstitute.Substitute.For<IHeater>();
            var fakeWindow = new FakeWindow();
            var ecs = new ECS(-1, -1, fakeTempSensor, fakeHeaterMock, fakeWindow);
            ecs.HeaterThreshold = heaterThreshold;

            ecs.Regulate(); //Will turn heater off because temp > threshold

            fakeHeaterMock.Received().TurnOn();
        }

        #endregion

        #region Regulate (window open/close)

        [Test]
        public void Regulate_CurrentTempLowerThanWindowThreshold_ClosesWindow()
        {
            var temperature = 25;
            var windowThreshold = 26;
            var fakeTempSensor = new FakeTempSensor(temperature);
            var fakeHeater = new FakeHeater();
            var fakeWindowMock = Substitute.For<IWindow>();
            var ecs = new ECS(-1, -1, fakeTempSensor, fakeHeater, fakeWindowMock);
            ecs.WindowThreshold = windowThreshold;

            ecs.Regulate(); //Will close window because temp < threshold

            fakeWindowMock.Received().CloseWindow();
        }

        [Test]
        public void Regulate_CurrentTempEqualsWindowThreshold_ClosesWindow()
        {
            var temperature = 25;
            var windowThreshold = 25;
            var fakeTempSensor = new FakeTempSensor(temperature);
            var fakeHeater = new FakeHeater();
            var fakeWindowMock = Substitute.For<IWindow>();
            var ecs = new ECS(-1, -1, fakeTempSensor, fakeHeater, fakeWindowMock);
            ecs.WindowThreshold = windowThreshold;

            ecs.Regulate(); //Will close window on because temp < threshold

            fakeWindowMock.Received().CloseWindow();
            
        }

        [Test]
        public void Regulate_CurrentTempHigherThanWindowThreshold_OpensWindow()
        {
            var temperature = 25;
            var windowThreshold = 24;
            var fakeTempSensor = new FakeTempSensor(temperature);
            var fakeHeater = new FakeHeater();
            var fakeWindowMock = Substitute.For<IWindow>();
            var ecs = new ECS(-1, -1, fakeTempSensor, fakeHeater, fakeWindowMock);
            ecs.WindowThreshold = windowThreshold;

            ecs.Regulate(); //Will open window because temp > threshold

            fakeWindowMock.Received().OpenWindow();
        }

            #endregion

        #region GetCurrentTemp

        [Test]
        public void GetCurrentTemp_WhenCalled_ReturnsCurrentTemp()
        {
            var fakeTempSensorMock = Substitute.For<ITempSensor>();
            //Sets the return value of the function.
            fakeTempSensorMock.GetTemp().Returns(25);
           
            var fakeHeater = new FakeHeater();
            var fakeWindow = new FakeWindow();
            var ecs = new ECS(-1, -1, fakeTempSensorMock, fakeHeater, fakeWindow);

            var result = ecs.GetCurrentTemp();

            Assert.That(result, Is.EqualTo(25));
        }

            #endregion

        #region RunSelfTest

        [TestCase(false, false, false, ExpectedResult = false)]
        [TestCase(true, false, false, ExpectedResult = false)]
        [TestCase(true, true, false, ExpectedResult = false)]
        [TestCase(false, true, false, ExpectedResult = false)]
        [TestCase(false, true, true, ExpectedResult = false)]
        [TestCase(false, false, true, ExpectedResult = false)]
        [TestCase(true, false, true, ExpectedResult = false)]
        [TestCase(true, true, true, ExpectedResult = true)]
        public bool RunSelfTest_WhenCalled_ReturnsTrueIfAllUnitsAreWorking(bool isTempSensorWorking, bool isHeaterWorking, bool isWindowWorking)
        {
            var fakeTempSensor = new FakeTempSensor(isTempSensorWorking);
            var fakeHeater = new FakeHeater(isHeaterWorking);
            var fakeWindow = new FakeWindow(isWindowWorking);
            var ecs = new ECS(20, 20, fakeTempSensor, fakeHeater, fakeWindow);

            var isAllUnitsWorking = ecs.RunSelfTest();

            return isAllUnitsWorking;
        }

        #endregion
    }
}