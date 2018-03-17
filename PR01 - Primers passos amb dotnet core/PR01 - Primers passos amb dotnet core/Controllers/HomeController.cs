using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PR01___Primers_passos_amb_dotnet_core.Models;

namespace PR01___Primers_passos_amb_dotnet_core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
           
            String data=HttpContext.Session.GetString("user");

            if (data != null)
            {
                ViewData["Message"] = data;
                return View();
            }
            
           
                
           return RedirectToAction("Login", "Account");
            
          
               
            
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}