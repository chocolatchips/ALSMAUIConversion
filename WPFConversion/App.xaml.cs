using WPFConversion.Views;

namespace WPFConversion
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new(new ClientView());

            //Width requires extra 20px to not reducce button size
            int width = 820;
            int height = 500;

            window.Height = height;
            window.Width = width;

            return window;
        }
    }
}