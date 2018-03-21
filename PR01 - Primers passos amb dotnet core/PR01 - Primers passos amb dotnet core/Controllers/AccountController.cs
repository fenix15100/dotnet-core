using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using Novell.Directory.Ldap;
using PR01___Primers_passos_amb_dotnet_core.Models;

namespace PR01___Primers_passos_amb_dotnet_core.Controllers
{
    public class AccountController : Controller
    {
        
        // GET
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("user")!=null)
            {
                return RedirectToAction("index", "Home"); 
                
            }

            if (HttpContext.Session.GetString("error")!=null)
            {
                ViewData["error"] = HttpContext.Session.GetString("error");

            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            
            
            if (CheckLdap(loginDto))
            {
                HttpContext.Session.SetString("user", loginDto.FirstName);
                HttpContext.Session.Remove("error");
                return RedirectToAction("index", "Home");   
            }
            
            HttpContext.Session.SetString("error", "Fallo la Autentificacion");
            return RedirectToAction("Login", "Account"); 
                
            


        }



        public bool CheckLdap(LoginDto loginDto)
        {
            
            var ldapConn = new LdapConnection();
            ldapConn.Connect("192.168.1.101", 389);

            try
            {
               ldapConn.Bind(@"cn="+loginDto.FirstName+",ou=Users,dc=fran,dc=local", loginDto.Password);
                return true;
            }catch (LdapException e)
            {

                Console.WriteLine(e);
                return false;
            }
           
         
            
        }


        public IActionResult GetUsersLdap()
        {
            List<String> ldapusers= new List<String>();
            List<String> atributos= new List<String>();
            
            
            
           var ldapConn = new LdapConnection();
           ldapConn.Connect("192.168.1.101", 389);
        
           string filter = "(ObjectClass=inetOrgPerson)";


            try{
                LdapSearchResults query = ldapConn.Search("ou=Users,dc=fran,dc=local", LdapConnection.SCOPE_SUB, filter, null, false);
                
                
                while (query.hasMore()){
                    
                    
                    
                    try {
                        LdapEntry nextEntry =query.next();
                        ldapusers.Add(nextEntry.DN);
                        atributos.Add(nextEntry.getAttributeSet().ToString());                 

                    }catch(LdapException e) {
                        Console.WriteLine("Error Entry: " + e.LdapErrorMessage);
                        
                    }
                }
            }catch (LdapException e){
                Console.WriteLine("Error Filtro: "+e.LdapErrorMessage);
                throw;
            }
           
            
          
               
            ViewBag.ldapusers = ldapusers;
            ViewBag.atributos = atributos;
            return View();

        }



        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("user") != null)
            {

                HttpContext.Session.Remove("user");
            }


            return RedirectToAction("index", "Home");   
        }

        
    }
}