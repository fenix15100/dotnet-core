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
            //Creamos el objeto Ldap
            LdapConnection ldapConn = new LdapConnection();
            
            //Abrimos el Socket contra el servidor Ldap
            ldapConn.Connect("192.168.1.102", 389);

            
            try
            {
               //Intentamos autentificarnos contra el dominio Ldap, utilizando los datos del objeto LoginDto
               //Si no salta la excepcion retornamos true si no es asi retornamos false
               ldapConn.Bind(@"cn="+loginDto.FirstName+",ou=Users,dc=fran,dc=local", loginDto.Password);
                return true;
            }catch (LdapException e)
            {
                //Debug
                Console.WriteLine(e);
                return false;
            }
           
         
            
        }


        public IActionResult GetUsersLdap()
        {
            //Listas que utilizare para almacenar los usuarios de Ldap y sus respectivos atributos
            List<String> ldapusers= new List<String>();
            List<String> atributos= new List<String>();
            
            
           
            //Abrimos Socket contra el server
           LdapConnection ldapConn = new LdapConnection();
           ldapConn.Connect("192.168.1.102", 389);
        
            //Definimos un filtro Ldap utilizado mas tarde, en este caso solo queremos los objetos tipo usuario
           string filter = "(ObjectClass=inetOrgPerson)";


            try{
                //Hacemos la busqueda
                LdapSearchResults query = ldapConn.Search("dc=fran,dc=local", LdapConnection.SCOPE_SUB, filter, null, false);
                
                
                //Recorremos la colecion de objetos Ldap
                while (query.hasMore()){
                    
                    
                    
                    try {
                        //Obtenemos el usuario iterado y obtenemos su DN y los metemos en la lista, hacemos lo mismo con
                        //TODOS sus atributos(Por comodidad he hecho un toString)
                        //En vez de recorrorerlos 1 a 1.
                        
                        LdapEntry nextEntry =query.next();
                        ldapusers.Add(nextEntry.DN);
                        atributos.Add(nextEntry.getAttributeSet().ToString());                 

                    }catch(LdapException e) {
                        //Si hubiera algun fallo el la obtencion del usuario iterado lo logeo y continuo
                        Console.WriteLine("Error Entry: " + e.LdapErrorMessage);
                        
                    }
                }
            }catch (LdapException e){
                //Si la busqueda ha fallado lo logueo y lanzo una excepcion para parar la app
                Console.WriteLine("Error Filtro: "+e.LdapErrorMessage);
                throw;
            }
            
            //Cargo las dos listas en la array dinamica de la vista
             
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