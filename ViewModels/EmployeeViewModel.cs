using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormConversion.Entities;

namespace WinFormConversion.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees;
        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                EmployeeSelected = value != null;
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
            IsEditing = true;
            IsEditing = false;
        }

        public void SaveEmployee(string firstName, string lastName, string userName)
        {
            Employee employee = new()
            { 
                FirstName = firstName,
                LastName = lastName,
                Username = userName
            };
            Employees.Add(employee);
        }

        public void DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public void EditEmployee()
        {
            IsEditing = true;
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CreateNewEmployee()
        {
            IsEditing = true;
        }
    }
}
