namespace Demoproject_SPA_Dialogs.Controllers.REST
{
    public class EasyEventResponse
    {

        public string type { get; set; } = "easy-event-response";

        public bool success { get; set; } = true;

        public string message { get; set; } = string.Empty;

        public EasyEventResponse(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

        public override string ToString()
        {
            return "{\"type\":\""+type+"\", \"success\":\""+success.ToString().ToLower()+"\", \"message\":\"" + message + "\"}";
        }

    }
}
