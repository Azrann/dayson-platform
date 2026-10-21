using System.Collections.Generic;

namespace DaysOn.Facade.Common.Dtos
{
    public class CommandResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
