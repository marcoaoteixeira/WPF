namespace Nameless.WPF.Configuration;

public static class AppConfigurationManagerExtensions {
    private const string THEME_KEY = nameof(Theme);
    private const string CONFIRM_BEFORE_EXIT_KEY = "ConfirmBeforeExit";

    extension(IAppConfigurationManager self) {
        public Theme GetTheme() {
            return self.TryGet<Theme>(THEME_KEY, out var output) ? output : default;
        }

        public void SetTheme(Theme value) {
            self.Set(THEME_KEY, value);
        }

        public bool GetConfirmBeforeExit() {
            return self.TryGet<bool>(CONFIRM_BEFORE_EXIT_KEY, out var output) && output;
        }

        public void SetConfirmBeforeExit(bool value) {
            self.Set(CONFIRM_BEFORE_EXIT_KEY, value);
        }
    }
}
