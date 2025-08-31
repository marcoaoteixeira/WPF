namespace Nameless.WPF.UI.Dialogs.TaskRunnerDialog;

/// <summary>
///     Represents the method that will handle the task runner action.
/// </summary>
/// <param name="cancellationToken">
///     The cancellation token that can be used to cancel the operation.
/// </param>
/// <returns>
///     A <see cref="Task"/> representing the asynchronous operation.
/// </returns>
public delegate Task TaskRunnerAsyncHandler(CancellationToken cancellationToken);