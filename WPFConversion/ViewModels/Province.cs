using System.Collections.Generic;

namespace WPFConversion.ViewModels
{
    public class Province
    {
        public static List<Province> Provinces { get; } = new List<Province>()
        {
            new Province("Alberta", "AB"),
            new Province("British Columbia", "BC"),
            new Province("New Brunswick", "NB"),
            new Province("Newfoundland and Labrador", "NL"),
            new Province("Northwest Territories", "NT"),
            new Province("Nova Scotia", "NS"),
            new Province("Nunavut", "NU"),
            new Province("Manitoba", "MB"),
            new Province("Ontario", "ON"),
            new Province("Prince Edward Island", "PE"),
            new Province("Quebec", "QC"),
            new Province("Saskatchewan", "SK"),
            new Province("Yukon", "YT"),
        };

        public string Name { get; }

        public string Code { get; }

        public Province(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
