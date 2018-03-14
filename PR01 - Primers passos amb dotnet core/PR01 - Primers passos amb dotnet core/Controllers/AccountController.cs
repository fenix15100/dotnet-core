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
            //todo CheckLdap(loginDto) if true meto en sesion y retorno viesta home si no meto en sesion error y retorno a form
            // alli handeo la ssesion para inyectar el error.
            HttpContext.Session.SetString("user", loginDto.FirstName);
            return RedirectToAction("index", "Home");
        }

        
    }
}