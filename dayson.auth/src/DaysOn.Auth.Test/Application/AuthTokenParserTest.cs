using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace DaysOn.Auth.Test.Application
{
    [TestFixture]
    public class AuthTokenParserTest
    {
        public string token = "REPLACE_WITH_YOUR_VALUE";
        
        [Test]
        public void WhenTokenIsValidThenIsValidReturnTrue()
        {
            var result = IsValidToken(token);
            Assert.IsTrue(result);
        }

        private bool IsValidToken(string access_token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(access_token))
            {
                var tokenS = handler.ReadToken(access_token) as JwtSecurityToken;

                var companyId = tokenS.Claims.First(claim => claim.Type == "Company").Value;
                if (!string.IsNullOrEmpty(companyId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
