using System.Windows;
using WPSConversion.ViewModels;

namespace WPSConversion.Views
{
    /// <summary>
    /// Logique d'interaction pour ClientView.xaml
    /// </summary>
    public partial class ClientView : Window, IView
    {
        public IViewModel ViewModel { get => DataContext as IViewModel; set => DataContext = value; }

        public ClientView()
        {
            InitializeComponent();

            ViewModel = new ClientViewModel(this);
        }

        public void Close()
        {
            Application.Current?.Quit();
        }

        public bool? ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}
