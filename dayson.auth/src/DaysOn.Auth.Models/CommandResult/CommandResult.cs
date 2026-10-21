using System.Collections.Generic;
using Newtonsoft.Json;

namespace DaysOn.Auth.Models.CommandResult
{
    public class CommandResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
