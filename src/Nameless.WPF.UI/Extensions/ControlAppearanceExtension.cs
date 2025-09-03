using Nameless.WPF.Resources;
using Wpf.Ui.Controls;

namespace Nameless.WPF.UI;

/// <summary>
///     <see cref="ControlAppearance"/> extensions methods.
/// </summary>
public static class ControlAppearanceExtension {
    /// <summary>
    ///     
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string GetTitle(this ControlAppearance self) {
        return self switch {
            ControlAppearance.Caution => Strings.ControlAppearance_Caution,
            ControlAppearance.Danger => Strings.ControlAppearance_Danger,
            ControlAppearance.Success => Strings.ControlAppearance_Success,
            _ => Strings.ControlAppearance_Info
        };
    }

    public static IconElement GetIcon(this ControlAppearance self) {
        return self switch {
            ControlAppearance.Caution => new SymbolIcon(SymbolRegular.Warning28),
            ControlAppearance.Danger => new SymbolIcon(SymbolRegular.ShieldError24),
            ControlAppearance.Info => new SymbolIcon(SymbolRegular.LightbulbFilament24),
            ControlAppearance.Success => new SymbolIcon(SymbolRegular.ThumbLike28),
            _ => new SymbolIcon(SymbolRegular.Book24)
        };
    }
}