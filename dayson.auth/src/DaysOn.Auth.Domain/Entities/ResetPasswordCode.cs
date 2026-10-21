using System;
using DaysOn.Common.SqlDataAccess;

namespace DaysOn.Auth.Domain.Entities
{
    public class ResetPasswordCode : Entity<Guid>
    {
        public ResetPasswordCode(Guid id, string userId, int daysToExpire = 1) : base(id)
        {
            UserId = userId;
            ExpiryDate = DateTime.UtcNow.AddDays(daysToExpire);
        }

        protected ResetPasswordCode()
            : base()
        {
        }

        public string UserId { get; private set; }

        public DateTime ExpiryDate { get; private set; }
    }
}