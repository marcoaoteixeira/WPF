using System.Reflection;

namespace Nameless.WPF.Mvvm;

public class ViewModelOptions {
    /// <summary>
    ///     Gets or sets the assemblies to scan for view model implementations.
    /// </summary>
    public Assembly[] Assemblies { get; set; } = [];
}