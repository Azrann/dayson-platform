using System;
using DaysOn.Common.Events.Dtos;
using DaysOn.Common.Events.Events;

namespace DaysOn.Common.Events.FunctionsEvents
{
    public class AddHolidayRequestEvent : BaseEvent
    {
        public Guid Id { get; set; }
        public Guid SubscriberId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Notes { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid CompanyId { get; set; }
        public Status Status { get; set; }
    }
}
