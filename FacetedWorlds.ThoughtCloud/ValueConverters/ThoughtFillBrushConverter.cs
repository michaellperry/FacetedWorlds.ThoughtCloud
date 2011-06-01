using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FacetedWorlds.ThoughtCloud.ValueConverters
{
    public class ThoughtFillBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool inConflict = value as bool? ?? false;
            return Application.Current.Resources[inConflict ? "ConflictThoughtFillBrush" : "ThoughtFillBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
