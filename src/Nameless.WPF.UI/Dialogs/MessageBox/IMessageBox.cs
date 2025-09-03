using System.Windows;

namespace Nameless.WPF.UI.Dialogs.MessageBox;

public interface IMessageBox {
    MessageBoxResult Show(MessageBoxOptions options);
}

public record MessageBoxOptions {
    public string? Title { get; init; }

    public required string Message { get; init; }

    public MessageBoxButtons Buttons { get; init; }

    public MessageBoxIcon Icon { get; init; }

    public Window? Owner { get; init; }
}