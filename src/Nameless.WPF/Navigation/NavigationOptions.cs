using System.Reflection;

namespace Nameless.WPF.Navigation;

public class NavigationOptions {
    /// <summary>
    ///     Gets or sets the assemblies to scan for.
    /// </summary>
    public Assembly[] Assemblies { get; set; } = [];
}