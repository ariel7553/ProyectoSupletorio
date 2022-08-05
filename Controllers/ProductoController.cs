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
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        public string ListaProductos()
        {
            if (User.IsInRole("Admin"))
            {
                using (BDFacturaEntities context = new BDFacturaEntities())
                {
                    Response resp = new Response();
                    try
                    {
                        resp.Data = context.Producto.ToList();
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
                        resp.Data = context.Producto.Where(x => x.Estado == 1 && x.Stock > 0).ToList();
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


        public string ListaProductosActivos()
        {

            using (BDFacturaEntities context = new BDFacturaEntities())
            {
                Response resp = new Response();
                try
                {
                    resp.Data = context.Producto.Where(x=> x.Estado == 1 && x.Stock > 0).ToList();
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

        public string ChangePointComma(string n)
        {
            return n.Replace(".", ",");
        }

        public string GuardarProducto(string nombre, string pre, string costo, string stock)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {

                Response resp = new Response();

                try
                {
                    var prd = context.Producto.Where(x => x.Nombre == nombre).FirstOrDefault();
                    if(prd == null)
                    {
                        Producto p = new Producto();
                        p.Nombre = nombre;
                        p.Precio = Convert.ToDecimal(ChangePointComma(pre));
                        p.Stock = Convert.ToDecimal(stock);
                        p.Costo = Convert.ToDecimal(ChangePointComma(costo));
                        p.Estado = 1;
                        context.Producto.Add(p);
                        context.SaveChanges();
                        resp.Status = 200;
                    }
                    else
                    {
                        resp.Error = "Ya existe un producto con ese nombre";
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
        public string EditarProducto(int id, string nombre, string pre, string costo, string stock)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {

                Response resp = new Response();

                try
                {
                    var p = context.Producto.Where(x => x.ID == id).FirstOrDefault();
                    if (p != null)
                    {
                        var prd = context.Producto.Where(x => x.Nombre == nombre).FirstOrDefault();



                        if (prd != null && prd.Nombre != p.Nombre)
                        {
                            resp.Error = "Ya existe un producto con ese nombre";
                            resp.Status = 400;
                            
                        }
                        else
                        {
                            p.Nombre = nombre;
                            p.Precio = Convert.ToDecimal(ChangePointComma(pre));
                            p.Stock = Convert.ToDecimal(stock);
                            p.Costo = Convert.ToDecimal(ChangePointComma(costo));
                            p.Estado = 1;
                            context.SaveChanges();
                            resp.Status = 200;
                        }
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

        public string EliminarProducto(int id)
        {
            using (BDFacturaEntities context = new BDFacturaEntities())
            {

                Response resp = new Response();

                try
                {
                    var p = context.Producto.Where(x => x.ID == id).FirstOrDefault();
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