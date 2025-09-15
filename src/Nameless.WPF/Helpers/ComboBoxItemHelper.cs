using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using Nameless.WPF.Configuration;
using Nameless.WPF.Resources;

namespace Nameless.WPF.Helpers;

public static class ComboBoxItemHelper {
    public static ComboBoxItem EmptyComboBoxItem => new() { Content = Strings.ComboBoxItem_Default_Select_Display_Text };

    public static ComboBoxItem Create<TEnum>(TEnum value)
        where TEnum : struct, Enum {
        return new ComboBoxItem {
            Content = GetTextFor(value),
            Tag = value
        };

        static string GetTextFor(TEnum value) {
            return value switch {
                Theme theme => theme.GetDisplayText(),
                _ => value.GetDescription()
            };
        }
    }

    public static bool TrySelect<TEnum>(ComboBoxItem[] items, TEnum value, [NotNullWhen(returnValue: true)] out ComboBoxItem? output)
        where TEnum : struct, Enum {
        output = items.FirstOrDefault(item => Equals(item.Tag, value));

        if (output is not null) {
            output.IsSelected = true;
        }

        return output is not null;
    }
}