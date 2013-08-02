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
        const string REQUIRED = "id='Result'";
        const string START_TOKEN = "<plaintext>";
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Console(string command)
        {
            var url = "http://api.wolframalpha.com/v2/query?appid=" + WA_KEY + "&input=" + command;

            var wac = new HttpClient();
            var responseString = "Get bent! Hit me back when you have something interesting to say...";
            var queryResult = wac.GetStringAsync(url).Result;

            var requiredIndex = queryResult.IndexOf(REQUIRED);
            if (requiredIndex != -1)
            {
                var startIndex = queryResult.IndexOf(START_TOKEN, requiredIndex);
                if (startIndex != -1)
                {
                    startIndex += START_TOKEN.Length;
                    var endIndex = queryResult.IndexOf("</plaintext>", startIndex);
                    if (endIndex != -1)
                    {
                        responseString = queryResult.Substring(startIndex, endIndex - startIndex);
                    }
                }
            }

            Response.Write(responseString);
            return new HttpStatusCodeResult(200);
        }
    }
}
