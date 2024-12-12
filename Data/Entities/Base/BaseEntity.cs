namespace Data.Entities.Base;

public abstract class BaseEntity<T>
{
    public required T Id { get; set; }
}