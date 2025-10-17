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
using api_CatalogoProducto.Validaciones;

namespace api_CatalogoProducto.Controllers
{
    public class ArticuloController : ApiController
    {
        // GET: api/Articulo
        public HttpResponseMessage Get()
        {
            try
            {
                ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                ImagenNegocio imagenNegocio = new ImagenNegocio();

                var articulos = articuloNegocio.listar();
                var imagenes = imagenNegocio.listar();

                if (ArticuloNegocioValidator.EstaBaseVacia(articulos))
                {
                    return Request.CreateResponse(
                        HttpStatusCode.OK,
                        new
                        {
                            mensaje = ArticuloNegocioValidator.MensajeBaseVacia(),
                            articulos = new List<ArticuloConImagenesDto>()
                        });
                }

                var resultado = articulos.Select(art => new ArticuloConImagenesDto
                {
                    Id = art.Id,
                    Codigo = art.Codigo,
                    Nombre = art.Nombre,
                    Descripcion = art.Descripcion,
                    Marca = art.Marca?.Descripcion,
                    Categoria = art.Categoria?.Descripcion,
                    Precio = art.Precio,
                    Imagenes = imagenes
                        .Where(img => img.IdArticulo == art.Id)
                        .Select(img => img.ImagenUrl)
                        .ToList()
                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, resultado);
            }
            catch (Exception)
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    ArticuloNegocioValidator.MensajeErrorGeneral());
            }
        }

        // GET: api/Articulo/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                ImagenNegocio imagenNegocio = new ImagenNegocio();

                var articulos = articuloNegocio.listar();
                var errores = ArticuloExistenciaValidator.ValidarExistencia(articulos, id);

                if (errores.Any())
                    return Request.CreateResponse(HttpStatusCode.NotFound, errores);

                var articulo = articulos.Find(x => x.Id == id);
                var imagenes = imagenNegocio.listar()
                    .Where(img => img.IdArticulo == id)
                    .Select(img => img.ImagenUrl)
                    .ToList();

                var dto = new ArticuloConImagenesDto
                {
                    Id = articulo.Id,
                    Codigo = articulo.Codigo,
                    Nombre = articulo.Nombre,
                    Descripcion = articulo.Descripcion,
                    Marca = articulo.Marca?.Descripcion,
                    Categoria = articulo.Categoria?.Descripcion,
                    Precio = articulo.Precio,
                    Imagenes = imagenes
                };

                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error al obtener el artículo.");
            }
        }

        // POST: api/Articulo
        public HttpResponseMessage Post([FromBody]ArticuloDto art)
        {
            try
            {
                decimal precioDecimal;
                var errores = ArticuloValidator.Validar(art, out precioDecimal);

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
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }

        // PUT: api/Articulo/5
        public HttpResponseMessage Put(int id, [FromBody]ArticuloDto art)
        {
            try
            {
                decimal precioDecimal;
                var errores = ArticuloValidator.Validar(art, out precioDecimal, modoEdicion: true, idActual: id);
                //errores.Add(ArticuloValidator.ValidarIdArticulo(id));

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
                    Precio = precioDecimal,
                    Id = id
                };

                ArticuloNegocio negocio = new ArticuloNegocio();
                negocio.modificar(nuevo);

                return Request.CreateResponse(HttpStatusCode.OK, "Artículo modificado correctamente.");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }

        // DELETE: api/Articulo/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var negocio = new ArticuloNegocio();

                bool existe = false;
                foreach (var art in negocio.listar())
                {
                    if (art.Id == id)
                    {
                        existe = true;
                        break;
                    }
                }

                if (!existe)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No se encontró el artículo con id {id}.");

                negocio.eliminar(id);
                
                return Request.CreateResponse(HttpStatusCode.OK, "El artículo se eliminó correctamente");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error al eliminar el artículo.");
            }

        }

        [HttpGet]
        [Route("api/Articulo/buscar")]
        public HttpResponseMessage Buscar(string campo, string criterio, string filtro)
        {
            try
            {
                if (campo == null || criterio == null || filtro == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Debe completar todos los campos para realizar la búsqueda.");
                }

                // Normalizo los textos
                campo = campo.Trim();
                criterio = criterio.Trim();
                filtro = filtro.Trim();

                // Validar campo permitido
                if (campo != "Precio" && campo != "Nombre" && campo != "Descripcion")
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Campo inválido. Use 'Precio', 'Nombre' o 'Descripcion'.");
                }

                // Validar criterio según el campo
                if (campo == "Precio")
                {
                    if (criterio != "Mayor a" && criterio != "Menor a" && criterio != "Igual a")
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Criterio inválido para el campo Precio.");
                    }

                    decimal valor;
                    if (!decimal.TryParse(filtro, out valor))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "El filtro para el campo Precio debe ser un número.");
                    }
                }
                else
                {
                    if (criterio != "Comienza con" && criterio != "Termina con" && criterio != "Contiene")
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Criterio inválido. Use 'Comienza con', 'Termina con' o 'Contiene'.");
                    }
                }

                // Reutilizo el método filtrar de la capa negocio
                ArticuloNegocio negocio = new ArticuloNegocio();
                List<Articulo> lista = negocio.filtrar(campo, criterio, filtro);

                return Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error al buscar los artículos.");
            }
        }

    }
}
