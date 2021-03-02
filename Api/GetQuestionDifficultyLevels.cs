using Api.Helper;
using Api.Model;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System.Linq;

namespace Cloudies.Function
{
    public class GetQuestionDifficultyLevels
    {
        [FunctionName("GetQuestionDifficultyLevels")]
        public QuestionDifficultyLevel[] Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            QuestionDifficultyLevel[] levels = OTDBHelper.GetDifficultyLevels()
                .Select(x => new QuestionDifficultyLevel { Id = x.id, Name = x.name })
                .ToArray();
            return levels;
        }
    }
}
