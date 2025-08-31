namespace Nameless.WPF.Configuration;

public static class AppConfigurationManagerExtensions {
    private const string THEME_KEY = nameof(Theme);
    private const string CONFIRM_BEFORE_EXIT_KEY = "ConfirmBeforeExit";

    public static Theme GetTheme(this IAppConfigurationManager self) {
        return self.TryGet<Theme>(THEME_KEY, out var output) ? output : default;
    }

    public static void SetTheme(this IAppConfigurationManager self, Theme value) {
        self.Set(THEME_KEY, value);
    }

    public static bool GetConfirmBeforeExit(this IAppConfigurationManager self) {
        return self.TryGet<bool>(CONFIRM_BEFORE_EXIT_KEY, out var output) && output;
    }

    public static void SetConfirmBeforeExit(this IAppConfigurationManager self, bool value) {
        self.Set(CONFIRM_BEFORE_EXIT_KEY, value);
    }
}
