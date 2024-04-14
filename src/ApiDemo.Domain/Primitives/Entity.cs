// -------------------------------------------------------------------------------------
//  <copyright file="Entity.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Primitives;

public abstract class Entity<T> : IEqualityComparer<Entity<T>>
    where T : notnull
{
    protected Entity(T id)
    {
        if (id is null || id.Equals(default(T)))
        {
            throw new ArgumentNullException(nameof(id));
        }

        Id = id;
    }

    protected Entity()
    {
    }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;

    public T Id { get; set; } = default!;

    public DateTime UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = default!;

    public int Version { get; set; }

    public static bool operator !=(Entity<T>? left, Entity<T>? right)
    {
        return !EqualOperator(left, right);
    }

    public static bool operator ==(Entity<T>? left, Entity<T>? right)
    {
        return EqualOperator(left, right);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<T> entity && Id.Equals(entity.Id);
    }

    public bool Equals(Entity<T>? left, Entity<T>? right)
    {
        if (left == null || right == null)
        {
            return false;
        }

        return left.Id.Equals(right.Id);
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public int GetHashCode(Entity<T> obj)
    {
        return obj.Id.GetHashCode();
    }

    protected static bool EqualOperator(Entity<T>? left, Entity<T>? right)
    {
        if (left is null || right is null)
        {
            return false;
        }

        return ReferenceEquals(left, right) || left.Id.Equals(right.Id);
    }
}