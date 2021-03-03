namespace Api.Model
{
    public class OTDBGetQuestionsResponse
    {
        public OTDBResponseCode response_code { get; set; }
        public OTDBQuestion[] results { get; set; }
    }
}
