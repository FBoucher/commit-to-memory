using Api.Helper;
using Api.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Service
{
    public class OTDBClient
    {
        private readonly HttpClient _httpClient;

        public OTDBClient(HttpClient httpClient) =>
            _httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));

        public async Task<OTDBResponse<OTDBSession>> CreateTriviaSessionAsync()
        {
            HttpResponseMessage response = await _httpClient
                .GetAsync("api_token.php?command=request")
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(
                    Msg_UnexpectedHttpStatus(response.StatusCode));

            OTDBCreateSessionResponse responseContent = await response.Content
                .ReadAsAsync<OTDBCreateSessionResponse>()
                .ConfigureAwait(false);

            if (responseContent == null)
                throw new InvalidOperationException(Msg_UnexpectedContent());

            if (responseContent?.response_code != OTDBResponseCode.Success)
                return OTDBResponse.Error<OTDBSession>(responseContent.response_code);

            if (string.IsNullOrWhiteSpace(responseContent.token))
                throw new InvalidOperationException(Msg_UnexpectedContent());

            return OTDBResponse.Success(new OTDBSession { token = responseContent.token });
        }

        public async Task<OTDBResponse<OTDBCategory[]>> GetCategoriesAsync()
        {
            HttpResponseMessage response = await _httpClient
                .GetAsync("api_category.php")
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(
                    Msg_UnexpectedHttpStatus(response.StatusCode));

            OTDBGetCategoriesResponse responseContent = await response.Content
                .ReadAsAsync<OTDBGetCategoriesResponse>()
                .ConfigureAwait(false);

            if (responseContent?.trivia_categories == null)
                throw new InvalidOperationException(Msg_UnexpectedContent());

            return OTDBResponse.Success(responseContent.trivia_categories);
        }

        public async Task<OTDBResponse<OTDBQuestion[]>> GetQuestionsAsync(
            int? quantity = null,
            int? categoryId = null,
            string questionType = null,
            string difficulty = null,
            string sessionToken = null)
        {
            var queryStringParameters = new Dictionary<string, string>()
            {
                { "amount", OTDBHelper.ParseQuantity(quantity)},
                { "category", OTDBHelper.ParseCategoryId(categoryId)},
                { "type", OTDBHelper.ParseQuestionType(questionType)},
                { "difficulty", OTDBHelper.ParseDifficulty(difficulty)},
                { "token", sessionToken},
            };

            string queryString = string.Join("&", queryStringParameters
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .OrderBy(x => x.Key)
                .Select(x => $"{x.Key}={x.Value}"));

            HttpResponseMessage response = await _httpClient
                .GetAsync(string.IsNullOrWhiteSpace(queryString)
                    ? $"api.php"
                    : $"api.php?{queryString}")
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(
                    Msg_UnexpectedHttpStatus(response.StatusCode));

            OTDBGetQuestionsResponse responseContent = await response.Content
                .ReadAsAsync<OTDBGetQuestionsResponse>()
                .ConfigureAwait(false);

            if (responseContent == null)
                throw new InvalidOperationException(Msg_UnexpectedContent());

            if (responseContent.response_code != OTDBResponseCode.Success)
                return OTDBResponse.Error<OTDBQuestion[]>(responseContent.response_code);

            if (responseContent.results == null)
                throw new InvalidOperationException(Msg_UnexpectedContent());

            return OTDBResponse.Success(responseContent.results);
        }

        private static string Msg_UnexpectedContent() =>
            $"OTDB api returned unexpected content.";

        public static string Msg_UnexpectedHttpStatus(HttpStatusCode status) =>
            $"OTDB api returned an unexpected http status code \"{(int)status}\".";
    }
}
