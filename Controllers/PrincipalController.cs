using FacturacionWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacturacionWeb.Controllers
{
    [Authorize]
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Index()
        {
            return View();
        }

        public string UsuarioMenu(string usuario) { 
            Response resp = new Response();
            try
            {
                using (BDFacturaEntities context = new BDFacturaEntities())
                {
                    var user = context.Users.Where(x=> x.UserName==usuario).FirstOrDefault();
                    if (user != null) {


                        var query = (from menu in context.Menu
                                    join um in context.Usuario_Menu on menu.ID equals um.ID_Menu
                                    join u in context.Users on um.UserName equals u.UserName
                                    where u.UserName == usuario
                                     select new MenuUsuario
                                     {
                                         NombreMenu = menu.Nombre,
                                         Icono = menu.Icono,
                                         Path = menu.Url
                                     });
                                    
                       resp.Status= 200;
                       resp.Data = query.ToList();
                       return JsonConvert.SerializeObject(resp);
                    }
                    else
                    {
                        resp.Status = 404;
                        resp.Error = "No se encontro el Usuario: " + usuario;
                        return JsonConvert.SerializeObject(resp);
                    }
                }

                
            }
            catch (Exception ex)
            {
                resp.Status = 400;
                resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                return JsonConvert.SerializeObject(resp);
            }
        }
    }
}