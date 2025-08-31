namespace Nameless.WPF.UI.Mvvm;

/// <summary>
///     Defines a type that has a view model.
/// </summary>
/// <typeparam name="TViewModel">
///     Type of the view model.
/// </typeparam>
public interface IHasViewModel<out TViewModel>
    where TViewModel : ViewModel {
    /// <summary>
    ///     Gets the view model.
    /// </summary>
    TViewModel ViewModel { get; }
}
