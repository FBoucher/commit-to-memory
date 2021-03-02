using Api.Model;
using Api.Service;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cloudies.Function
{
    public class GetQuestionCards
    {
        private readonly OTDBClient _client;

        public GetQuestionCards(OTDBClient client) => _client = client;

        [FunctionName("GetQuestionCards")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            OTDBResponse<OTDBQuestion[]> result = await _client.GetQuestionsAsync(
                quantity: int.TryParse(req.Query["quantity"], out int numberOfQuestions)
                    ? numberOfQuestions
                    : (int?)null,
                categoryId: int.TryParse(req.Query["category"], out int category)
                    ? category
                    : (int?)null,
                questionType: req.Query["type"],
                difficulty: req.Query["difficulty"],
                sessionToken: req.Query["sessionToken"]);

            if (result.ResponseCode == OTDBResponseCode.NoResult)
                return new ObjectResult(Array.Empty<QuestionCard>());

            if (result.ResponseCode == OTDBResponseCode.TokenEmpty)
                return new ObjectResult(Array.Empty<QuestionCard>());

            if (result.ResponseCode == OTDBResponseCode.TokenNotFound)
                return new BadRequestObjectResult(new ApiError
                { Message = "Session token is invalid." });

            if (result.ResponseCode != OTDBResponseCode.Success)
            {
                log.LogError(
                    "OTDB api returned unexpected response code: \"{responseCode}\".",
                    result.ResponseCode);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            QuestionCard[] questionCards = result.Content
                .Select(x => new QuestionCard
                {
                    Category = x.category,
                    Type = x.type,
                    Difficulty = x.difficulty,
                    Question = x.question,
                    CorrectAnswer = x.correct_answer,
                    IncorrectAnswers = x.incorrect_answers
                }).ToArray();

            return new ObjectResult(questionCards);
        }
    }
}
