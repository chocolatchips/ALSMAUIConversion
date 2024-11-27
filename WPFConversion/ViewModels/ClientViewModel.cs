using System.Collections.ObjectModel;
using WPFConversion.Entities;
using WPFConversion.Views;
using Contact = WPFConversion.Entities.Contact;

namespace WPFConversion.ViewModels
{
    public partial class ClientViewModel : ViewModelBase<ClientView>
    {
        private Client _originalClient;

        private Client _EditClient;
        /// <summary>EditClient</summary>
        public Client EditClient { get => _EditClient; private set => SetNotifyableProperty(ref _EditClient, value, nameof(EditClient)); }

        public ObservableCollection<Client> ClientList { get; private set; }

        private bool _IsEditing;
        /// <summary>IsEditing</summary>
        public bool IsEditing
        {
            get => _IsEditing;

            set
            {
                if (SetNotifyableProperty(ref _IsEditing, value, nameof(IsEditing)))
                {
                    RaiseCanExecuteChanged();
                }
            }
        }

        private Client _SelectedClient;
        /// <summary>SelectedClient</summary>
        public Client SelectedClient
        {
            get => _SelectedClient;

            set
            {
                if (SetNotifyableProperty(ref _SelectedClient, value, nameof(SelectedClient)))
                {
                    EditClient = SelectedClient;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public void ContactAltered()
        {
            if (ContactChanged(EditClient.Contacts.Last()))
                EditClient.Contacts.Add(new Contact());
        }

        private bool ContactChanged(Contact contact)
        {
            return !string.IsNullOrWhiteSpace(contact.FirstName)
                || !string.IsNullOrWhiteSpace(contact.LastName)
                || !string.IsNullOrWhiteSpace(contact.Email);
        }

        public List<Province> ProvinceList { get; protected set; }

        public ClientViewModel(ClientView view)
            : base(view)
        {
            CancelCommand = new DelegateCommand(Cancel, CanCancel);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
            EditCommand = new DelegateCommand(Edit, CanEdit);
            NewCommand = new DelegateCommand(New, CanNew);
            SaveCommand = new DelegateCommand(Save, CanSave);

            ClientList = new ObservableCollection<Client>();
            ProvinceList = Province.Provinces;
        }

        #region Cancel
        public DelegateCommand CancelCommand { get; }

        private bool CanCancel() => IsEditing;

        /// <summary>
        /// Cancels the edit process and undoes any changes to existing client.
        /// </summary>
        private async void Cancel()
        {
            try
            {
                bool res = await VerifyCancel();
                if (res)
                {
                    if (EditClient.IsDirty && !EditClient.IsNew)
                    {
                        SelectedClient.MatchProperties(_originalClient);
                    }

                    _originalClient = null;
                    EditClient = SelectedClient;

                    IsEditing = false;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Cancel");
            }
        }

        /// <summary>
        /// Triggers confirmation for user to verify cancellation process.
        /// Alerts user unsaved changes are lost.
        /// </summary>
        /// <returns>A <see cref="Task{Boolean}"/> representing async function.
        /// Returns true if user confirms cancellation; otherwise false.</returns>
        private Task<bool> VerifyCancel()
        {
            return TriggerVerificationAlert("Confirmation", "Are you sure you want to cancel editing? Any changes will be lost.");
        }
        #endregion

        #region Delete
        public DelegateCommand DeleteCommand { get; }

        private bool CanDelete() => !IsEditing && SelectedClient != null;

        /// <summary>
        /// Deletes current SelectedClient from ClientList.
        /// </summary>
        private async void Delete()
        {
            try
            {
                bool res = await VerifyDelete();
                if (res)
                {
                    ClientList.Remove(SelectedClient);

                    _originalClient = null;
                    SelectedClient = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Delete");
            }
        }

        /// <summary>
        /// Triggers confirmation for user to verify deletion of selected client.
        /// </summary>
        /// <returns>A <see cref="Task{Boolean}"/> representing async function.
        /// Returns true if user confirms cancellation; otherwise false.</returns>
        private Task<bool> VerifyDelete()
        {
            return TriggerVerificationAlert("Confirmation", $"Do you really want to delete \'{SelectedClient}\'");
        }
        #endregion

        #region Edit
        public DelegateCommand EditCommand { get; }

        private bool CanEdit() => !IsEditing && SelectedClient != null;

        /// <summary>
        /// Initiates the edit process for the selected client.
        /// Creates a copy of selected client to undo changes if required.
        /// </summary>
        private void Edit()
        {
            try
            {
                _originalClient = SelectedClient.Clone();
                EditClient = SelectedClient.Clone();
                EditClient.AcceptChanges();
                EditClient.Contacts.Add(new Contact());

                IsEditing = true;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Edit");
            }
        }
        #endregion

        #region New
        public DelegateCommand NewCommand { get; }

        private bool CanNew() => !IsEditing;

        /// <summary>
        /// Creates a new <see cref="Client"/> that can be edited by user.
        /// </summary>
        private void New()
        {
            try
            {
                _originalClient = null;
                SelectedClient = null;
                EditClient = new Client();
                EditClient.Contacts.Add(new Contact());

                IsEditing = true;
            }
            catch (Exception ex)
            {
                ShowError(ex, "New");
            }
        }
        #endregion

        #region Save
        public DelegateCommand SaveCommand { get; }

        private bool CanSave() => IsEditing;

        /// <summary>
        /// Initiates save process for client.
        /// Saves and ends editing if client is valid and has new unsaved changes;
        /// otherwise continues editing process.
        /// </summary>
        private void Save()
        {
            try
            {
                // Removes last contact in contact list. Contact should always be empty due to
                // ContactAltered(). Contact must be removed otherwise client cannot be valid.
                Contact lastContact = EditClient.Contacts.Last();
                EditClient.Contacts.RemoveAt(EditClient.Contacts.Count - 1);

                // Checks client for unsaved changed and validity.
                if (EditClient.IsDirty && Validate())
                {
                    bool isNew = EditClient.IsNew;
                    Client client;

                    if (isNew)
                    {
                        client = EditClient.Clone();
                    }
                    else
                    {
                        SelectedClient.MatchProperties(EditClient);
                        client = SelectedClient;
                    }

                    foreach (Contact contact in client.Contacts)
                    {
                        contact.ClientId = client.Id;
                        contact.AcceptChanges();
                    }

                    client.AcceptChanges();

                    _originalClient = null;
                    SelectedClient = null;

                    if (isNew)
                    {
                        ClientList.Add(client);
                    }

                    ClientList = new ObservableCollection<Client>(ClientList.OrderBy(x => x.Name).ToList());
                    RaisePropertyChanged(nameof(ClientList));

                    SelectedClient = client;
                    EditClient = SelectedClient;

                    IsEditing = false;
                }
                else
                {
                    // Adds new contact if save fails to return client to presave state. 
                    EditClient.Contacts.Add(lastContact);
                }

            }
            catch (Exception ex)
            {
                ShowError(ex, "Save");
            }
        }
        #endregion

        /// <summary>
        /// Raises CanExecuteChanged event for every command. Updates if command can execute.
        /// </summary>
        private void RaiseCanExecuteChanged()
        {
            CancelCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            NewCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Checks if editing client is valid. Triggers alerts if City is empty or
        /// Username is already used by another client.
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            if (!EditClient.Validate())
            {
                //  Checks to client for empty, null, or whitespace city. Triggers alert to notify user.
                if (string.IsNullOrWhiteSpace(EditClient.City))
                {
                    TriggerAlert("Validation Error", "City must not be empty");
                }
                return false;
            }

            if (EditClient.IsNew)
            {
                // Checks to ensure new client does not use same username as existing client.
                if (ClientList.Any(x => x.Name.Equals(EditClient.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    TriggerAlert("Validation Error", $"Company name {EditClient.Name} already exists");
                    return false;
                }
            }
            else
            {
                // Checks client list to ensure existing client does not change username to match another client.
                if (ClientList.Any(x => x.Id != SelectedClient.Id && x.Name.Equals(EditClient.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    TriggerAlert("Validation Error", $"Company name {EditClient.Name} already exists");
                    return false;
                }
            }

            return true;
        }
    }
}
