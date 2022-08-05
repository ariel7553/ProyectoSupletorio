using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacturacionWeb.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        [DisplayName("Usuario")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Contraseña")]
        [Required]
        public string UserPassword { get; set; }
        public string Nombres { get; set; }
        public int Estado { get; set; }
    }
}