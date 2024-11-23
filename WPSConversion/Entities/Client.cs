using System;
using System.ComponentModel;
using System.Linq;

namespace WPSConversion.Entities
{
    public class Client : EntityBase<Client>
    {
        private string _Name;
        /// <summary>Company Name</summary>
        public string Name { get => _Name; set => SetField(ref _Name, value?.Trim(), nameof(Name)); }

        private string _AddressLine1;
        /// <summary>Address - Line 1</summary>
        public string AddressLine1 { get => _AddressLine1; set => SetField(ref _AddressLine1, value?.Trim(), nameof(AddressLine1)); }

        private string _AddressLine2;
        /// <summary>Address - Line 2</summary>
        public string AddressLine2 { get => _AddressLine2; set => SetField(ref _AddressLine2, value?.Trim(), nameof(AddressLine2)); }

        private string _City;
        /// <summary>City</summary>
        public string City { get => _City; set => SetField(ref _City, value?.Trim(), nameof(City)); }

        private string _Province;
        /// <summary>Province Code</summary>
        public string Province { get => _Province; set => SetField(ref _Province, value?.Trim(), nameof(Province)); }

        private string _PostalCode;
        /// <summary>Postal Code</summary>
        public string PostalCode { get => _PostalCode; set => SetField(ref _PostalCode, value?.Trim(), nameof(PostalCode)); }

        private int _LicenceCount;
        /// <summary>LicenceCount</summary>
        public int LicenceCount { get => _LicenceCount; set => SetField(ref _LicenceCount, value, nameof(LicenceCount)); }

        private DateTime _RenewalDate;
        /// <summary>RenewalDate</summary>
        public DateTime RenewalDate { get => _RenewalDate; set => SetField(ref _RenewalDate, value, nameof(RenewalDate)); }

        public BindingList<Contact> Contacts { get; }

        public Client()
        {
            RenewalDate = DateTime.Today.AddYears(1);
            Contacts = new BindingList<Contact>();
        }

        protected override void MatchPropertiesInternal(Client fromEntity)
        {
            if (fromEntity == null)
            {
                return;
            }

            Name = fromEntity.Name;
            AddressLine1 = fromEntity.AddressLine1;
            AddressLine2 = fromEntity.AddressLine2;
            City = fromEntity.City;
            Province = fromEntity.Province;
            PostalCode = fromEntity.PostalCode;
            LicenceCount = fromEntity.LicenceCount;
            RenewalDate = fromEntity.RenewalDate;

            Contacts.Clear();

            foreach (Contact contact in fromEntity.Contacts)
            {
                Contacts.Add(contact.Clone());
            }
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(AddressLine1))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(City))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Province))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(PostalCode))
            {
                return false;
            }

            if (LicenceCount < 0)
            {
                return false;
            }

            if (Contacts.Any(x => !x.Validate()))
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
