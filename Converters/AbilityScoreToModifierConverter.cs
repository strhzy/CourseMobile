using System.Globalization;

namespace DnDPartyManagerMobile.Converters;

public class AbilityScoreToModifierConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int score)
        {
            return $"{(score - 10) / 2:+#;-#;0}";
        }
        return "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}