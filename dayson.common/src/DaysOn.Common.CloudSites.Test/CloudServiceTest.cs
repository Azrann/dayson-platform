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
    public class CloudServiceTest
    {
        [Test]
        public void WhenIRequestSitesIReceiveAList()
        {
            var result = CloudService.GetSites().Result;
            Assert.IsTrue(result.Count > 8);
        }

        [Test]
        public void WhenIGetISiteIdICanChangeStatus()
        {
            var hrnowId = "5b9c29cd-eaf7-4adf-95da-05d9118a525f";
            var hrnow = CloudService.GetSite(hrnowId).Result;

            bool result = false;
            if (hrnow.status == CloudSiteStatus.Running.ToString())
            {
                result = CloudService.Stop(hrnow.id).Result;
            }
            Assert.IsTrue(result);

            result = false;
            if (hrnow.status == CloudSiteStatus.Running.ToString())
            {
                result = CloudService.Start(hrnow.id).Result;
            }
            Assert.IsTrue(result);

            result = false;
            if (hrnow.status == CloudSiteStatus.Running.ToString())
            {
                result = CloudService.Restart(hrnow.id).Result;
            }
            Assert.IsTrue(result);
        }

        [Test]
        public void WhenIGetISiteIdICanScaleUpAnDown()
        {
            var hrnowId = "5b9c29cd-eaf7-4adf-95da-05d9118a525f";
            var hrnow = CloudService.GetSite(hrnowId).Result;

            bool result = false;
            if (hrnow.plan == CloudSitePlan.free.ToString())
            {
                //standard
                result = CloudService.ScaleTo(hrnowId, CloudSitePlan.standard).Result;
            }
            Assert.IsTrue(result);

            result = false;
            if (hrnow.plan == CloudSitePlan.hobby.ToString())
            {
                result = CloudService.ScaleTo(hrnowId, CloudSitePlan.free).Result;
            }
            Assert.IsTrue(result);
        }

        [Test]
        public void ICanCreateASite()
        {
            var result = CloudService.Create(Guid.NewGuid().ToString(),CloudSitePlan.free).Result;
            Assert.IsTrue(result);
        }

        [Test]
        public void ICanDeleteASite()
        {
            var sites = CloudService.GetSites().Result;
            var siteId = sites.First(x => x.name == "b9ecb8df-6590-42ff-9191-b156be29303a").id;

            var result = CloudService.Delete(siteId).Result;
            Assert.IsTrue(result);
        }
    }
}
