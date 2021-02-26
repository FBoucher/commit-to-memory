using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace Cloudies.Function
{
    public static class GetQuestionTypes
    {
        [FunctionName("GetQuestionTypes")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string amount = req.Query["amount"];
            string difficulty = req.Query["difficulty"];
            

            if( !String.IsNullOrEmpty(amount) && !String.IsNullOrEmpty(difficulty)) 
            {
                var client = new HttpClient();
                string apiURL = $"https://opentdb.com/api.php?amount={amount}&difficulty={difficulty}&type=multiple";
                var response = await client.GetStringAsync(apiURL);
                return new OkObjectResult(response);
            }
            
            return new OkObjectResult("NOTHING!!");
        }
    }
}
