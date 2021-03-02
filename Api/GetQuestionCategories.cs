using Api.Model;
using Api.Service;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cloudies.Function
{
    public class GetQuestionCategories
    {
        private readonly OTDBClient _client;

        public GetQuestionCategories(OTDBClient client) => _client = client;

        [FunctionName("GetQuestionCategories")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var result = await _client.GetCategoriesAsync();

            if (result.ResponseCode != OTDBResponseCode.Success)
            {
                log.LogError(
                    "OTDB api returned unexpected response code: \"{responseCode}\".",
                    result.ResponseCode);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            QuestionCategory[] categories = result.Content
                .Select(x => new QuestionCategory { Id = x.id.ToString(), Name = x.name })
                .OrderBy(x => x.Name)
                .ToArray();

            return new ObjectResult(categories);
        }
    }
}
