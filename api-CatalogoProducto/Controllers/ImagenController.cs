using api_CatalogoProducto.Models;
using api_CatalogoProducto.Validaciones;
using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api_CatalogoProducto.Controllers
{
    public class ImagenController : ApiController
    {
        // GET: api/Imagen
        public IEnumerable<Imagen> Get()
        {
            ImagenNegocio negocio = new ImagenNegocio();

            return negocio.listar();
        }

        /**********************************************************/
        // GET: api/Imagen/5
        public HttpResponseMessage Get(int id)
        {
            ImagenNegocio negocio = new ImagenNegocio();
            ImagenValidator validar = new ImagenValidator();
           
            try
            {
                string error = validar.ValidarIdImagen(id);
                if (!string.IsNullOrEmpty(error)) return Request.CreateResponse(HttpStatusCode.NotFound, error);

                List<Imagen> lista = negocio.listar();

                var imagen = lista.Find(x => x.Id == id);
                return Request.CreateResponse(HttpStatusCode.OK, imagen);
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
            
        }

        /**********************************************************/
        /*Metodo GET todas las imagenes por IdArticulo*/

        [HttpGet]                                      //[HttpGet] Este método se ejecuta cuando se hace una petición GET (lectura)
        [Route("api/Imagen/PorArticulo/{idArticulo}")] // [Route] Define la ruta personalizada (URL) que invoca este método
        public IEnumerable<Imagen> GetPorArticulo(int idArticulo)
        {
            ImagenNegocio negocio = new ImagenNegocio();
            return negocio.listarPorArticulo(idArticulo);
        }

        /**********************************************************/
        // POST: api/Imagen
        public HttpResponseMessage Post([FromBody] ImagenDto img)
        {
            ImagenNegocio negocio = new ImagenNegocio();
            ImagenValidator validar = new ImagenValidator();
            var listaDeErrores = new List<string>();

            try
            {
                listaDeErrores = validar.ValidarImagen(img);

                if (listaDeErrores.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, listaDeErrores);
                }
                negocio.agregarVariasImagenes(img.IdArticulo, img.Imagenes);

                return Request.CreateResponse(HttpStatusCode.OK, "Imagen agregada correctamente.");
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }

        }

        /**********************************************************/
        // PUT: api/Imagen/5
        public HttpResponseMessage Put(int id, [FromBody] ImagenDtoModificar img)
        {
            ImagenNegocio negocio = new ImagenNegocio();
            Imagen nueva = new Imagen();
            ImagenDto imagenDto = new ImagenDto();

            ImagenValidator validar = new ImagenValidator();
            var listaDeErrores = new List<string>();

            nueva.IdArticulo = img.IdArticulo;
            nueva.ImagenUrl = img.ImagenUrl;

            imagenDto.IdArticulo = img.IdArticulo;
            imagenDto.Imagenes = new List<string>();
            imagenDto.Imagenes.Add(img.ImagenUrl);

            try
            {
                listaDeErrores = validar.ValidarImagen(imagenDto);

                string error = validar.ValidarIdImagen(id);
                if (!string.IsNullOrEmpty(error)) listaDeErrores.Add(error);

                if (listaDeErrores.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, listaDeErrores);
                }

                negocio.modificarImagenConId(id, nueva);
                return Request.CreateResponse(HttpStatusCode.OK, "Imagen modificada correctamente");
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }

        /**********************************************************/
        // DELETE: api/Imagen/5
        public HttpResponseMessage Delete(int id)
        {
            ImagenNegocio negocio = new ImagenNegocio();
            var validar = new ImagenValidator();

            try
            {
                string error = validar.ValidarIdImagen(id);
                if (!string.IsNullOrEmpty(error)) return Request.CreateResponse(HttpStatusCode.NotFound, error);

                negocio.eliminarImagen(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Imagen eliminada correctamente");
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }
    }
}
