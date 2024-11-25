using WPFConversion.Views;

namespace WPFConversion
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new ClientView();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            //Width requires extra 20px to not reducce button size
            int width = 820;
            int height = 500;

            window.Height = height;
            window.Width = width;

            return window;
            //return new Window(new AppShell());
        }
    }
}