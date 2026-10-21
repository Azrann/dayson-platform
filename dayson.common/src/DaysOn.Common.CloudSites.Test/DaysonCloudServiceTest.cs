using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysOn.Common.CloudSites.Models;
using NUnit.Framework;

namespace DaysOn.Common.CloudSites.Test
{
    [TestFixture]
    public class DaysonCloudServiceTest
    {
        [Test]
        public void WhenIRestartAllServicesIDontGetExceptions()
        {
            DaysOnCloudService.RestartAllSites().Wait();
        }

        [Test]
        public void WhenIScaleDownSitesForNightIDontGetExceptions()
        {
            DaysOnCloudService.ScaleDownSitesForNight().Wait();
            var result = CloudService.GetSites().Result;
            foreach (var site in result)
            {
               Assert.AreEqual(CloudSitePlan.free.ToString(),site.plan); 
            }
        }

        [Test]
        public void WhenIScaleUpSitesForTestIDontGetExceptions()
        {
            DaysOnCloudService.ScaleUpSitesForTest().Wait();
            var result = CloudService.GetSites().Result;
            foreach (var site in result.Where(x => x.name == "dayson-auth" || x.name == "dayson-companybc"))
            {
                Assert.AreEqual(CloudSitePlan.hobby.ToString(), site.plan);
            }
        }
    }
}
