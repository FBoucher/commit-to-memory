namespace Api.Model
{
    public enum OTDBResponseCode
    {
        /// <summary>
        ///     Returned results successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        ///     No Results Could not return results.The API doesn't have enough questions
        ///     for your query. (Ex. Asking for 50 Questions in a Category that only has
        ///     20.)
        /// </summary>
        NoResult = 1,

        /// <summary>
        ///     Invalid Parameter Contains an invalid parameter. Arguements passed in
        ///     aren't valid. (Ex. Amount = Five)
        /// </summary>
        InvalidParameter = 2,

        /// <summary>
        ///     Token Not Found Session Token does not exist.
        /// </summary>
        TokenNotFound = 3,

        /// <summary>
        ///     Session Token has returned all possible questions for the specified query
        ///     Resetting the Token is necessary.
        /// </summary>
        TokenEmpty = 4
    }
}
