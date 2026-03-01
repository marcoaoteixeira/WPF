namespace Nameless.WPF.EntityFrameworkCore.Entities;

public interface IAuditableEntity {
    public string? MostRecentAuditee { get; set; }
    public DateTimeOffset? CreationDate { get; set; }
    public DateTimeOffset? ModificationDate { get; set; }
}