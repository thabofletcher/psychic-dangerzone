using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AverageAmerican;
using System.Net.Http;

namespace psychic_dangerzone.Controllers
{
    public class HomeController : Controller
    {

        const string WA_KEY = "4UEVYA-QPE4K4LUKG";
        const string START_TOKEN = "<plaintext>";
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        [HttpPost]
        public ActionResult Index(string query)
        {
            var url = "http://api.wolframalpha.com/v2/query?appid=" + WA_KEY + "&input=" + query;

            var wac = new HttpClient();

            var responseString = "Get bent! Hit me back when you have something interesting to say...";
            var queryResult = wac.GetStringAsync(url).Result;

            var startIndex = queryResult.LastIndexOf(START_TOKEN);
            if (startIndex != -1)
            {
                startIndex += START_TOKEN.Length;
                var endIndex = queryResult.LastIndexOf("</plaintext>");
                if (endIndex != -1)
                {
                    responseString = queryResult.Substring(startIndex, endIndex - startIndex);
                }
            }
            
            //wac.BaseAddress = 

            //        var query = new WolframAlphaQuery
            //        {
            //            APIKey = WA_KEY,
            //            Formats = new List<string>(),
            //            Query = inputStr,
            //            Asynchronous = false,
            //            AllowCached = true
            //        };

            //var newengine = new WolframAlphaEngine(WA_KEY);
            //var result = newengine.GetWolframAlphaQueryResult(newengine.GetStringRequest(query));
            //result.Pods[inputStr].
            //Console.WriteLine(query.Query);

            //WolframAlpha.WrapperCore.WolframAlphaQuery query = new WolframAlpha.WrapperCore.WolframAlphaQuery();
            //query.APIKey =
            //query.

            //var consumer = new AverageAmerican< 

            return View("Index", null, responseString);
        }
    }
}
