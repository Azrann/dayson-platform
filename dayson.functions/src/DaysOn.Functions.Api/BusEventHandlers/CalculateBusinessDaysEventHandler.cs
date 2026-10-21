using System;
using DaysOn.Common;
using DaysOn.Common.Events.ResultEvents;
using log4net;
using System.Reflection;
using Newtonsoft.Json;
using Castle.Core;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;

namespace DaysOn.Functions.Api.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<CalculateBusinessDaysEvent>))]
    public class CalculateBusinessDaysEventHandler : IMessageHandler<CalculateBusinessDaysEvent>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly IPubnubMessageBus _messageBus;

        public CalculateBusinessDaysEventHandler(IPubnubMessageBus messageBus)
        {
            _messageBus = messageBus;
            _messageBus.SetSecretKey().Start();
        }

        public bool Handle(CalculateBusinessDaysEvent message)
        {
            _logger.LogInf($"CalculateBusinessDaysEventHandler called with message {JsonConvertExt.SerializeObject(message)}");
            Guid channelId = message.SubscriberId;

            var nonWorkingDays = WorkingDaysHelper.GetTotalBankHolidaysBetweenDates(message.NonWorkingDays, message.From, message.To);
            var nonWorkingHalfDays = WorkingDaysHelper.GetTotalBankHolidaysBetweenDates(message.NonWorkingHalfDays, message.From, message.To);

            nonWorkingDays = WorkingDaysHelper.FilterWorkingDaysOfTheWeek(message.WorkingDaysOfTheWeek, nonWorkingDays);
            nonWorkingHalfDays = WorkingDaysHelper.FilterWorkingDaysOfTheWeek(message.WorkingDaysOfTheWeek, nonWorkingHalfDays);

            double days = WorkingDaysHelper.GetWorkingDays(message.From, message.To, message.WorkingDaysOfTheWeek) - nonWorkingDays.Count - (nonWorkingHalfDays.Count / 2.0);
            var result = new DoubleResultEvent()
            {
                Result= days
            };
            _messageBus.Publish(result, channelId.ToString());
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogInf($"CalculateBusinessDaysEventHandler throws exception {JsonConvertExt.SerializeObject(ex)}");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}
