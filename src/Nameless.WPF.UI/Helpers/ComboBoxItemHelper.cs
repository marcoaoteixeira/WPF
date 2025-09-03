using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UI.Helpers;

public static class ComboBoxItemHelper {
    public static ComboBoxItem EmptyComboBoxItem => new() { Content = Strings.ComboBoxItem_Default_Select };

    public static ComboBoxItem Create<TEnum>(TEnum value)
        where TEnum : struct, Enum {
        return new ComboBoxItem {
            Content = value.GetDescription(),
            Tag = value
        };
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