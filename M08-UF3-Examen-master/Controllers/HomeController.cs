using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using M8_UF3_Examen.Models;
using Microsoft.AspNetCore.Http;
using Novell.Directory.Ldap;

namespace M8_UF3_Examen.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("User") == null) return RedirectToAction("Login", "Account");
            ViewData["User"] = HttpContext.Session.GetString("User");
            return View();
        }

        public IActionResult Users()
        {
            if (HttpContext.Session.GetString("User") == null) return RedirectToAction("Login", "Account");
            ViewData["User"] = HttpContext.Session.GetString("User");

            List<LdapUser> users = GetUsers();

            ViewBag.ldapusers = users;
            return View();
        }

        private List<LdapUser> GetUsers(){
            List<LdapUser> users = new List<LdapUser>();

            //Abrimos Socket contra el server
            LdapConnection ldapConn = new LdapConnection();
            ldapConn.Connect("192.168.123.159", 389);

            //Definimos un filtro Ldap utilizado mas tarde, en este caso solo queremos los objetos tipo usuario
            string filter = "(ObjectClass=inetOrgPerson)";



            //Hacemos la busqueda
             LdapSearchResults query =ldapConn.Search("dc=examen,dc=ies", LdapConnection.SCOPE_SUB, filter, null, false);



            foreach (var element in query)
            {
                LdapUser user = new LdapUser();
                
                

                user.DN = element.DN;
                user.Name = element.getAttribute("givenName").ToString();
                user.Surname = element.getAttribute("sn").ToString();
                users.Add(user);

            }

            return users;


        }
    }
}