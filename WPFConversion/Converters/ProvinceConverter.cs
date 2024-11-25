using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFConversion.Entities;

namespace WPFConversion.Converters
{
    class ProvinceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && value is string pr)
            {
                return Province.Provinces.FirstOrDefault(x => x.Code.Equals(pr));
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && value is Province pr)
            {
                return pr.Code;
            }
            return null;
        }
    }
}
