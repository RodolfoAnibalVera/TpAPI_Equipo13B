using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_CatalogoProducto.Validaciones
{
    public static class ArticuloNegocioValidator
    {
        public static bool EstaBaseVacia(List<Articulo> articulos)
        {
            return articulos == null || !articulos.Any();
        }

        public static string MensajeBaseVacia()
        {
            return "No hay artículos disponibles.";
        }

        public static string MensajeErrorGeneral()
        {
            return "Ocurrió un error al obtener los artículos.";
        }
    }
}