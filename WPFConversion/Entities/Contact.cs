namespace WPFConversion.Entities
{
    public class Contact : EntityBase<Contact>
    {

        private int _ClientId;
        /// <summary>ClientId</summary>
        public int ClientId { get => _ClientId; set => SetField(ref _ClientId, value, nameof(ClientId)); }

        private string _FirstName;
        /// <summary>First Name</summary>
        public string FirstName { get => _FirstName; set => SetField(ref _FirstName, value?.Trim(), nameof(FirstName)); }

        private string _LastName;
        /// <summary>LastName</summary>
        public string LastName { get => _LastName; set => SetField(ref _LastName, value?.Trim(), nameof(LastName)); }

        private string _Email;
        /// <summary>Email</summary>
        public string Email { get => _Email; set => SetField(ref _Email, value?.Trim(), nameof(Email)); }

        public Contact()
        {

        }

        protected override void MatchPropertiesInternal(Contact fromEntity)
        {
            if (fromEntity == null)
            {
                return;
            }

            FirstName = fromEntity.FirstName;
            LastName = fromEntity.LastName;
            Email = fromEntity.Email;
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks (FirstName, LastName, and Email) for non-whitespace characters. Returns if non-whitespace character found. 
        /// </summary>
        /// <returns>True if any of (FirstName, LastName, Email) are not null, empty, or whitespace; otherwise returns false</returns>
        public bool NotEmpty()
        {
            return !string.IsNullOrWhiteSpace(FirstName)
                || !string.IsNullOrWhiteSpace(LastName)
                || !string.IsNullOrWhiteSpace(Email);
        }

        public event Func<string, string, Task>? AlertTriggered;
        protected async Task TriggerAlert(string title, string message)
        {
            if (AlertTriggered != null)
                await AlertTriggered.Invoke(title, message);
        }


        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
