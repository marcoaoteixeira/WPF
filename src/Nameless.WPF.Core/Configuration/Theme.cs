﻿using System.Text.Json.Serialization;

namespace Nameless.WPF.Configuration;

/// <summary>
///     UI themes.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<Theme>))]
public enum Theme {
    /// <summary>
    ///     The light theme.
    /// </summary>
    Light,

    /// <summary>
    ///     The dark theme.
    /// </summary>
    Dark,

    /// <summary>
    ///     The high contrast theme.
    /// </summary>
    HighContrast
}