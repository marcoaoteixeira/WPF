using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases;

public abstract record Response {
    public string? Error { get; }

    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public virtual bool Succeeded => string.IsNullOrWhiteSpace(Error);

    protected Response(string? error) {
        Error = error;
    }
}
