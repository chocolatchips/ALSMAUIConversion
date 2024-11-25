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

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
