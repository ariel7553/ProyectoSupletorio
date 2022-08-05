using FacturacionWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacturacionWeb.Controllers
{
    [Authorize]
    public class FacturarController : Controller
    {
        // GET: Facturar
        public ActionResult Index()
        {
           
            return View();

        }

        public ActionResult VerFactura(int id = 0)
        {
            Factura f = new Factura();
            using (var context = new BDFacturaEntities())
            {
                f = context.Factura.Include("Cliente").Include("Detalle_Factura").Where(x => x.ID == id).FirstOrDefault();
                if (f != null)
                {
                    f.Detalle_Factura = context.Detalle_Factura.Include("Producto").Where(x => x.ID_Fac == id).ToList();
                }
            }
            ViewBag.Sub = f.Detalle_Factura.Sum(x => x.Cant * x.Precio).ToString("0.00");
            return View(f);
        }

        public ActionResult CrearFactura()
        {
            using (var context = new BDFacturaEntities())
            {
                var factura = context.Factura.Max(x => x.ID);
                ViewBag.idFactura = factura + 1;
                return View();
            }
            
        }
        public string ListarFacturas()
        {
            if (User.IsInRole("Admin"))
            {
                using (BDFacturaEntities context = new BDFacturaEntities())
                {

                    Response resp = new Response();
                    try
                    {
                        var query = (from Fac in context.Factura
                                     join Us in context.Users on Fac.Usuario equals Us.UserName
                                     join Cli in context.Cliente on Fac.ID_Cliente equals Cli.ID

                                     select new FacturaModelView
                                     {
                                         ID = Fac.ID,
                                         CedRuc = Cli.CedRuc,
                                         Cliente = Cli.Nombres + " " + Cli.Apellidos,
                                         Fecha = (DateTime)Fac.Fecha,
                                         Total = Fac.Total,
                                         Iva = Fac.IVA,
                                         Empleado = Us.Nombres,
                                         Estado = Fac.Estado

                                     });

                        resp.Status = 200;

                        resp.Data = query.ToList();
                        return JsonConvert.SerializeObject(resp);
                    }
                    catch (Exception ex)
                    {
                        resp.Status = 404;
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
                        var query = (from Fac in context.Factura
                                     join Us in context.Users on Fac.Usuario equals Us.UserName 
                                     join Cli in context.Cliente on Fac.ID_Cliente equals Cli.ID
                                     where Us.UserName == User.Identity.Name

                                     select new FacturaModelView
                                     {
                                         ID = Fac.ID,
                                         CedRuc = Cli.CedRuc,
                                         Cliente = Cli.Nombres + " " + Cli.Apellidos,
                                         Fecha = (DateTime)Fac.Fecha,
                                         Total = Fac.Total,
                                         Iva = Fac.IVA,
                                         Empleado = Us.Nombres,
                                         Estado = Fac.Estado

                                     });

                        resp.Status = 200;

                        resp.Data = query.ToList();
                        return JsonConvert.SerializeObject(resp);
                    }
                    catch (Exception ex)
                    {
                        resp.Status = 404;
                        resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                        return JsonConvert.SerializeObject(resp);
                    }
                }
            }
            
        }

        public string VerificarStock(int ID_Producto, int cant)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {

                Response resp = new Response();
                try
                {
                    int i = 0;
                    var pro = context.Producto.Where(x => x.ID == ID_Producto).FirstOrDefault();
                    if (pro != null)
                    {
                        if (pro.Stock >= cant)
                        {
                            i = 1;
                        }

                    }

                    resp.Status = 200;
                    resp.Data = i;
                    return JsonConvert.SerializeObject(resp);
                }
                catch (Exception ex)
                {
                    resp.Status = 404;
                    resp.Error = ex.Message.Replace("'", "").Replace("\"", "");
                    return JsonConvert.SerializeObject(resp);
                }
            }
        }

        public string GuardarFactura(Cliente cliente, List<DetalleViewModel> Detalle)
        {
            Response resp = new Response();
            using (BDFacturaEntities context = new BDFacturaEntities())
            {
                using (var scope = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        decimal sub = 0;
                        Factura f = new Factura();
                        foreach (var item in Detalle)
                        {
                            var prod = context.Producto.Where(x => x.ID == item.ID_Pro).FirstOrDefault();
                            if (prod != null)
                            {
                                sub += prod.Precio * item.Cant;
                            }
                        }

                        
                        f.ID_Cliente = cliente.ID;
                        f.Usuario = User.Identity.Name;
                       
                        //decimal sub = Detalle.Sum(d => d.Cant * d.Precio);
                        f.IVA = sub * Convert.ToDecimal(0.12);
                        f.Total = sub + f.IVA;
                        //f.Cliente = cliente;
                        f.Detalle_Factura = new HashSet<Detalle_Factura>();
                        f.Fecha = DateTime.Now;
                        f.Estado = 1;
                        context.Factura.Add(f);
                        context.SaveChanges();


                        foreach (var item in Detalle)
                        {
                            var prod = context.Producto.Where(x => x.ID == item.ID_Pro).FirstOrDefault();
                            if (prod != null)
                            {
                                if (prod.Stock >= item.Cant)
                                {
                                    prod.Stock = prod.Stock - item.Cant;
                                    
                                    context.SaveChanges();
                                    Detalle_Factura detalle_Factura = new Detalle_Factura();
                                    detalle_Factura.ID_Fac = f.ID;
                                    detalle_Factura.Cant = item.Cant;
                                    detalle_Factura.Precio = prod.Precio;
                                    detalle_Factura.ID_Pro = prod.ID;
                                    detalle_Factura.SubTotal = prod.Precio * item.Cant;
                                    detalle_Factura.Estado = 1;
                                    context.Detalle_Factura.Add(detalle_Factura);
                                    context.SaveChanges();

                                }
                                else
                                {
                                    scope.Rollback();
                                    resp.Status = 202;
                                    resp.Error = "Stock Insuficiente para el Producto: " + item.Desc + ", Stock: " + prod.Stock;
                                    return JsonConvert.SerializeObject(resp);
                                }
                            }
                            else
                            {
                                scope.Rollback();
                                resp.Status = 404;
                                resp.Error = "No existe el producto: " + item.Desc;
                                return JsonConvert.SerializeObject(resp);
                            }
                        }

                        scope.Commit();
                        resp.Status = 200;
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
                        //throw;
                        scope.Rollback();
                        resp.Status = 404;
                        resp.Error = e.Message.Replace("'", "").Replace("\"", "");
                        return JsonConvert.SerializeObject(resp);
                    }

                    catch (Exception ex)
                    {
                        scope.Rollback();
                        resp.Status = 404;
                        resp.Error = ex.Message.Replace("'", "").Replace("\"","");
                        return JsonConvert.SerializeObject(resp);
                    }

                }
            }
        }
    }
}