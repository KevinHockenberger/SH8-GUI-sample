using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace SH8_Sample.ViewModels.Converters
{
  //any custom converters you want to use in your xaml
  public class BoolToBrushColorConverter : MarkupExtension, IValueConverter
  {
    private Brush TrueValue { get; set; } = new SolidColorBrush(System.Windows.Media.Colors.Red); // could be used directly instead of defining here but for consistency it is defined
    private Brush FalseValue { get; set; } = new SolidColorBrush(System.Windows.Media.Colors.Lime); // could be used directly instead of defining here but for consistency it is defined
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is not bool) return Binding.DoNothing;
      return (bool)value ? TrueValue : FalseValue;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == TrueValue) return true;
      if (value == FalseValue) return false;
      return Binding.DoNothing;
    }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }
  }
}
