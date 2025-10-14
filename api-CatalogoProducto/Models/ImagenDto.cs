using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace api_CatalogoProducto.Models
{
    public class ImagenDto
    {
        public int IdArticulo { get; set; }
        public List<string> Imagenes { get; set; }
    }
}