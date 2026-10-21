using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustEat.StatsD;
using NUnit.Framework;

namespace DaysOn.Common.ServiceBase.Test
{
    [TestFixture]
    public class StatsDTest
    {
        [Test]
        public void WhenICallStatdsDIdontReceiveAnyError()
        {
            var statsDConfig = new StatsDConfiguration
            {
                Host = "statsd.hopto.org",
                Port = 8125,
                Prefix = "5e4360be-cafa-4a93-bfdd-e1a0989074a3",
            };
            IStatsDPublisher publisher = new StatsDPublisher(statsDConfig);
            var bucketName = $"QA.DaysOnCommon.webapi.Test.StatsD.Get.200".ToLower();
            var bucketNameCounter = $"QA.DaysOnCommon.webapi.Test.StatsD.Send".ToLower();
            publisher.Increment(bucketNameCounter);
            Random rnd = new Random();
            var milliseconds = rnd.Next(1,50) * 28;
            publisher.Timing(TimeSpan.FromMilliseconds(milliseconds), bucketName);
            Assert.AreEqual(true,true);
        }
    }
}
