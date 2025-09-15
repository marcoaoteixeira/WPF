﻿using System.Reflection;
using Wpf.Ui.Controls;

namespace Nameless.WPF.Navigation;

public class DiscoverableNavigationViewItemProvider : INavigationViewItemProvider {
    private readonly Assembly[] _assemblies;
    private readonly Lazy<Entry[]> _entries;

    public DiscoverableNavigationViewItemProvider(Assembly[] assemblies) {
        _assemblies = Guard.Against.Null(assemblies);
        _entries = new Lazy<Entry[]>(DiscoverNavigationViewItems);
    }

    public IEnumerable<NavigationViewItem> GetMainItems() {
        return _entries.Value
                       .Where(entry => !entry.Attribute.Footer)
                       .Select(entry => entry.ViewIem);
    }

    public IEnumerable<NavigationViewItem> GetFooterItems() {
        return _entries.Value
                       .Where(entry => entry.Attribute.Footer)
                       .Select(entry => entry.ViewIem);
    }

    private Entry[] DiscoverNavigationViewItems() {
        return _assemblies.SelectMany(assembly => assembly.GetExportedTypes())
                          .Where(type => type.HasAttribute<NavigationViewItemAttribute>())
                          .Select(CreateTuple)
                          .Select(CreateEntry)
                          .ToArray();

        static Tuple<Type, NavigationViewItemAttribute> CreateTuple(Type type) {
            var attribute = type.GetCustomAttribute<NavigationViewItemAttribute>()
                            ?? throw new InvalidOperationException($"Missing '{nameof(NavigationViewItemAttribute)}' attribute.");

            return new Tuple<Type, NavigationViewItemAttribute>(type, attribute);
        }

        static Entry CreateEntry(Tuple<Type, NavigationViewItemAttribute> tuple) {
            var (type, attribute) = tuple;
            var viewItem = new NavigationViewItem {
                Content = attribute.Title,
                Icon = new SymbolIcon { Symbol = attribute.Icon },
                TargetPageType = type,
                ToolTip = attribute.ToolTip
            };

            return new Entry(viewItem, attribute);
        }
    }

    internal record Entry(NavigationViewItem ViewIem, NavigationViewItemAttribute Attribute);
}