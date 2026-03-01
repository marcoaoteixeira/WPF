using Nameless.Registration;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;

namespace Nameless.WPF.Navigation;

public class NavigationRegistrationSettings : AssemblyScanAware<NavigationRegistrationSettings> {
    private Type? _navigationWindow;
    private readonly HashSet<Type> _navigationViews = [];

    public Type NavigationWindow => GetNavigationWindow();
    
    public IReadOnlyCollection<Type> NavigationViews => GetNavigationViews();

    public NavigationRegistrationSettings RegisterNavigationWindow<TNavigationWindow>()
        where TNavigationWindow : INavigationWindow {
        return RegisterNavigationWindow(typeof(TNavigationWindow));
    }

    public NavigationRegistrationSettings RegisterNavigationWindow(Type type) {
        Throws.When.IsNonConcreteType(type);
        Throws.When.IsNotAssignableFrom(type, typeof(INavigationWindow));

        _navigationWindow = type;

        return this;
    }

    public NavigationRegistrationSettings RegisterNavigationView<TNavigationView, TView>()
        where TNavigationView : INavigableView<TView> {
        return RegisterNavigationView(typeof(TNavigationView));
    }

    public NavigationRegistrationSettings RegisterNavigationView(Type type) {
        Throws.When.IsNonConcreteType(type);
        Throws.When.IsOpenGenericType(type);
        Throws.When.IsNotAssignableFromGeneric(type, typeof(INavigableView<>));

        _navigationViews.Add(type);

        return this;
    }

    private Type GetNavigationWindow() {
        var result = UseAssemblyScan
            ? DiscoverImplementationFor<INavigationWindow>()
            : _navigationWindow;

        return result ?? throw new InvalidOperationException($"There is not implementation of '{nameof(INavigationWindow)}' available.");
    }

    private IReadOnlyCollection<Type> GetNavigationViews() {
        return UseAssemblyScan
            ? DiscoverImplementationsFor(typeof(INavigableView<>))
            : _navigationViews;
    }
}