using System;
using DaysOn.Common.Events.Dtos;

namespace DaysOn.HolidaysBc.Models.Commands
{

    public class ChangeHolidayStatusRequest
    {
        public Guid HolidayId { get; set; }
        public Status Status { get; set; }
    }
}
