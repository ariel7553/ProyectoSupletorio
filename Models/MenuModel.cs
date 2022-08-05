using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturacionWeb.Models
{
    public class MenuModel
    {
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public string Path { get; set; }

        public MenuModel(string nombre, string icono, string path)
        {
            Nombre = nombre;
            Icono = icono;
            Path = path;
        }
    }


}