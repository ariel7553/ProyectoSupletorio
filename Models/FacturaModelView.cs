using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturacionWeb.Models
{
    public class FacturaModelView
    {
        public int ID { get; set; }
        public string CedRuc { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public decimal Iva { get; set; }
        public string Empleado { get; set; }
        public int Estado { get; set; }
    }

    public class UsuarioModelView
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public int IdRol { get; set; }
        public int Estado { get; set; }
    }
}