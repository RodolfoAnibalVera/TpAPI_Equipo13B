using dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace api_CatalogoProducto.Models
{
    public class ArticuloDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public string Imagenes { get; set; }

        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public decimal Precio
        {
            get; set;

        }
    }
}