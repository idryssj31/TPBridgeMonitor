using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BridgeMonitor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BridgeMonitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var bridges = GetBridgeMonitorListFromApi();
            bridges.Sort((x, y) => DateTime.Compare(x.ClosingDate, y.ClosingDate));
            DateTime now = DateTime.Now;
            foreach (var bridge in bridges)
            {
                if (bridge.ClosingDate > now)
                {
                    return View(bridge);
                }
                else
                {
                    continue;
                }
            }
            return View();
        }

        public IActionResult fermeture()
        {
            var bridges = GetBridgeMonitorListFromApi();
            var bridges_before = new List<Bridge>();
            var bridges_after = new List<Bridge>();
            var bridge_result = new fermeture()
            {
                BridgesBefore = bridges_before,
                BridgesAfter = bridges_after
            };
            bridges.Sort((x, y) => DateTime.Compare(x.ClosingDate, y.ClosingDate));
            DateTime now = DateTime.Now;
            foreach (var bridge in bridges)
            {
                if (bridge.ClosingDate > now)
                {
                    bridges_after.Add(bridge);
                }
                else
                {
                    bridges_before.Add(bridge);
                }
            }

            return View(bridge_result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static readonly HttpClient client = new HttpClient();

        // methode pour interroger l'api.

        private static List<Bridge> GetBridgeMonitorListFromApi()
        {

            var stringTask = client.GetStringAsync("https://api.alexandredubois.com/pont-chaban/api.php");
            var myJsonResponse = stringTask.Result;
            var result = JsonConvert.DeserializeObject<List<Bridge>>(myJsonResponse);
            return result;
        }



    }
}