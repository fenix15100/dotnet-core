using System;
using M8_UF3_Examen.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Novell.Directory.Ldap;

namespace M8_UF3_Examen.Controllers
{
    public class AccountController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(UserCredentials credentials)
        {
            if (IsValidUser(credentials))
            {
                HttpContext.Session.SetString("User",credentials.User);
                return RedirectToAction("Index", "Home");
            }
            return View(credentials);
        }

        private bool IsValidUser(UserCredentials credentials)
        {
            
            //Creamos el objeto Ldap
            LdapConnection ldapConn = new LdapConnection();
            
            //Abrimos el Socket contra el servidor Ldap
            ldapConn.Connect("192.168.123.159", 389);

            
            try
            {
                //Intentamos autentificarnos contra el dominio Ldap, utilizando los datos del objeto LoginDto
                //Si no salta la excepcion retornamos true si no es asi retornamos false
                ldapConn.Bind(@"cn="+credentials.User+",dc=examen,dc=ies", credentials.Pass);
                return true;
            }catch (LdapException e)
            {
                //Debug
                Console.WriteLine(e);
                return false;
            }
        }
    }
}