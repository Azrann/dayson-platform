using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Configuration;
using System.Net;
using DaysOn.CompanyBc.Test.Common;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Events;

namespace DaysOn.CompanyBc.Test.Acceptance
{
    [TestFixture]
    public class CompanyControllerTest
    {
        [Test]
        public void WhenIRaiseAUserCreatedEvent_UserIsCreated()
        {
            /* Test with harcoded data, will fail when we recreate DB again */
            var name = string.Format("Alberto{0}",Guid.NewGuid().ToString());
            var email = string.Format("{0}@gmail.com", name);
            // EmployeeCreatedEventHandler handler = new EmployeeCreatedEventHandler(null);
            //handler.Handle(new EmployeeCreatedEvent(email, new Guid("7eb279ed-3f9b-43a8-8c6e-9eee4208245a"), Guid.NewGuid(), "REPLACE_WITH_YOUR_VALUE"));
        }
    }
}
