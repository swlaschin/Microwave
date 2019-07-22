using Microsoft.AspNetCore.Mvc;
using Microwave.Web.Models;
using System.Diagnostics;

namespace Microwave.Web.Controllers
{
    public class MicrowaveController : Controller
    {
        public IActionResult Index()
        {
            ViewData["State"] = "to do";
            return View("Index");
        }

        public IActionResult Start(int howLong)
        {
            ViewData["State"] = "to do";
            ViewData["ErrorMessage"] = "to do";

            return View("Index");
        }


        public IActionResult Open()
        {
            ViewData["State"] = "to do";
            ViewData["ErrorMessage"] = "to do";
            return View("Index");
        }

        public IActionResult Close()
        {
            ViewData["State"] = "to do";
            ViewData["ErrorMessage"] = "to do";
            return View("Index");
        }

        //public IActionResult Close()
        //{
        //    var result = Microwave.Api.Close();
        //    ViewData["State"] = Microwave.Api.stateToString(result.State);
        //    ViewData["ErrorMessage"] = Microwave.Api.translateError("en", result.Error);
        //    return View("Index");
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
