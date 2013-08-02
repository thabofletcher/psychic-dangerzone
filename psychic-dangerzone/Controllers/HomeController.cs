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

        List<string> DefaultResponses = new List<string>
        {
            "Why don't you ask a good question?!",
            "You suck!",
            "I think this one has had too much to drink!",
            "JFDI",
            "Seriously?",
            "Yeaaaah, I'm gonna have to get back to you on that one",
            "Buffer overrun",
            "42"
        };

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Console(string command)
        {
            var url = "http://api.wolframalpha.com/v2/query?appid=" + WA_KEY + "&input=" + command;

            var wac = new HttpClient();
            var responseString = "";
            var queryResult = wac.GetStringAsync(url).Result;

            var startIndex = 0;
            while (true)
            {
                startIndex = queryResult.IndexOf(START_TOKEN, startIndex);
                if (startIndex == -1)
                    break;

                startIndex += START_TOKEN.Length;
                var endIndex = queryResult.IndexOf("</plaintext>", startIndex);
                if (endIndex != -1)
                {
                    var thisResponse = queryResult.Substring(startIndex, endIndex - startIndex);

                    var truncindex = thisResponse.IndexOf((char)13);
                    if (truncindex != -1)
                        thisResponse = thisResponse.Substring(0, truncindex);

                    truncindex = thisResponse.IndexOf((char)10);
                    if (truncindex != -1)
                        thisResponse = thisResponse.Substring(0, truncindex);

                    thisResponse = thisResponse.Trim();
                    if (string.IsNullOrEmpty(thisResponse))
                        continue;

                    thisResponse = thisResponse.Replace(" | ", ": ");
                    thisResponse = thisResponse.Replace("| ", "");

                    if (responseString.Contains(thisResponse))
                        continue;

                    responseString += thisResponse + Environment.NewLine;
                }
            }

            if (responseString == "")
            {
                var random = new Random((int)DateTime.Now.ToFileTime());
                var index = random.Next(0, DefaultResponses.Count - 1);
                responseString = DefaultResponses[index] + Environment.NewLine;
            }

            Response.Write(Environment.NewLine + responseString);
            return new HttpStatusCodeResult(200);
        }
    }
}
