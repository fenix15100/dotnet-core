using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Novell.Directory.Ldap;
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
            
            if (CheckLdap(loginDto))
            {
                HttpContext.Session.SetString("user", loginDto.FirstName);
                return RedirectToAction("index", "Home");   
            }
            
            HttpContext.Session.SetString("error", "Fallo la Autentificacion");
            return RedirectToAction("Login", "Account"); 
                
            


        }



        public bool CheckLdap(LoginDto loginDto)
        {
            
            var ldapConn = new LdapConnection();
            ldapConn.Connect("192.168.123.240", 389);

            try
            {
               ldapConn.Bind(@"cn="+loginDto.FirstName+",dc=fran,dc=local", loginDto.Password);
                return true;
            }catch (Exception e)
            {

                Console.WriteLine(e);
                return false;
            }
           
         
            
        }


        public LdapSearchResults GetResult()
        {   
            var ldapConn = new LdapConnection();
            ldapConn.Connect("192.168.123.240", 389);
            
            string filter = "(ObjectClass=*)";
            
            var query = ldapConn.Search("dc=fran,dc=local", LdapConnection.SCOPE_SUB, filter, null, false);

            return query;

        }

        
    }
}