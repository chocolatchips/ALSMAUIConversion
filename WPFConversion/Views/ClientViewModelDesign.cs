using System.Linq;
using WPFConversion.Entities;
using WPFConversion.ViewModels;
using Contact = WPFConversion.Entities.Contact;

namespace WPFConversion.Views
{
    internal class ClientViewModelDesign : ClientViewModel
    {
        public ClientViewModelDesign()
            : base(null)
        {
            ProvinceList = Province.Provinces;

            ClientList.Add(new Client()
            {
                Name = "ALS Geoanalytics",
                AddressLine1 = "2103 Dollarton Hwy",
                City = "Vancouver",
                Province = "BC",
                PostalCode = "V7H 0A7",
                LicenceCount = 3,
                RenewalDate = System.DateTime.Today.AddYears(1),
            });

            ClientList.Add(new Client()
            {
                Name = "ALS Geochemistry Montreal",
                AddressLine1 = "1650 50e Avenue, Lachine QC H8T 2V5",
                City = "Lachine",
                Province = "QC",
                PostalCode = "H8T 2V5",
                LicenceCount = 0,
                RenewalDate = System.DateTime.MinValue,
            });

            SelectedClient = ClientList.FirstOrDefault();
            if (SelectedClient!= null)
            {
                SelectedClient.Contacts.Add(new Contact() { FirstName = "Joshua", LastName = "McLean", Email = "joshua.mclean@alsglobal.com" });
                SelectedClient.Contacts.Add(new Contact() { FirstName = "Ludivine", LastName = "Cormier", Email = "ludivine.cormier@alsglobal.com" });
            }

            IsEditing = false;
        }
    }
}
