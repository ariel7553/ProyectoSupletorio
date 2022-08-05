using FacturacionWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FacturacionWeb.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            return View();
        }

        public bool ValidarPassword(string pass)
        {
            var lowerCaseRegex = "(?=.*[a-z])";
            var upperCaseRegex = "(?=.*[A-Z])";
            var symbolsRegex = @"(?=.*[!@#$%^&+/',\""*])";
            var numericRegex = "(?=.*[0-9])";

          

            if (!new Regex(@"^" + lowerCaseRegex + "").IsMatch(pass))
            {
                return false;
            }
            if (!new Regex(@"^" + upperCaseRegex + "").IsMatch(pass))
            {
                return false;

            }
            if (!new Regex(@"^" + symbolsRegex + "").IsMatch(pass))
            {
                return false;

            }
            if (!new Regex(@"^" + numericRegex + "").IsMatch(pass))
            {
                return false;

            }

            if (pass.Length <= 8)
            {
                return false;

            }
            return true;
        }

        public string EditarUsuario(int id, string nombre, string usuario, string pass, string rol)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {

                Response resp = new Response();

                try
                {


                    var u = context.Users.Where(x => x.UserName == usuario).FirstOrDefault();

                    if (!ValidarPassword(pass))
                    {
                        resp.Error = "Password no cumple las politicas (Mayusculas, Minusculas, Numeros y Caracteres especiales)";
                        resp.Status = 400;
                        return JsonConvert.SerializeObject(resp);
                    }

                    if (u != null)
                    {

                        // Borrar Todos los roles
                        context.UserRolesMapping.RemoveRange(context.UserRolesMapping.Where(x => x.UserID == id));
                        context.SaveChanges();

                        // Borrar todos los menus
                        context.Usuario_Menu.RemoveRange(context.Usuario_Menu.Where(x => x.UserName == usuario));
                        context.SaveChanges();

                        u.Nombres = nombre;
                        u.UserPassword = pass;
                        u.Estado = 1;
                        context.SaveChanges();

                        int r = int.Parse(rol);

                        UserRolesMapping urm = new UserRolesMapping();
                        urm.UserID = u.ID;
                        urm.RoleID = r;

                        context.UserRolesMapping.Add(urm);
                        context.SaveChanges();

                        // Verificar el Rol que viene

                        if (r == 1)
                        {
                            Usuario_Menu um = new Usuario_Menu();
                            um.ID_Menu = 1;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 2;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 5;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 6;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 8;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);

                            context.SaveChanges();
                        }
                        else
                        {
                            Usuario_Menu um = new Usuario_Menu();
                            um.ID_Menu = 6;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 2;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 5;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);

                            context.SaveChanges();
                        }
                        resp.Status = 200;
                    }
                    else
                    {
                        resp.Error = "No se encontro un Usuario con ese nombre";
                        resp.Status = 400;
                    }

                    return JsonConvert.SerializeObject(resp);
                }
                catch (Exception ex)
                {

                    resp.Status = 400;
                    resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                    return JsonConvert.SerializeObject(resp);
                }
            }

        }
        public string GuardarUsuario(string nombre, string usuario, string pass, string rol)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {

                Response resp = new Response();

                try
                {
                    var prd = context.Users.Where(x => x.UserName == usuario).FirstOrDefault();

                    if (!ValidarPassword(pass))
                    {
                        resp.Error = "Password no cumple las politicas (Mayusculas, Minusculas, Numeros y Caracteres especiales)";
                        resp.Status = 400;
                        return JsonConvert.SerializeObject(resp);
                    }

                    if (prd == null )
                    {
                        Users u = new Users();
                        u.Nombres = nombre;
                        u.UserName = usuario;
                        u.UserPassword = pass;
                        u.Estado = 1;
                        context.Users.Add(u);
                        context.SaveChanges();

                        int r = int.Parse(rol);
                        var idNum = context.UserRolesMapping.Max(x => x.ID);

                        UserRolesMapping urm = new UserRolesMapping();
                        
                        urm.UserID = u.ID;
                        urm.RoleID = r;

                        context.UserRolesMapping.Add(urm);
                        context.SaveChanges();

                        // Verificar el Rol que viene

                        if (r == 1)
                        {
                            Usuario_Menu um = new Usuario_Menu();
                            um.ID_Menu = 1;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 2;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 5;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 6;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 8;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);

                            context.SaveChanges();
                        }
                        else
                        {
                            Usuario_Menu um = new Usuario_Menu();
                            um.ID_Menu = 6;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 2;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);
                            context.SaveChanges();

                            um.ID_Menu = 5;
                            um.UserName = u.UserName;
                            um.Estado = 1;
                            context.Usuario_Menu.Add(um);

                            context.SaveChanges();
                        }
                        resp.Status = 200;
                    }
                    else
                    {
                        resp.Error = "Ya existe un Usuario con ese nombre";
                        resp.Status = 400;
                    }

                    return JsonConvert.SerializeObject(resp);
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    resp.Status = 400;
                    resp.Error = e.Message.Replace("'", "").Replace("\"", "");
                    return JsonConvert.SerializeObject(resp);
                }
            }

        }
        public string ObtenerRoles()
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {
                Response resp = new Response();
                try
                {
                    resp.Data = context.RoleMaster.ToList(); ;
                    resp.Status = 200;
                    return JsonConvert.SerializeObject(resp);
                }
                catch (Exception ex)
                {
                    resp.Status = 400;
                    resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                    return JsonConvert.SerializeObject(resp);
                }

            }
        }

        public string ListarUsuario()
        {

            using (BDFacturaEntities context = new BDFacturaEntities())
            {
                Response resp = new Response();
                try
                {
                    var userRoles = (from user in context.Users
                                     join roleMapping in context.UserRolesMapping
                                     on user.ID equals roleMapping.UserID
                                     join role in context.RoleMaster
                                     on roleMapping.RoleID equals role.ID
                                     select new UsuarioModelView
                                     {
                                         ID = user.ID,
                                         Nombre = user.Nombres,
                                         Usuario = user.UserName,
                                         Estado = user.Estado,
                                         Password = user.UserPassword,
                                         Rol = role.RollName,
                                         IdRol = role.ID
                                     }

                                 ).ToArray();
                    resp.Data = userRoles;
                    resp.Status = 200;
                    return JsonConvert.SerializeObject(resp);
                }
                catch (Exception ex)
                {
                    resp.Status = 400;
                    resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                    return JsonConvert.SerializeObject(resp);
                }
                
            }


        }

        public string EliminarUsuario(int id)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {

                Response resp = new Response();

                try
                {
                    var p = context.Users.Where(x => x.ID == id).FirstOrDefault();
                    if (p != null)
                    {
                        p.Estado = 0;
                        context.SaveChanges();
                        resp.Status = 200;

                    }
                    else
                    {
                        resp.Status = 400;
                        resp.Error = "No se pudo encontrar un product con ese ID";

                    }
                    return JsonConvert.SerializeObject(resp);

                    //context.Producto.Add(p);


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
}