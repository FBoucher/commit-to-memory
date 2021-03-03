using Api.Helper;
using Api.Model;
using Api.Service;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Threading.Tasks;

namespace Cloudies.Function
{
    public class CreateTriviaSession
    {
        private readonly OTDBClient _client;

        public CreateTriviaSession(OTDBClient client) => _client = client;

        [FunctionName("CreateTriviaSession")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "create", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            OTDBResponse<OTDBSession> result = await _client.CreateTriviaSessionAsync();

            if (result.ResponseCode != OTDBResponseCode.Success)
            {
                log.LogError(
                    "OTDB api returned unexpected response code: \"{responseCode}\".",
                    result.ResponseCode);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            return new ObjectResult(result.Content);
        }
    }
}
