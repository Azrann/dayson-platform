using System;

namespace DaysOn.Log.Web.Models
{
    public class LogItem
    {
        private DateTimeOffset _date;
        public int Id { get; set; }
        public DateTimeOffset Date {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                DateString =
                    TimeZoneInfo.ConvertTime(value, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"))
                        .ToString("dd-MM HH:mm:ss");
            }
        }
        public string DateString { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
