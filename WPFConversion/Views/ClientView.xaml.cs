using System.Windows;
using WPFConversion.ViewModels;

namespace WPFConversion.Views
{
    /// <summary>
    /// Logique d'interaction pour ClientView.xaml
    /// </summary>
    public partial class ClientView : ContentPage, IView
    {
        public IViewModel ViewModel { get => BindingContext as IViewModel; set => BindingContext = value; }
        public ClientView()
        {
            ViewModel = new ClientViewModel(this);
            InitializeComponent();
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
