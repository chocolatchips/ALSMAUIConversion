using System.Collections.ObjectModel;
using System.ComponentModel;
using WinFormConversion.Entities;

namespace WinFormConversion.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees;

        private Employee? _selectedEmployee;
        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                EmployeeSelected = value != null;
                EditingEmployee = value ?? null;
            }
        }

        private Employee? _editingEmployee;
        public Employee? EditingEmployee
        {
            get => _editingEmployee;
            set
            {
                _editingEmployee = value;
                OnPropertyChanged(nameof(EditingEmployee));
            }
        }

        private bool _employeeSelected;
        public bool EmployeeSelected
        {
            get => _employeeSelected;
            set
            {
                _employeeSelected = value;
                OnPropertyChanged(nameof(EmployeeSelected));
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public EmployeeViewModel()
        {
            Employees = new();
            SaveEmployee("Chris", "Sheard", "Auto");
            SaveEmployee("Chris", "Sheard", "Second");
            IsEditing = false;
        }

        public string IsValidEmployee(string firstName, string lastName, string username)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return "First Name must not be empty";
            else if (string.IsNullOrWhiteSpace(lastName))
                return "Last Name must not be empty";
            else if (string.IsNullOrWhiteSpace(username))
                return "Username must not be empty";
            
            if (SelectedEmployee == null)
            {
                if (Employees.Any(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return $"Username '{username}' already exists.";
                }
            }
            else
            {
                if (Employees.Any(x => x.Id != SelectedEmployee.Id && x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return $"Username '{username}' already exists.";
                }
            }
            return string.Empty;
        }

        public void SaveEmployee(string firstName, string lastName, string username)
        {
            if (EmployeeSelected)
                SaveExistingEmployee();
            else
                SaveNewEmployee(firstName, lastName, username);
            
            IsEditing = false;
        }

        private void SaveNewEmployee(string firstName, string lastName, string username)
        {
            Employee employee = new()
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username
            };
            Employees.Add(employee);
        }

        private void SaveExistingEmployee()
        {
            if (SelectedEmployee != null && EditingEmployee != null)
            {
                SelectedEmployee.FirstName = EditingEmployee.FirstName;
                SelectedEmployee.LastName = EditingEmployee.LastName;
                SelectedEmployee.Username = EditingEmployee.Username;
            }
        }

        public void DeleteEmployee()
        {
            if (SelectedEmployee == null)
                return;

            Employees.Remove(SelectedEmployee);
        }

        public void EditEmployee()
        {
            IsEditing = true;
            if (SelectedEmployee != null)
                EditingEmployee = SelectedEmployee.Clone();
        }

        public void CreateNewEmployee()
        {
            IsEditing = true;
            SelectedEmployee = null;
        }

        public void CancelEdit()
        {
            if (EmployeeSelected)
                CancelEditExisting();
            else
                CancelEditNew();

            IsEditing = false;
        }

        private void CancelEditExisting()
        {
            if (SelectedEmployee != null && EditingEmployee != null)
            {
                EditingEmployee.FirstName = SelectedEmployee.FirstName;
                EditingEmployee.LastName = SelectedEmployee.LastName;
                EditingEmployee.Username = SelectedEmployee.Username;

            }
        }

        private void CancelEditNew()
        {
            SelectedEmployee = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
