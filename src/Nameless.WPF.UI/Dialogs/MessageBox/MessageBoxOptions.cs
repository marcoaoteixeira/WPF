using System.Windows;

namespace Nameless.WPF.UI.Dialogs.MessageBox;

public sealed class MessageBoxOptions {
    public string? Title { get; set; }

    public MessageBoxButtons Buttons { get; set; }

    public MessageBoxIcon Icon { get; set; }

    public Window? Owner { get; set; }
}