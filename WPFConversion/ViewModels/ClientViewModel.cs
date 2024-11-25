using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
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

        private void Cancel()
        {
            try
            {
                if (EditClient.IsDirty && !EditClient.IsNew)
                {
                    SelectedClient.MatchProperties(_originalClient);
                }

                _originalClient = null;
                EditClient = SelectedClient;

                IsEditing = false;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Cancel");
            }
        }
        #endregion

        #region Delete
        public DelegateCommand DeleteCommand { get; }

        private bool CanDelete() => !IsEditing && SelectedClient != null;

        private void Delete()
        {
            try
            {
                ClientList.Remove(SelectedClient);

                _originalClient = null;
                SelectedClient = null;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Delete");
            }
        }
        #endregion

        #region Edit
        public DelegateCommand EditCommand { get; }

        private bool CanEdit() => !IsEditing && SelectedClient != null;

        private void Edit()
        {
            try
            {
                _originalClient = SelectedClient.Clone();
                EditClient = SelectedClient.Clone();
                EditClient.AcceptChanges();

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

        private void New()
        {
            try
            {
                _originalClient = null;
                SelectedClient = null;
                EditClient = new Client();

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

        private void Save()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                ShowError(ex, "Save");
            }
        }
        #endregion

        private void RaiseCanExecuteChanged()
        {
            CancelCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            NewCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool Validate()
        {
            if (!EditClient.Validate())
            {
                return false;
            }

            if (EditClient.IsNew)
            {
                if (ClientList.Any(x => x.Name.Equals(EditClient.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
            }
            else
            {
                if (ClientList.Any(x => x.Id != SelectedClient.Id && x.Name.Equals(EditClient.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
