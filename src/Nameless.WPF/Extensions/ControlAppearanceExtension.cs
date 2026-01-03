using Nameless.WPF.Resources;
using Wpf.Ui.Controls;

namespace Nameless.WPF;

/// <summary>
///     <see cref="ControlAppearance"/> extensions methods.
/// </summary>
public static class ControlAppearanceExtension {
    /// <param name="self">
    ///     The current <see cref="ControlAppearance"/>.
    /// </param>
    extension(ControlAppearance self) {
        /// <summary>
        ///     Retrieves the text title associated with the
        ///     specified <see cref="ControlAppearance"/>.
        /// </summary>
        /// <returns>
        ///     The text title associated with the <see cref="ControlAppearance"/>.
        /// </returns>
        public string GetTitle() {
            return self switch {
                ControlAppearance.Caution => Strings.ControlAppearance_Caution,
                ControlAppearance.Danger => Strings.ControlAppearance_Danger,
                ControlAppearance.Success => Strings.ControlAppearance_Success,
                _ => Strings.ControlAppearance_Info
            };
        }

        /// <summary>
        ///     Retrieves the icon associated with the
        ///     specified <see cref="ControlAppearance"/>.
        /// </summary>
        /// <returns>
        ///     The icon associated with the <see cref="ControlAppearance"/>.
        /// </returns>
        public IconElement GetIcon() {
            return self switch {
                ControlAppearance.Caution => new SymbolIcon(SymbolRegular.Warning28),
                ControlAppearance.Danger => new SymbolIcon(SymbolRegular.ShieldError24),
                ControlAppearance.Info => new SymbolIcon(SymbolRegular.LightbulbFilament24),
                ControlAppearance.Success => new SymbolIcon(SymbolRegular.ThumbLike28),
                _ => new SymbolIcon(SymbolRegular.Book24)
            };
        }
    }
}