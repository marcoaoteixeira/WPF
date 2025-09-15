using System.Windows.Controls;
using Nameless.WPF.Configuration;
using Nameless.WPF.Helpers;
using Wpf.Ui.Appearance;

namespace Nameless.WPF;

public static class ThemeExtension {
    public static ApplicationTheme ToApplicationTheme(this Theme self) {
        return self switch {
            Theme.Light => ApplicationTheme.Light,
            Theme.Dark => ApplicationTheme.Dark,
            Theme.HighContrast => ApplicationTheme.HighContrast,
            _ => ApplicationTheme.Light
        };
    }

    public static ComboBoxItem ToComboBoxItem(this Theme self) {
        return ComboBoxItemHelper.Create(self, ThemeExtensions.GetDisplayText);
    }

    public static ComboBoxItem GetComboBoxItem(this Theme self, ComboBoxItem[] available) {
        return ComboBoxItemHelper.TrySelect(available, self, out var output)
            ? output
            : ComboBoxItemHelper.EmptyComboBoxItem;
    }
}