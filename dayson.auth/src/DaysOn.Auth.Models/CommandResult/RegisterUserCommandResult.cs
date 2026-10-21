using Newtonsoft.Json;

namespace DaysOn.Auth.Models.CommandResult
{
    public class RegisterUserCommandResult : CommandResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ActivationCode { get; set; }
    }
}