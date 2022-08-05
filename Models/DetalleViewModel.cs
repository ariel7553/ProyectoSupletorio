using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturacionWeb.Models
{
    public class DetalleViewModel
    {
        public int Num { get; set; }
        public int ID_Pro { get; set; }
        public string Desc { get; set; }
        public decimal Cant { get; set; }
        public decimal Precio { get; set; }
        public decimal Sub { get; set; }
        public DetalleViewModel()
        {

        }
    }
}