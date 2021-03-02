namespace Api.Model
{
    public class OTDBCreateSessionResponse
    {
        public OTDBResponseCode response_code { get; set; }
        public string response_message { get; set; }
        public string token { get; set; }
    }
}
