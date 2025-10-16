using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_CatalogoProducto.Validaciones
{
    public static class ArticuloExistenciaValidator
    {
        public static List<string> ValidarExistencia(List<Articulo> lista, int id)
        {
            var errores = new List<string>();

            if (id <= 0)
            {
                errores.Add("El ID debe ser mayor a cero.");
                return errores;
            }

            if (lista == null || !lista.Any())
            {
                errores.Add("No hay artículos en la base de datos.");
                return errores;
            }

            var existe = lista.Exists(x => x.Id == id);
            if (!existe)
                errores.Add($"No se encontró ningún artículo con ID {id}.");

            return errores;
        }
    }
}