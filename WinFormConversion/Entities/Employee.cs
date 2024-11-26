using System.ComponentModel;

namespace WinFormConversion.Entities
{
    public class Employee : EntityBase<Employee>
    {
        /// <summary>First Name</summary>
        private string _FirstName;
        public string FirstName
        {
            get => _FirstName;
            set => SetField(ref _FirstName, value?.Trim(), nameof(FirstName));
        }

        /// <summary>Last Name</summary>
        private string _LastName;
        public string LastName
        {
            get => _LastName;
            set => SetField(ref _LastName, value?.Trim(), nameof(LastName));
        }

        /// <summary>Username (should be unique)</summary>
        private string _Username;
        public string Username
        {
            get => _Username;
            set => SetField(ref _Username, value?.Trim(), nameof(Username));
        }

        /// <summary>
        /// Creates a new instance of "Employee"
        /// </summary>
        public Employee() 
        {
            
        }

        protected override void MatchPropertiesInternal(Employee fromEntity)
        {
            if (fromEntity == null)
                return;

            FirstName = fromEntity.FirstName;
            LastName = fromEntity.LastName;
            Username = fromEntity.Username;
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                AlertTriggered?.Invoke("Validation Error", "First Name must not be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                AlertTriggered?.Invoke("Validation Error", "Last Name must not be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username))
            {
                AlertTriggered?.Invoke("Validation Error", "Username must not be empty.");
                return false;
            }
            return true;
        }

        public event Func<string, string, Task>? AlertTriggered;
        protected async Task TriggerAlert(string title, string message)
        {
            if (AlertTriggered != null)
                await AlertTriggered.Invoke(title, message);
        }


        public override string ToString()
        {
            return Username;
        }
    }
}
