using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturacionWeb.Models
{
    public class Response
    {
        public int Status { get; set; }
        public string Error { get; set; }
        public Object Data { get; set; }
    }
}