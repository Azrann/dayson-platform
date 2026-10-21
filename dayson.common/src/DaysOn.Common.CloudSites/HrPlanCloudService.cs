using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.Common.CloudSites.Models;
using Newtonsoft.Json;

namespace DaysOn.Common.CloudSites
{
    public class HrPlanCloudService
    {
        public static Task<WebsiteAndDb> CreateSiteAndDb(string seed, string sitenameprefix, string dbnameprefix = "")
        {
            var serviceName = $"HrPlan{sitenameprefix}-{seed}";
            var dbname = dbnameprefix == string.Empty
                ? $"HrPlan{sitenameprefix}{seed}".ToLower()
                : $"HrPlan{dbnameprefix}{seed}".ToLower();

            List<Task> tasks = new List<Task>
            {
                CloudService.Create(serviceName, CloudSitePlan.free),
                DatabaseService.Create(dbname, DatabasePlan.free, DatabaseType.mssql)
            };
            Task.WhenAll(tasks).Wait();

            Database serviceDb = null;
            while (serviceDb == null)
            {
                Thread.Sleep(2500);
                serviceDb = DatabaseService.GetDatabases().Result.FirstOrDefault(x => x.name == dbname);
            }

            var users = DatabaseService.GetUsers(serviceDb.id).Result;

            WebsiteAndDb result = new WebsiteAndDb()
            {
                servicename = serviceName,
                dbname = dbname,
                dbHost = "unknown",
                username = users.First().userName,
                password = users.First().password,
                websiteUrl = $"{serviceName}.gearhostpreview.com",
                prefix = sitenameprefix
            };

            return Task.FromResult(result);
        }

        public static Task<WebsiteAndDb> CreateSite(string seed, string sitenameprefix)
        {
            var serviceName = $"HrPlan{sitenameprefix}-{seed}";

            CloudService.Create(serviceName, CloudSitePlan.free).Wait();
            WebsiteAndDb result = new WebsiteAndDb()
            {
                servicename = serviceName,
                dbname = "unknown",
                dbHost = "unknown",
                username = "REPLACE_WITH_YOUR_VALUE",
                password = "REPLACE_WITH_YOUR_VALUE",
                websiteUrl = $"{serviceName}.gearhostpreview.com",
                prefix = sitenameprefix
            };
            return Task.FromResult(result);
        }

        public static Task<List<WebsiteAndDb>> CreateSitesAndDb()
        {
            string seed = Guid.NewGuid().ToString().Substring(0, 4);
            var result = new List<WebsiteAndDb>();
            result.Add(CreateSiteAndDb(seed, "Auth").Result);
            result.Add(CreateSiteAndDb(seed, "Company").Result);
            result.Add(CreateSiteAndDb(seed, "Absence").Result);
            result.Add(CreateSiteAndDb(seed, "Functions","nosql").Result);
            result.Add(CreateSite(seed, "Facade").Result);

            return Task.FromResult(result);
        }

        public static void WriteConfigFile(WebsiteAndDb siteDb)
        {
            switch (siteDb.prefix)
            {
                case "Auth":
                    WriteFile("AuthDb","AuthServer", siteDb);
                    break;
                case "Company":
                    WriteFile("CompanyDB", "CompanyServer", siteDb);
                    break;
                case "Absence":
                    WriteFile("HolidaysDB", "HolidayServer", siteDb);
                    break;
                case "Functions":
                    WriteFile("NoSQL", "FunctionsServer", siteDb);
                    break;
            }
        }

        private static void WriteFile(string dbKeyName, string siteKeyName, WebsiteAndDb siteDb)
        {
            string db = $"REPLACE_WITH_YOUR_VALUE";
            string site = $"<add key=\"{siteKeyName}\" value=\"http:\\\\{siteDb.websiteUrl}\\\" xdt:Transform=\"SetAttributes\" xdt:Locator=\"Match(key)\"/>";
            var execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.CreateDirectory($"{execPath}\\configFiles");
            try
            {
                using (StreamWriter w = File.AppendText($"{execPath}\\configFiles\\{DateTime.Now.ToShortDateString().Replace("/", "-")}-config.txt"))
                {
                    w.WriteLine(db);
                    w.WriteLine(site);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToShortTimeString() + JsonConvert.SerializeObject(ex));
            }
        }
    }
}
