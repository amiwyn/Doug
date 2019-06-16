namespace Doug.Models
{
    public class DougResponse
    {
        public string Message { get; set; }

        public DougResponse() { }

        public DougResponse(string message)
        {
            Message = message;
        }
    }
}
