namespace DaysOn.HolidaysBc.Models.Commands
{
    public class CarryOverCommand : ApiCommand
    {
        public int Year { get; set; }
        public int Days { get; set; }
    }
}
