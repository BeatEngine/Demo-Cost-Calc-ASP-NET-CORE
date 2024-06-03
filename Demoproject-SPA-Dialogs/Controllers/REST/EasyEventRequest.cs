using System.Text.Json;

namespace Demoproject_SPA_Dialogs.Controllers.REST
{
    public class EasyEventRequest
    {
        public string? type { get; set; }
        public Dictionary<string, string>? values { get; set; }

        public EasyEventRequest() { }

        public EasyEventRequest(JsonDocument json)
        {
            type = json.RootElement.GetProperty("type").Deserialize<string?>();
            if(!type.Equals("easy-event-request"))
            {
                throw new Exception("Type for Request is '" + type + "' instead of 'easy-event-request'.");
            }
            values = json.RootElement.GetProperty("values").Deserialize<Dictionary<string, string>>();
        }

    }
}
