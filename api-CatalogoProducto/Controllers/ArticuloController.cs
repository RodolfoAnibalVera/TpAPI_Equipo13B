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
        public void Post([FromBody]ArticuloDto art)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo nuevo = new Articulo();
            nuevo.Codigo = art.Codigo;
            nuevo.Nombre = art.Nombre;
            nuevo.Descripcion = art.Descripcion;
            nuevo.Imagenes = art.Imagenes;
            nuevo.Marca = new Marca { Id = art.IdMarca };
            nuevo.Categoria = new Categoria { Id = art.IdCategoria };
            nuevo.Precio = art.Precio;

            negocio.agregar(nuevo);
        }

        // PUT: api/Articulo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
        }
    }
}
