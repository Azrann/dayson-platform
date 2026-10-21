using Moq;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace DaysOn.HolidaysBc.Api.Controllers.Tests
{
    public class BaseControllerTest
    {
        public Guid _employeeId;
        public Guid _companyId;
        public Mock<IPrincipal> _principal;

        public BaseControllerTest()
        {
            _employeeId = Guid.NewGuid();
            _companyId = Guid.NewGuid();

            ClaimsIdentity claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("Company", _companyId.ToString()));
            claims.AddClaim(new Claim("Employee", _employeeId.ToString()));

            _principal = new Mock<IPrincipal>();
            _principal.Setup(x => x.Identity).Returns(claims);
        }
    }
}