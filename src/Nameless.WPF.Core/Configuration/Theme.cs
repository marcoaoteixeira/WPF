using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Nameless.WPF.Configuration;

/// <summary>
///     UI themes.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<Theme>))]
public enum Theme {
    /// <summary>
    ///     The light theme.
    /// </summary>
    [Description("Claro")]
    Light,

    /// <summary>
    ///     The dark theme.
    /// </summary>
    [Description("Escuro")]
    Dark,

    /// <summary>
    ///     The high contrast theme.
    /// </summary>
    [Description("Alto Contraste")]
    HighContrast
}