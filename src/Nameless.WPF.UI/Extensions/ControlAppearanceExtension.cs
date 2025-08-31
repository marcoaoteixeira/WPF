using Nameless.WPF.UI.Resources;
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
            ControlAppearance.Caution => ControlAppearanceResource.Caution,
            ControlAppearance.Danger => ControlAppearanceResource.Danger,
            ControlAppearance.Success => ControlAppearanceResource.Success,
            _ => ControlAppearanceResource.Info
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