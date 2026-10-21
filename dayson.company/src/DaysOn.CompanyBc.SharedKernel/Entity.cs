using System;
using System.ComponentModel.DataAnnotations;

namespace DaysOn.CompanyBc.SharedKernel
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>, IEntity
    {
        private TId id;

        protected Entity(TId id)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the default value.", "id");
            }

            this.id = id;
            this.CreatedOn = DateTime.UtcNow;
            this.LastUpdated = DateTime.UtcNow;
        }

        protected Entity()
        {
        }

        [Key]
        public TId Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdated { get; set; }

        public override bool Equals(object obj)
        {
            var entity = obj as Entity<TId>;

            if (entity != null)
            {
                return this.Equals(entity);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #region IEquatable<Entity> Members

        public bool Equals(Entity<TId> other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Id.Equals(other.Id);
        }

        #endregion
    }
}
