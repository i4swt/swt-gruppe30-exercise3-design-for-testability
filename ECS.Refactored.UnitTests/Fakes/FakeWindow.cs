namespace ECS.Refactored.UnitTests.Fakes
{
    public class FakeWindow : IWindow
    {
        public bool WindowIsOpen { get; private set; }
        public bool WindowIsClosed { get; private set; }
        private bool _isPassingSelfTest;

        public FakeWindow()
        {
            
        }

        public FakeWindow(bool isPassingSelfTest)
        {
            _isPassingSelfTest = isPassingSelfTest;
        }

        public bool RunSelfTest()
        {
            return _isPassingSelfTest;
        }

        public void CloseWindow()
        {
            WindowIsClosed = true;
        }

        public void OpenWindow()
        {
            WindowIsOpen = true;
        }
    }
}