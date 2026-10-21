using System;
using System.Collections.Generic;
using DaysOn.Common.Events.Events;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace DaysOn.Auth.Domain.Entities
{
    public class User : IdentityUser
    {
        public User(string userId, string email, string companyId) : base(email)
        {
            Id = userId;
            Email = email;
            CompanyId = new Guid(companyId);
        }

        protected User()
        {
        }

        public Guid CompanyId { get; set; }

        public bool IsActive { get; private set; }

        public void Activate(ActivationCode activationCode)
        {
            if (activationCode.UserId.ToString() != Id)
            {
                throw new ArgumentException($"Activation code {activationCode.Id} is not valid");
            }
            
            if (activationCode.ExpiryDate < DateTime.UtcNow)
            {
                throw new ArgumentException($"Activation code {activationCode.Id} has already expired");
            }

            IsActive = true;
            EmailConfirmed = true;
        }
    }
}
