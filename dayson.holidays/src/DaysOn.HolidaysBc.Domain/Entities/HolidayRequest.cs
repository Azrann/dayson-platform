using System;
using DaysOn.Common.SqlDataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class HolidayRequest: Entity<Guid>
    {
        public HolidayRequest(Guid id) : base(id)
        {
        }

        protected HolidayRequest()
            : base()
        {
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Status Status { get; set; }
        public decimal NumberOfDays { get; set; }
        public string Notes { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int Year { get; set; }

        public void Cancelled()
        {
            Status = Status.Canceled;
        }

        public void Rejected()
        {
            Status = Status.Rejected;
        }

        public void Approved()
        {
            Status = Status.Aproved;
        }

        public void SetYear(DateTime employeeYearStartDay)
        {
            var startDateOfYear = new DateTime(From.Year, employeeYearStartDay.Month, employeeYearStartDay.Day);
            var compareResult = DateTime.Compare(From, startDateOfYear);

            if (compareResult >= 0)
            {
                Year = From.Year;
                if (DateTime.Compare(To, startDateOfYear.AddYears(1)) >= 0)
                {
                    throw new ArgumentException("Holiday request between two years.");
                }
            }
            else
            {
                Year = From.Year - 1;
                if (DateTime.Compare(To, startDateOfYear) >= 0)
                {
                    throw new ArgumentException("Holiday request between two years.");
                }
            }
        }
    }
}
