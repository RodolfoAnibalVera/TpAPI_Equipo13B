using api_CatalogoProducto.Models;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace api_CatalogoProducto.Validaciones
{
    public class ImagenValidator
    {

        public List<string> ValidarImagen(ImagenDto img)
        {

            List<string> listaDeErrores = new List<string>();

            if (img == null)
            {
                listaDeErrores.Add("No se recibió ningún dato o el formato del JSON es inválido");
                return listaDeErrores;
            }

            if (img.IdArticulo <= 0)
            {
                listaDeErrores.Add("El IdArticulo debe ser mayor que cero");
            }
            else
            {
                var articuloNegocio = new ArticuloNegocio();

                //Any() devuelve True si encuentra una coincidencia en la DB de Articulos, False si no encuentra. 
                var articuloExiste = articuloNegocio.listar().Any(a => a.Id == img.IdArticulo);

                if (!articuloExiste)
                    listaDeErrores.Add($"No existe un artículo con Id = {img.IdArticulo}");
            }

           
            if (img.Imagenes == null || img.Imagenes.Count == 0)
            {
                listaDeErrores.Add("Debe incluir al menos una URL de imagen o el formato de Json es incorrecto");
            }

            
            if (img.Imagenes == null || img.Imagenes.Count == 0)
            {
                listaDeErrores.Add("Debe incluir al menos una URL de imagen o el formato de Json es incorrecto");
            }
            else
            {
                foreach (var url in img.Imagenes)
                {
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        listaDeErrores.Add("Se encontró una URL vacía o el formato de Json es incorrecto");
                        continue;
                    }

                    if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        listaDeErrores.Add($"La URL '{url}' no tiene un formato válido");
                    }
                }
            }

            return listaDeErrores;
        }

        public string ValidarIdImagen(int id)
        {
            string Errores = null;

            if (id <= 0)
            {
                Errores = "El Id debe ser mayor que cero";
            }
            else
            {
                var imagenNegocio = new ImagenNegocio();

                //Any() devuelve True si encuentra una coincidencia en la DB de Imagenes, False si no encuentra. 
                var imagenExiste = imagenNegocio.listar().Any(a => a.Id == id);

                if (!imagenExiste)
                    Errores = $"No existe una imagen con Id = {id}";
            }

            return Errores;
        }
    }
}