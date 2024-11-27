using Microsoft.Extensions.Options;
using Microsoft.Maui.Controls.Platform.Compatibility;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using WinFormConversion.Entities;
using WinFormConversion.Views;

namespace WinFormConversion.ViewModels
{
    public class EmployeeViewModel : ViewModelBase<EmployeeView>
    {
        public ObservableCollection<Employee> EmployeeList { get; private set; }

        private Employee? _SelectedEmployee;
        public Employee? SelectedEmployee
        {
            get => _SelectedEmployee;
            set
            {
                if (SetNotifyableProperty(ref _SelectedEmployee, value, nameof(SelectedEmployee)))
                {
                    EditingEmployee = value ?? null;
                    RaiseCanExecuteChanged();
                }
            }
        }

        private Employee? _EditingEmployee;
        public Employee? EditingEmployee
        {
            get => _EditingEmployee;
            set
            {
                if (EditingEmployee != null)
                {
                    // Disconnects Employee.AlertTriggered to ViewBaseModel.TriggerAlert
                    EditingEmployee.AlertTriggered -= async (title, message) =>
                    {
                        TriggerAlert(title, message);
                    };
                }
                SetNotifyableProperty(ref _EditingEmployee, value, nameof(EditingEmployee));
                if (EditingEmployee != null)
                {
                    // Connects Employee.AlertTriggered to ViewBaseModel.TriggerAlert allowing
                    // for EditEmployee to display alerts.
                    EditingEmployee.AlertTriggered += async (title, message) =>
                    {
                        TriggerAlert(title, message);
                    };

                }
            }
        }

        private bool _IsEditing;
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

        public EmployeeViewModel(EmployeeView view)
            : base(view)
        {
            SaveCommand = new DelegateCommand(Save, CanSave);
            NewCommand = new DelegateCommand(New, CanNew);
            EditCommand = new DelegateCommand(Edit, CanEdit);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
            CancelCommand = new DelegateCommand(Cancel, CanCancel);

            EmployeeList = new ObservableCollection<Employee>();
            IsEditing = false;
            }

        #region Save
        public DelegateCommand SaveCommand { get; }
        private bool CanSave() => IsEditing;
        private void Save()
        {
            try
            {
                if (EditingEmployee == null)
                    return;
                if (Validate())
                {
                    bool isNew = EditingEmployee.IsNew;
                    Employee employee;
                    

                    if (isNew)
                        employee = EditingEmployee.Clone();
                    else
                    {
                        SelectedEmployee.MatchProperties(EditingEmployee);
                        employee = SelectedEmployee;
                    }

                    employee.AcceptChanges();
                    SelectedEmployee = null;

                    if (isNew)
                    {
                        EmployeeList.Add(employee);
                    }

                    RaisePropertyChanged(nameof(EmployeeList));

                    IsEditing = false;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Save");
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
                SelectedEmployee = null;
                EditingEmployee = new Employee();
                IsEditing = true;
            }
            catch (Exception ex)
            {
                ShowError(ex, "New");
            }
        }
        #endregion

        #region Edit
        public DelegateCommand EditCommand { get; }
        private bool CanEdit() => !IsEditing && SelectedEmployee != null;
        private void Edit()
        {
            try
            {
                if (SelectedEmployee == null)
                    return;
                EditingEmployee = SelectedEmployee.Clone();
                EditingEmployee.AcceptChanges();
                IsEditing = true;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Edit");
            }
        }
        #endregion

        #region Delete
        public DelegateCommand DeleteCommand { get; }
        private bool CanDelete() => !IsEditing && SelectedEmployee != null;
        private async void Delete()
        {
            try
            {
                bool res = await VerifyDelete();
                if (res)
                {
                    EmployeeList.Remove(SelectedEmployee);

                    SelectedEmployee = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Delete");
            }
        }

        private async Task<bool> VerifyDelete()
        {
            return await TriggerVerificationAlert("Confirmation", $"Do you really want to delete \'{SelectedEmployee}\'");
        }
        #endregion
        
        #region Cancel
        public DelegateCommand CancelCommand { get; }
        private bool CanCancel() => IsEditing;
        private void Cancel()
        {
            try
            {
                EditingEmployee = SelectedEmployee;
                RaisePropertyChanged(nameof(EditingEmployee));
                IsEditing = false;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Cancel");
            }
        }
        #endregion


        private bool Validate()
        {
            if (!EditingEmployee.Validate())
            {
                return false;
            }
            if (EditingEmployee.IsNew)
            {
                if (EmployeeList.Any(x => x.Username.Equals(EditingEmployee.Username, StringComparison.InvariantCultureIgnoreCase)))
                {
                    TriggerAlert("Validation Error", $"Username '{EditingEmployee.ToString()}' already exists");
                    return false;
                }
            }
            return true;
        }

        private void RaiseCanExecuteChanged()
        {
            CancelCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            NewCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
