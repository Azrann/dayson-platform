using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Caching;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Utilities.Extension;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.Common.MessageHandlers
{
    public class PingAsyncEventHandler<G> : IMessageHandler<G> where G : PingEvent
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool Handle(G message)
        {
            _logger.Info($"PingAsyncEventHandlerAsync called with message {JsonConvertExt.SerializeObject(message)}");
            if (LastPingEvent.GetKeys() == null)
            {
                _logger.Warn($"PingAsyncEventHandlerAsync LastPingEvent.Keys is null {JsonConvertExt.SerializeObject(message)}");
                SetLastPingEvent();
                return true;
            }

            if (!LastPingEvent.HasKey(message.CorrelationId))
            {
                _logger.Info($"PingAsyncEventHandlerAsync LastPingEvent.Keys {JsonConvertExt.SerializeObject(LastPingEvent.GetKeys())} does not contains CorrelationId {message.CorrelationId}");
                SetLastPingEvent();
                return false;
            }

            _logger.Info($"PingAsyncEventHandlerAsync correlationId found {JsonConvertExt.SerializeObject(message.CorrelationId)}");
            SetLastPingEvent();
            return true;
        }

        private static void SetLastPingEvent()
        {
            LastPingEvent.SetDate(DateTime.Now);
        }

        public void LogError(Exception ex)
        {
            _logger.Warn($"PingAsyncEventHandlerAsync throws exception {JsonConvertExt.SerializeObject(ex)}");
        }

        public void LogMessage(string message)
        {
            _logger.Info(message);
        }
    }

    public class LastPingEvent      
    {
        private static DateTime Date {get; set;}
        private static List<Guid> Keys { get; set; }

        private static readonly Object MyLock = new Object();

        public static void SetDate(DateTime date)
        {
            lock (MyLock)
            {
                ObjectCache cache = MemoryCache.Default;
                cache.Set("PingDate", date,DateTimeOffset.Now.AddMinutes(30));
            }
        }

        public static DateTime GetDate()
        {
            lock (MyLock)
            {
                ObjectCache cache = MemoryCache.Default;
                var resultDate = cache.Get("PingDate");
                if (resultDate == null)
                {
                    return DateTime.MinValue;
                }
                return (DateTime)resultDate;
            }
        }

        public static void AddGuid(Guid key)
        {
            lock (MyLock)
            {
                ObjectCache cache = MemoryCache.Default;
                List<Guid> keys = (List<Guid>)cache.Get("PingGuids");
                if (keys == null)
                {
                    keys = new List<Guid>();
                }
                keys.Add(key);
                cache.Set("PingGuids", keys, DateTimeOffset.Now.AddMinutes(30));
            }
        }

        public static bool HasKey(Guid key)
        {
            lock (MyLock)
            {
                ObjectCache cache = MemoryCache.Default;
                List<Guid> keys = (List<Guid>)cache.Get("PingGuids");
                if (keys != null && keys.Contains(key))
                {
                    keys.Remove(key);
                    cache.Set("PingGuids", keys, DateTimeOffset.Now.AddMinutes(30));
                    return true;
                }
                return false;
            }
        }

        public static List<Guid> GetKeys()
        {
            lock (MyLock)
            {
                ObjectCache cache = MemoryCache.Default;
                return (List<Guid>)cache.Get("PingGuids");
            }
        }
    }
        
}
