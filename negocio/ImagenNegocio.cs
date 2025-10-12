using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ImagenNegocio
    {
        public List<Imagen> listar()
        {
            List<Imagen> lista = new List<Imagen>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, IdArticulo, ImagenUrl FROM IMAGENES");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Imagen aux = new Imagen();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.IdArticulo = (int)datos.Lector["IdArticulo"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];


                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregarImagen(int IdArticulo, String ImagenUrl)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@id, @url)");
                datos.setearParametro("@id", IdArticulo);
                datos.setearParametro("@url", ImagenUrl);
                datos.ejecutarAccion();

            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificarImagen(Imagen imagen)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE IMAGENES SET IdArticulo = @idArticulo, ImagenUrl = @imagenUrl WHERE Id = @idImagen");
                datos.setearParametro("@idArticulo", imagen.IdArticulo);
                datos.setearParametro("@imagenUrl", imagen.ImagenUrl);
                datos.setearParametro("@idImagen", imagen.Id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarImagen(int Id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM IMAGENES WHERE Id = @id");
                datos.setearParametro("@id", Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        //*******Validaciones*******
        public bool existeArticulo(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT 1 FROM ARTICULOS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public bool existeImagen(int idImagen)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT 1 FROM IMAGENES WHERE Id = @idImagen");
                datos.setearParametro("@idImagen", idImagen);
                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
