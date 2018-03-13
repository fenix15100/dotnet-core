using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PR01___Primers_passos_amb_dotnet_core.Models;

namespace PR01___Primers_passos_amb_dotnet_core.Controllers
{
    public class AccountController : Controller
    {
        
        // GET
        public IActionResult Login()
        {
            
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            HttpContext.Session.SetString("user", loginDto.FirstName);
            return RedirectToAction("index", "Home");
        }

        
    }
}