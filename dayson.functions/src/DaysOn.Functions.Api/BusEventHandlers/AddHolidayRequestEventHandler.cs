using System;
using DaysOn.Common;
using log4net;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Functions.BoundedContext.Services;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using Newtonsoft.Json;
using Status = DaysOn.Common.Events.Dtos.Status;

namespace DaysOn.Functions.Api.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandlerAsync<AddHolidayRequestEvent>))]
    public class AddHolidayRequestEventHandler : IMessageHandlerAsync<AddHolidayRequestEvent>
    {
        private readonly ICreateHolidayService _createHolidayService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPubnubMessageBus _messageBus;

        public AddHolidayRequestEventHandler(ICreateHolidayService createHolidayService, IPubnubMessageBus messageBus)
        {
            _messageBus = messageBus;
            _createHolidayService = createHolidayService;
        }

        public async Task<bool> Handle(AddHolidayRequestEvent message)
        {
            _logger.LogInf($"AddHolidayRequestEventHandler called with message {JsonConvertExt.SerializeObject(message)}");
            //Todo: Create object HolidayRequestData that is populated with results from all operations so that 
            // we do not have to call same services again and we can pass it to the Rule engine.
            // at the end of the call this data is saved for logging diagnostics or something.
            if (message.From.Millisecond != 0)
            {
                _logger.LogErr($"AddHolidayRequestEventHandler specify milliseconds {message.From.Millisecond}");
                SendResult(new BaseResultEvent()
                {
                    Ok = false,
                    CorrelationId = message.CorrelationId
                }, message.SubscriberId.ToString());
                return true;
            }

            var validationResponse = await _createHolidayService.Validate(message);
            if (!validationResponse.Ok)
            {
                SendResult(validationResponse, message.SubscriberId.ToString());
                return true;
            }

            var businessDaysResponse = _createHolidayService.GetBusinessDays(message, validationResponse);
            if (!businessDaysResponse.Ok)
            {
                SendResult(businessDaysResponse, message.SubscriberId.ToString());
            }

            if (message.Status != Status.Aproved)
            {
                BaseResultEvent functionResponse;
                if (!_createHolidayService.GetNonValidatedCompanyRules(message, out functionResponse))
                {
                    SendResult(functionResponse, message.SubscriberId.ToString());
                    return true;
                }

                if (!_createHolidayService.GetNonValidatedTeamRules(message, out functionResponse))
                {
                    SendResult(functionResponse, message.SubscriberId.ToString());
                    return true;
                }

                if (
                    !_createHolidayService.GetNonValidatedSystemRules(message, out functionResponse,
                        businessDaysResponse.Days))
                {
                    SendResult(functionResponse, message.SubscriberId.ToString());
                    return true;
                }
            }

            var holidayRequest = await _createHolidayService.CreateHolidayRequest(message, businessDaysResponse.Days, validationResponse.YearStartDay);
            
            _createHolidayService.UpdateCalculation(message.EmployeeId, holidayRequest.Year);

            SendResult(new BaseResultEvent()
            {
                Ok = true,
                CorrelationId = message.CorrelationId
            }, message.SubscriberId.ToString());
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"AddHolidayRequestEventHandler throws exception {JsonConvertExt.SerializeObject(ex)}");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }

        private void SendResult(BaseResultEvent functionResponse,string channelId)
        {
            _messageBus.SetSecretKey().Start();
            _messageBus.Publish(functionResponse, channelId);
        }
    }
}
