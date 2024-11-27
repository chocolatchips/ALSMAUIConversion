using WinFormConversion.Views;
namespace WinFormConversion
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new(new EmployeeView());
            window.Title = "Employee Management";
            window.Height = 400;
            window.Width = 600;
            return window; 
        }
    }
}