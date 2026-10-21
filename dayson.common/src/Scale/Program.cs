using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DaysOn.Common.CloudSites;
using DaysOn.Common.CloudSites.Models;
using Newtonsoft.Json;

namespace Scale
{
    class Program
    {
        static void Main(string[] args)
        {
            var scaled = CloudService.GetSites().Result
                .Any(x => x.name == "dayson-auth" && x.plan == CloudSitePlan.hobby.ToString());
            if (args.Length == 0 && !scaled)
            {
                ScaleUp();
            }
            else if (args.Length > 0 && scaled)
            {
                ScaleDown();
            }
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
        private static void ScaleUp()
        {
            LogWrite($"{DateTime.Now} ScaleUp started");
            try
            {
                DaysOnCloudService.ScaleUpSitesForTest().Wait();
            }
            catch (Exception ex)
            {
                LogWrite(ex);
            }
            LogWrite($"{DateTime.Now} ScaleUp finished");
        }

        private static void ScaleDown()
        {
            LogWrite($"{DateTime.Now} ScaleDown started");
            try
            {
                DaysOnCloudService.ScaleDownSitesForNight().Wait();
            }
            catch (Exception ex)
            {
                LogWrite(ex);
            }
            LogWrite($"{DateTime.Now} ScaleDown finished");
        }

        private static void LogWrite(Exception ex)
        {
            LogWrite(JsonConvert.SerializeObject(ex));
        }

        private static void LogWrite(string logMessage)
        {
            Console.WriteLine(logMessage);
            var execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.CreateDirectory($"{execPath}\\logs");
            try
            {
                using (StreamWriter w = File.AppendText($"{execPath}\\logs\\{DateTime.Now.ToShortDateString().Replace("/", "-")}-log.txt"))
                {
                    w.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToShortTimeString() + JsonConvert.SerializeObject(ex));
            }
        }
    }
}
