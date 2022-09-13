using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Data;
using System.Windows.Markup;


namespace CookPopularInstallerUI
{
    [MarkupExtensionReturnType(typeof(HorizontalAlignment))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanToHorizontalAligment : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool bl)
            {
                return bl ? HorizontalAlignment.Center : HorizontalAlignment.Right;
            }

            return HorizontalAlignment.Center;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
