using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio, " +
                    "ISNULL(M.Descripcion,'') AS Marca, " +
                    "ISNULL(C.Descripcion,'') AS Categoria, " +
                    "ISNULL(I.ImagenUrl,'')   AS ImagenUrl " +
                    "FROM ARTICULOS A " +
                    "LEFT JOIN MARCAS M ON A.IdMarca = M.Id " +
                    "LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id " +
                    "OUTER APPLY (SELECT TOP 1 ImagenUrl FROM IMAGENES I WHERE I.IdArticulo = A.Id ORDER BY Id) I");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Imagenes = (string)datos.Lector["ImagenUrl"];

                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    lista.Add(aux);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) " +
                 "VALUES ('" + nuevo.Codigo + "', '" + nuevo.Nombre + "', '" +
                nuevo.Descripcion + "', " + nuevo.Precio +
                ", @IdMarca, @IdCategoria)" + "INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (SCOPE_IDENTITY(), @ImagenUrl);");
                datos.setearParametro("@IdMarca", nuevo.Marca.Id);
                datos.setearParametro("@IdCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@ImagenUrl", nuevo.Imagenes);
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

        public void modificar(Articulo art)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                   "UPDATE ARTICULOS SET " +
                   "Codigo = @Codigo, " +
                   "Nombre = @Nombre, " +
                   "Descripcion = @Descripcion, " +
                   "Precio = @Precio, " +
                   "IdMarca = @IdMarca, " +
                   "IdCategoria = @IdCategoria " +
                   "WHERE Id = @IdArticulo;"
                );

                datos.setearParametro("@Codigo", art.Codigo);
                datos.setearParametro("@Nombre", art.Nombre);
                datos.setearParametro("@Descripcion", art.Descripcion);
                datos.setearParametro("@Precio", art.Precio);
                datos.setearParametro("@IdMarca", art.Marca.Id);
                datos.setearParametro("@IdCategoria", art.Categoria.Id);
                datos.setearParametro("@ImagenUrl", art.Imagenes);
                datos.setearParametro("@IdArticulo", art.Id);

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

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM IMAGENES  WHERE IdArticulo = @id; " +
                                     "DELETE FROM ARTICULOS WHERE Id= @id;");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio, " +
                                  "ISNULL(M.Descripcion,'') AS Marca, " +
                                  "ISNULL(C.Descripcion,'') AS Categoria, " +
                                  "ISNULL(I.ImagenUrl,'')   AS ImagenUrl " +
                                  "FROM ARTICULOS A " +
                                  "LEFT JOIN MARCAS M ON A.IdMarca = M.Id " +
                                  "LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id " +
                                  "OUTER APPLY (SELECT TOP 1 ImagenUrl " +
                                  "FROM IMAGENES I WHERE I.IdArticulo = A.Id ORDER BY Id) I " +
                                  "WHERE 1=1 AND ";

                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "A.Precio < " + filtro;
                            break;
                        default:
                            consulta += "A.Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Nombre LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Nombre LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Nombre LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Descripcion")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Descripcion LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Descripcion LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo a = new Articulo();
                    a.Id = (int)datos.Lector["Id"];
                    a.Codigo = (string)datos.Lector["Codigo"];
                    a.Nombre = (string)datos.Lector["Nombre"];
                    a.Descripcion = (string)datos.Lector["Descripcion"];
                    a.Precio = (decimal)datos.Lector["Precio"];
                    a.Imagenes = (string)datos.Lector["ImagenUrl"];

                    a.Marca = new Marca();
                    a.Marca.Descripcion = (string)datos.Lector["Marca"];
                    a.Categoria = new Categoria();
                    a.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    lista.Add(a);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
