using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormConversion.Entities
{
    public class Employee : INotifyPropertyChanged
    {
        /// <summary>Used to ensure unique identifiers for each instance</summary>
        private static int _nextId = 1;

        /// <summary>Unique identifier</summary>
        public int Id { get; }

        /// <summary>First Name</summary>

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>Last Name</summary>
        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        /// <summary>Username (should be unique)</summary>
        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        /// <summary>
        /// Creates a new instance of "Employee" and sets a unique Id value
        /// </summary>
        public Employee()
        {
            Id = _nextId++;
        }

        public Employee Clone()
        {
            return new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                username = Username
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
