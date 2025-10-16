using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using api_CatalogoProducto.Models;
using dominio;
using negocio;
using System.Globalization;

namespace api_CatalogoProducto.Validaciones
{
    public static class ArticuloValidator
    {
        public static List<string> Validar(ArticuloDto art, out decimal precioDecimal, bool modoEdicion = false, int idActual = 0)
        {
            List<string> errores = new List<string>();
            precioDecimal = 0;

            if (art == null)
            {
                errores.Add("No se recibió ningún dato o el formato del JSON es inválido.");
                return errores;
            }

            if (string.IsNullOrWhiteSpace(art.Codigo))
                errores.Add("El código es obligatorio.");

            ArticuloNegocio negocio = new ArticuloNegocio();
            var existentes = negocio.listar();

            if (!string.IsNullOrWhiteSpace(art.Codigo))
            {
                bool codigoDuplicado = modoEdicion
                    ? existentes.Any(a => a.Codigo == art.Codigo && a.Id != idActual)
                    : existentes.Any(a => a.Codigo == art.Codigo);

                if (codigoDuplicado)
                    errores.Add("Ya existe un artículo con el mismo código.");
            }

            if (string.IsNullOrWhiteSpace(art.Nombre))
                errores.Add("El nombre es obligatorio.");

            if (art.IdMarca <= 0)
                errores.Add("La marca debe ser un número válido mayor a cero.");

            if (art.IdCategoria <= 0)
                errores.Add("La categoría debe ser un número válido mayor a cero.");

            if (string.IsNullOrWhiteSpace(art.Precio))
                errores.Add("El precio es obligatorio.");
            else
            {
                var cultura = new CultureInfo("es-AR");
                if (!decimal.TryParse(art.Precio, NumberStyles.Number, cultura, out precioDecimal) || precioDecimal <= 0)
                    errores.Add("El precio debe ser un número válido mayor a cero.");
            }

            if (!Uri.IsWellFormedUriString(art.Imagenes, UriKind.Absolute))
                errores.Add("El formato de URL imagen es inválido.");

            if (art.IdMarca > 0)
            {
                MarcaNegocio marcaNegocio = new MarcaNegocio();
                Marca marca = marcaNegocio.Listar().Find(m => m.Id == art.IdMarca);
                if (marca == null)
                    errores.Add("La marca no existe.");
            }

            if (art.IdCategoria > 0)
            {
                CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                Categoria categoria = categoriaNegocio.Listar().Find(c => c.Id == art.IdCategoria);
                if (categoria == null)
                    errores.Add("La categoría no existe.");
            }

            return errores;
        }

        public static string ValidarIdArticulo(int id)
        {
            string Errores = "";

            if (id <= 0)
            {
                Errores = "El Id debe ser mayor que cero.";
            }
            else
            {
                var articuloNegocio = new ArticuloNegocio();

                //Any() devuelve True si encuentra una coincidencia en la DB de Articulo, False si no encuentra. 
                var articuloExiste = articuloNegocio.listar().Any(a => a.Id == id);

                if (!articuloExiste)
                    Errores = $"No existe un Articulo con Id = {id}.";
            }

            return Errores;
        }

    }
}