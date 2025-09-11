using Wpf.Ui.Controls;

namespace Nameless.WPF.UI.Navigation;

public interface INavigationViewItemProvider {
    IEnumerable<NavigationViewItem> GetMainNavigationViewItems();
    IEnumerable<NavigationViewItem> GetFooterNavigationViewItems();
}
