using System;
using DaysOn.Common.SqlDataAccess;

namespace DaysOn.Auth.Domain.Entities
{
    public class ActivationCode : Entity<Guid>
    {
        public ActivationCode(Guid id, Guid userId, int daysToExpire = 1)
            : base(id)
        {
            UserId = userId;
            ExpiryDate = DateTime.UtcNow.AddDays(daysToExpire);
        }

        protected ActivationCode()
            : base()
        {
        }

        public Guid UserId { get; private set; }

        public DateTime ExpiryDate { get; private set; }
    }
}