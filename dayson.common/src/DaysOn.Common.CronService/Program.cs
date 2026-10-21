using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using CronNET;
using DaysOn.Common.CloudSites;
using Newtonsoft.Json;

namespace DaysOn.Common.CronService
{
    static class Program
    {
        private static readonly CronDaemon CronDaemon = new CronDaemon();

        static void Main(string[] args)
        {
            LogWrite("Starting CRON JOBS...");
            CronDaemon.AddJob("*/5 * * * *", task);
            CronDaemon.AddJob("4,9,14,19,24,29,34,39,44,49,54,59 * * * *", PingProd);
            CronDaemon.AddJob("3,8,13,18,23,28,33,38,43,48,53,58 * * * *", PingQa);
            CronDaemon.AddJob("1 6,14,20 * * *", scaleUp);
            CronDaemon.AddJob("32 0,9,17 * *", scaleDown);

            CronDaemon.Start();

            // Wait and sleep forever. Let the cron daemon run.
            while (true) Thread.Sleep(6000);
        }

        private static void scaleUp()
        {
            LogWrite($"{DateTime.Now} scaleUp started");
            try
            {
                DaysOnCloudService.ScaleUpSitesForTest().Wait();
            }
            catch (Exception ex)
            {
                LogWrite(ex);
            }
            LogWrite($"{DateTime.Now} scaleUp finished");
        }

        private static void scaleDown()
        {
            LogWrite($"{DateTime.Now} scaleDown started");
            try
            {
                DaysOnCloudService.ScaleDownSitesForNight().Wait();
            }
            catch (Exception ex)
            {
                LogWrite(ex);
            }
            LogWrite($"{DateTime.Now} scaleDown finished");
        }

        static void task()
        {
            LogWrite($"{DateTime.Now} ping started");
            List<Task> tasks = new List<Task>();
            try
            {
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-functions.gear.host/", "ping"));
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-companybc.gear.host/", "ping"));
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-holidays.gearhostpreview.com/", "ping"));
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-auth.gear.host/", "ping"));
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                LogWrite(ex);
            }
            LogWrite($"{DateTime.Now} ping finished");
        }

        static void PingQa()
        {
            LogWrite($"{DateTime.Now}  ping qa started");
            List<Task> tasks = new List<Task>();
            try
            {
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-facade.gearhostpreview.com/", "version"));
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-functions.gear.host/", "version"));
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-companybc.gear.host/", "version"));
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-holidays.gearhostpreview.com/", "version"));
                tasks.Add(HttpClientHelper.GetAsync("http://dayson-auth.gear.host/", "version"));
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                LogWrite(ex);
            }
            LogWrite($"{DateTime.Now} ping qa finished");
        }

        static void PingProd()
        {
            LogWrite($"{DateTime.Now} ping prod started");
            List<Task> tasks = new List<Task>();
            try
            {
                tasks.Add(HttpClientHelper.GetAsync("http://HrPlanFacade-39e7.gearhostpreview.com/", "version"));
                tasks.Add(HttpClientHelper.GetAsync("http://HrPlanFunctions-39e7.gearhostpreview.com/", "ping"));
                tasks.Add(HttpClientHelper.GetAsync("http://HrPlanAbsence-39e7.gearhostpreview.com/", "ping"));
                tasks.Add(HttpClientHelper.GetAsync("http://HrPlanCompany-39e7.gearhostpreview.com/", "ping"));
                tasks.Add(HttpClientHelper.GetAsync("http://HrPlanAuth-39e7.gearhostpreview.com/", "ping"));
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                LogWrite(ex);
            }
            LogWrite($"{DateTime.Now} ping prod finished");
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
                using (StreamWriter w = File.AppendText($"{execPath}\\logs\\{DateTime.Now.ToShortDateString().Replace("/","-")}-log.txt"))
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
