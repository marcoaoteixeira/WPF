using Nameless.WPF.Resources;

namespace Nameless.WPF.Configuration;

public static class ThemeExtensions {
    public static string GetDisplayText(this Theme self) {
        return self switch {
            Theme.Light => Strings.Theme_Light,
            Theme.Dark => Strings.Theme_Dark,
            Theme.HighContrast => Strings.Theme_HighContrast,
            _ => self.ToString()
        };
    }
}
