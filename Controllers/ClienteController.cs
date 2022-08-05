using FacturacionWeb.Models;
using Newtonsoft.Json;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TransactionScope = System.Transactions.TransactionScope;

namespace FacturacionWeb.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            //List<Cliente> clientes = ListaClientes();
            return View();
        }


        public string GuardarCliente(string ced, string nom, string ape, string tel, string dir)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {
                using (var scope = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    Response resp = new Response();
                    try
                    {
                        var cl = context.Cliente.Where(x => x.CedRuc == ced).FirstOrDefault();
                        if (cl == null)
                        {
                           Cliente c = new Cliente();
                            c.CedRuc = ced;
                            c.Nombres = nom;
                            c.Apellidos = ape;
                            c.Telefono = tel;
                            c.Direccion = dir;
                            c.Estado = 1;
                            context.Cliente.Add(c);
                            context.SaveChanges();
                            scope.Commit();
                            resp.Status = 200;
                        }
                        else
                        {
                            resp.Error = "Ya existe un cliente con esa cédula";
                            resp.Status = 400;

                        }

                        

                        return JsonConvert.SerializeObject(resp);
                    }
                    catch (Exception ex)
                    {
                        scope.Rollback();
                        resp.Status = 400;
                        resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                        return JsonConvert.SerializeObject(resp);
                    }
                }


            }


        }

        public string EditarCliente(int id, string ced, string nom, string ape, string tel, string dir)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {
                using (var scope = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    Response resp = new Response();
                    try
                    {

                        var cli = context.Cliente.Where(x => x.ID == id).FirstOrDefault();

                        if (cli != null)
                        {
                            // Busqueda si existe un producto con esa cedula

                            var cliDB = context.Cliente.Where(x => x.CedRuc == ced).FirstOrDefault();

                            if(cliDB != null && cliDB.CedRuc != cli.CedRuc)
                            {
                                resp.Error = "Ya existe un cliente con esa cédula";
                                resp.Status = 400;
                                return JsonConvert.SerializeObject(resp);
                            }
                        }

                        var c = context.Cliente.Where(x => x.ID == id).FirstOrDefault();
                        if(c!= null)
                        {

                            c.CedRuc = ced;
                            c.Nombres = nom;
                            c.Apellidos = ape;
                            c.Telefono = tel;
                            c.Direccion = dir;
                            c.Estado = 1;
                            context.SaveChanges();
                            scope.Commit();
                            resp.Status = 200;
                        }
                        else
                        {
                            scope.Rollback();
                            resp.Status = 400;
                            resp.Error = "No se encuentra un cliente con el ID " + id;
                        }
                        

                        return JsonConvert.SerializeObject(resp);
                    }
                    catch (Exception ex)
                    {
                        scope.Rollback();
                        resp.Status = 400;
                        resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                        return JsonConvert.SerializeObject(resp);
                    }
                }


            }


        }
        public string EliminarCliente(int id)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {
                using (var scope = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    Response resp = new Response();
                    try
                    {
                        var c = context.Cliente.Where(x => x.ID == id).FirstOrDefault();
                        if (c != null)
                        {
                            
                            c.Estado = 0;
                            context.SaveChanges();
                            scope.Commit();
                            resp.Status = 200;
                        }
                        else
                        {
                            scope.Rollback();
                            resp.Status = 400;
                            resp.Error = "No se encuentra un cliente con el ID " + id;
                        }


                        return JsonConvert.SerializeObject(resp);
                    }
                    catch (Exception ex)
                    {
                        scope.Rollback();
                        resp.Status = 400;
                        resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                        return JsonConvert.SerializeObject(resp);
                    }
                }


            }


        }

        public string ListaClientes()
        {
            if (User.IsInRole("Admin"))
            {
                using (BDFacturaEntities context = new BDFacturaEntities())
                {

                    Response resp = new Response();
                    try
                    {
                        resp.Data = context.Cliente.ToList();
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
            else
            {
                using (BDFacturaEntities context = new BDFacturaEntities())
                {

                    Response resp = new Response();
                    try
                    {
                        resp.Data = context.Cliente.Where(x => x.Estado == 1).ToList();
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

                
        }


        public string ListaClientesActivos()
        {

            using (BDFacturaEntities context = new BDFacturaEntities())
            {

                Response resp = new Response();
                try
                {
                    resp.Data = context.Cliente.Where(x=> x.Estado ==1).ToList();
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
    }
}