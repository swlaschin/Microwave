using Microsoft.AspNetCore.Mvc;
using Microwave.Web.Models;
using System.Diagnostics;

namespace Microwave.Web.Controllers
{
    public class MicrowaveController : Controller
    {
        public IActionResult Index()
        {
            var state = Microwave.Api.GetState();
            ViewData["State"] = Microwave.Api.StateToString(state);
            ViewData["ErrorMessage"] = "";
            return View("Index");
        }

        public IActionResult Start(int howLong)
        {
            var result = Microwave.Api.Start(howLong);
            ViewData["State"] = Microwave.Api.StateToString(result.State);
            ViewData["ErrorMessage"] = result.Error;
            return View("Index");
        }


        public IActionResult Open()
        {
            var result = Microwave.Api.Open();
            ViewData["State"] = Microwave.Api.StateToString(result.State);
            ViewData["ErrorMessage"] = result.Error;
            return View("Index");
        }

        public IActionResult Close()
        {
            var result = Microwave.Api.Close();
            ViewData["State"] = Microwave.Api.StateToString(result.State);
            ViewData["ErrorMessage"] = result.Error;
            return View("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
