using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;

namespace DnDPartyManagerMobile.Behaviors;

public class RegexValidationBehavior : Behavior<Entry>
{
    public static readonly BindableProperty RegexPatternProperty =
        BindableProperty.Create(nameof(RegexPattern), typeof(string), typeof(RegexValidationBehavior), string.Empty);

    public static readonly BindableProperty ValidationTypeProperty =
        BindableProperty.Create(nameof(ValidationType), typeof(string), typeof(RegexValidationBehavior), string.Empty);

    public string RegexPattern
    {
        get => (string)GetValue(RegexPatternProperty);
        set => SetValue(RegexPatternProperty, value);
    }

    public string ValidationType
    {
        get => (string)GetValue(ValidationTypeProperty);
        set => SetValue(ValidationTypeProperty, value);
    }

    protected override void OnAttachedTo(Entry entry)
    {
        base.OnAttachedTo(entry);
        entry.TextChanged += OnEntryTextChanged;
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        base.OnDetachingFrom(entry);
        entry.TextChanged -= OnEntryTextChanged;
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        var entry = (Entry)sender;
        bool isValid = true;
        string newText = args.NewTextValue?.Replace(".", "") ?? "";

        if (!string.IsNullOrEmpty(newText))
        {
            isValid = Regex.IsMatch(newText, RegexPattern);

            if (isValid && ValidationType == "Ip")
            {
                // Форматируем текст, добавляя точки каждые 3 цифры
                string formattedText = FormatIpAddress(newText);
                isValid = ValidateIpAddress(formattedText);

                if (isValid && formattedText != entry.Text)
                {
                    // Обновляем текст без вызова бесконечного цикла
                    entry.TextChanged -= OnEntryTextChanged;
                    entry.Text = formattedText;
                    entry.CursorPosition = formattedText.Length;
                    entry.TextChanged += OnEntryTextChanged;
                }
            }
            else if (isValid && ValidationType == "Port")
            {
                isValid = int.TryParse(newText, out int port) && port >= 0 && port <= 65535;
            }
        }

        entry.TextColor = isValid ? Colors.Black : Colors.Red;
        entry.BackgroundColor = isValid ? Colors.White : Color.FromRgba(255, 0, 0, 0.1);
    }

    private string FormatIpAddress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Разбиваем на группы по 3 цифры
        string result = "";
        for (int i = 0; i < input.Length; i++)
        {
            result += input[i];
            if (i % 3 == 2 && i < input.Length - 1)
                result += ".";
        }
        return result;
    }

    private bool ValidateIpAddress(string formattedText)
    {
        if (string.IsNullOrEmpty(formattedText))
            return true;

        var parts = formattedText.Split('.');
        if (parts.Length > 4)
            return false;

        foreach (var part in parts)
        {
            if (!int.TryParse(part, out int num) || num < 0 || num > 255)
                return false;
        }
        return true;
    }
}