using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace Datos
{
    public class ProductoDB
    {
        string cadena = "server=localhost; user=root; database=factura2; password=123456";

        public Boolean Insertar(Producto producto)
        {
            Boolean inserto = false;

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" INSERT INTO producto VALUES ");
                sql.Append(" (@Codigo, @Descripcion, @Existencia, @Precio, @Foto, @EstaActivo);");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;

                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = producto.Codigo;
                        comando.Parameters.Add("@Descripcion", MySqlDbType.VarChar, 200).Value = producto.Descripcion;
                        comando.Parameters.Add("@Existencia", MySqlDbType.Int32).Value = producto.Existencia;
                        comando.Parameters.Add("@Precio", MySqlDbType.Decimal).Value = producto.Precio;
                        comando.Parameters.Add("@Foto", MySqlDbType.LongBlob).Value = producto.Imagen;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = producto.EstaActivo;
                        comando.ExecuteNonQuery();
                        inserto = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return inserto;
        }

        public Boolean Editar(Producto producto)
        {
            Boolean modifico = false;

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" UPDATE producto SET ");
                sql.Append(" Descripcion = @Descripcion, Existencia = @Existencia, Precio = @Precio, Foto = @Foto, EstaActivo = @EstaActivo ");
                sql.Append(" WHERE Codigo = @Codigo");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("Codigo", MySqlDbType.VarChar, 80).Value = producto.Codigo;
                        comando.Parameters.Add("Descripcion", MySqlDbType.VarChar, 200).Value = producto.Descripcion;
                        comando.Parameters.Add("Existencia", MySqlDbType.Int32).Value = producto.Existencia;
                        comando.Parameters.Add("Precio", MySqlDbType.Decimal).Value = producto.Precio;
                        comando.Parameters.Add("Foto", MySqlDbType.LongBlob).Value = producto.Imagen;
                        comando.Parameters.Add("EstaActivo", MySqlDbType.Bit).Value = producto.EstaActivo;
                        comando.ExecuteNonQuery();
                        modifico = true;
                    }
                }

            }
            catch (System.Exception ex)
            {
            }
            return modifico;
        }

        public DataTable DevolverProductos()
        {
            DataTable dt = new DataTable();

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT * FROM producto ");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        MySqlDataReader dr = comando.ExecuteReader();
                        dt.Load(dr);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }

        public byte[] DevolverImagen(string codigoProducto)
        {
            byte[] imagen = new byte[0];

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT * FROM producto WHERE Codigo = @Codigo;");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;

                        comando.Parameters.Add("Codigo", MySqlDbType.VarChar, 80).Value = codigoProducto;
                        MySqlDataReader dr = comando.ExecuteReader();
                        if (dr.Read())
                        {
                            imagen = (byte[])dr["Foto"];
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return imagen;
        }

        public Boolean Eliminar(string codigoProducto)
        {
            Boolean elimino = false;

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" DELETE FROM producto WHERE Codigo = @Codigo; ");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = codigoProducto;
                        comando.ExecuteNonQuery();
                        elimino = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return elimino;
        }

        public Producto DevolverProductoPorCodigo(string codigo)
        {
            Producto producto = null;

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT * FROM producto WHERE Codigo = @Codigo;");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {

                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = codigo;
                        MySqlDataReader dr = comando.ExecuteReader();
                        if (dr.Read())
                        {
                            producto = new Producto();
                            producto.Codigo = dr["Codigo"].ToString();
                            //producto.Codigo = codigo;
                            producto.Descripcion = dr["Descripcion"].ToString();
                            producto.Existencia = Convert.ToInt32(dr["Existencia"]);
                            producto.Precio = Convert.ToDecimal(dr["Precio"]);
                            if (dr["Foto"].GetType() != typeof(DBNull))
                            {
                                producto.Imagen = (byte[])dr["Foto"];
                            }
                            producto.EstaActivo = Convert.ToBoolean(dr["EstaActivo"]);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return producto;
        }

        public DataTable DevolverProductosPorDescripcion(string descripcion)
        {
            DataTable dt = new DataTable();

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT * FROM producto WHERE Descripcion LIKE '%" + descripcion + "%';");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        MySqlDataReader dr = comando.ExecuteReader();
                        dt.Load(dr);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }
    }
}
