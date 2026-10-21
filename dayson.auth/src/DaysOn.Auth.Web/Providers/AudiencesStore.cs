using System;
using System.Linq;
using System.Security.Cryptography;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace DaysOn.Auth.Web.Providers
{
    public static class AudiencesStore
    {
        private static readonly AuthContext AuthContext;

        static AudiencesStore()
        {
           AuthContext = new AuthContext();
        }

        public static Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            var newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AuthContext.Audience.Add(newAudience);
            AuthContext.SaveChanges();
            return newAudience;
        }

        public static Audience AddAudience(string name, string clientId)
        {

                var key = new byte[32];
                RNGCryptoServiceProvider.Create().GetBytes(key);
                var base64Secret = TextEncodings.Base64Url.Encode(key);

                Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AuthContext.Audience.Add(newAudience);
            AuthContext.SaveChanges();
                return newAudience;


          /*  var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            var newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AuthContext.Audience.Add(newAudience);
            AuthContext.SaveChanges();
            return newAudience;*/
        }

        public static Audience FindAudience(string clientId)
        {
            Audience audience = AuthContext.Audience.FirstOrDefault(x=>x.ClientId==clientId);
            return audience;
        }
    }
}