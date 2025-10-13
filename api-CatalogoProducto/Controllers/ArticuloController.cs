using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls.WebParts;
using api_CatalogoProducto.Models;
using System.Globalization;

namespace api_CatalogoProducto.Controllers
{
    public class ArticuloController : ApiController
    {
        // GET: api/Articulo
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            
            return negocio.listar();
        }

        // GET: api/Articulo/5
        public Articulo Get(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> lista = negocio.listar();

            return lista.Find(x => x.Id == id);
        }

        // POST: api/Articulo
        public HttpResponseMessage Post([FromBody]ArticuloDto art)
        {
            try
            {
                List<string> errores = new List<string>();

                if (art == null)
                {
                    errores.Add("No se recibió ningún dato o el formato del JSON es inválido.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errores);
                }

                if (string.IsNullOrWhiteSpace(art.Codigo))
                    errores.Add("El código es obligatorio.");

                if (string.IsNullOrWhiteSpace(art.Nombre))
                    errores.Add("El nombre es obligatorio.");

                if (art.IdMarca <= 0)
                    errores.Add("La marca debe ser un número válido mayor a cero.");

                if (art.IdCategoria <= 0)
                    errores.Add("La categoría debe ser un número válido mayor a cero.");

                decimal precioDecimal = 0;
                var cultura = new CultureInfo("es-AR");

                if (string.IsNullOrWhiteSpace(art.Precio))
                    errores.Add("El precio es obligatorio.");
                else if (!decimal.TryParse(art.Precio, NumberStyles.Number, cultura, out precioDecimal) || precioDecimal <= 0)
                    errores.Add("El precio debe ser un número válido mayor a cero.");

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

                if (errores.Any())
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errores);

                Articulo nuevo = new Articulo
                {
                    Codigo = art.Codigo,
                    Nombre = art.Nombre,
                    Descripcion = art.Descripcion,
                    Imagenes = art.Imagenes,
                    Marca = new Marca { Id = art.IdMarca },
                    Categoria = new Categoria { Id = art.IdCategoria },
                    Precio = precioDecimal
                };

                ArticuloNegocio negocio = new ArticuloNegocio();
                negocio.agregar(nuevo);

                return Request.CreateResponse(HttpStatusCode.OK, "Artículo agregado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }

        // PUT: api/Articulo/5
        public void Put(int id, [FromBody]ArticuloDto art)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo nuevo = new Articulo();
            nuevo.Codigo = art.Codigo;
            nuevo.Nombre = art.Nombre;
            nuevo.Descripcion = art.Descripcion;
            nuevo.Imagenes = art.Imagenes;
            nuevo.Marca = new Marca { Id = art.IdMarca };
            nuevo.Categoria = new Categoria { Id = art.IdCategoria };
            
            nuevo.Id = id;

            negocio.modificar(nuevo);
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
        }
    }
}
