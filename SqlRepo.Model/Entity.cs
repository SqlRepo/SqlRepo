using System;

namespace SqlRepo.Model
{
    public abstract class Entity<T> : IEquatable<Entity<T>>, IEntity<T>
    {
        private int? oldHashCode;

        public T Id { get; set; }

        public bool Equals(Entity<T> other)
        {
            if(other == null)
            {
                return false;
            }

            var isOtherTransient = Equals(other.Id, default(T));
            if(this.IsTransient() && isOtherTransient)
            {
                return ReferenceEquals(this, other);
            }

            return other.Id.Equals(this.Id);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((Entity<T>)obj);
        }

        public override int GetHashCode()
        {
            if(this.oldHashCode.HasValue)
            {
                return this.oldHashCode.Value;
            }

            if(this.IsTransient())
            {
                this.oldHashCode = base.GetHashCode();
                return this.oldHashCode.Value;
            }

            return this.Id.GetHashCode();
        }

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }

        public virtual bool IsTransient()
        {
            return Equals(this.Id, default(T));
        }
    }
}