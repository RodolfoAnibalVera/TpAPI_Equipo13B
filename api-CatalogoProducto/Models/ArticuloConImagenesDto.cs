﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_CatalogoProducto.Models
{
    public class ArticuloConImagenesDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public List<string> Imagenes { get; set; }
    }
}