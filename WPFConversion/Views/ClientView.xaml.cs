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
            SetAlerts();
            InitializeComponent();
        }

        private void SetAlerts()
        {
            ((ClientViewModel)ViewModel).AlertTriggered += async (title, message) =>
            {
                await DisplayAlert(title, message, "OK");
            };
            ((ClientViewModel)ViewModel).VerificationTriggered += async (title, message) =>
            {
                return await DisplayAlert(title, message, "Yes", "No");
            };
        }

        public void Close()
        {
            Application.Current?.Quit();
        }

        public bool? ShowDialog()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles unfocuded event for contact entry control in view.
        /// Verifies binding matches last contact in EditClient's ContactList
        /// to check if new client needs to be added. Intended to simulate the
        /// functionality of WPF DataGrid control.
        /// </summary>
        /// <param name="sender">Source of event, <see cref="Entry"/> expected.</param>
        /// <param name="e">Event arguments for focus change.</param>
        private void ContactEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (ViewModel is not ClientViewModel viewModel)
                return;
            if (sender is not Entry entry)
                return;

            if (entry?.BindingContext == viewModel.EditClient.Contacts.Last())
                viewModel.ContactAltered();
        }
    }
}
