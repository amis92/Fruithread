using System;
using Windows.UI.Xaml.Data;

namespace Fruithread.Helpers
{
    public class MultiplierConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var intValue = (int) value;
            var mutliplier = int.Parse(parameter.ToString());
            return intValue*mutliplier;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}