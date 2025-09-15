using System.Reflection;

namespace Nameless.WPF.Windows;

public class WindowFactoryOptions {
    /// <summary>
    ///     Gets or sets the assemblies to scan for.
    /// </summary>
    public Assembly[] Assemblies { get; set; } = [];
}