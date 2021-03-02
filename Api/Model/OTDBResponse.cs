namespace Api.Model
{
    public class OTDBResponse<T> where T : class
    {
        public OTDBResponseCode ResponseCode { get; set; }
        public string Error { get; set; }
        public T Content { get; set; }
    }

    public static class OTDBResponse
    {
        public static OTDBResponse<T> Success<T>(T content) 
            where T : class => new OTDBResponse<T>
        {
            Content = content,
            ResponseCode = OTDBResponseCode.Success
        };

        public static OTDBResponse<T> Error<T>(
            OTDBResponseCode errorCode, 
            string errorDetails = null) 
                where T : class => new OTDBResponse<T>
        {
            ResponseCode = errorCode,
            Error = errorDetails
        };
    }
}
