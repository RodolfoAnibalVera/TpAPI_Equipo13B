using api_CatalogoProducto.Models;
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

        // GET: api/Imagen/5
        public Imagen Get(int id)
        {
            ImagenNegocio negocio = new ImagenNegocio();
            List<Imagen> lista = negocio.listar();

            return lista.Find(x => x.Id == id);
        }

        // POST: api/Imagen
        public void Post([FromBody] ImagenDto img)
        {
            ImagenNegocio negocio = new ImagenNegocio();

            try
            {
                negocio.agregarVariasImagenes(img.IdArticulo, img.Imagenes);
            }
            catch (Exception ex)
            {

                Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }

        }

        // PUT: api/Imagen/5
        public void Put([FromBody] ImagenDto img)
        {
            ImagenNegocio negocio = new ImagenNegocio();

            try
            {
                negocio.modificarVariasImagenes(img.IdArticulo, img.Imagenes);
            }
            catch (Exception ex)
            {

                Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }

        // DELETE: api/Imagen/5
        public void Delete(int id)
        {
            ImagenNegocio negocio = new ImagenNegocio();

            try
            {
                negocio.eliminarImagen(id);
            }
            catch (Exception ex)
            {

                Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }
    }
}
