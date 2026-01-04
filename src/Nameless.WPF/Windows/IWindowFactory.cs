using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.Windows;

/// <summary>
///     Defines a window factory.
/// </summary>
public interface IWindowFactory {
    /// <summary>
    ///     Tries to create a window.
    /// </summary>
    /// <typeparam name="TWindow">
    ///     Type of the window.
    /// </typeparam>
    /// <param name="output">
    ///     The window instance.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if it was possible to create the window;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    bool TryCreate<TWindow>([NotNullWhen(returnValue: true)] out TWindow? output)
        where TWindow : IWindow;
}