namespace Nameless.WPF.EntityFrameworkCore.Entities;

public abstract class EntityBase<TID>
    where TID : struct, IEquatable<TID> {
    public TID ID { get; set; }
}