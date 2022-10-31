namespace Inmobiliaria_2022.Models;
using MySql.Data.MySqlClient;

public class RepositorioInmueble
{
    String ConnectionStrings = "Server=localhost;User=root;Password=;Database=inmobiliaria_2022;SslMode=none";

    public RepositorioInmueble()
    {

    }

    public IList<Inmueble> ObtenerInmuebles()
    {
        var res = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdInmueble,Direccion,Uso,Tipo,Ambientes,Latitud,Longitud,Precio,Estado,propietario.IdPropietario,Nombre,Apellido
                            FROM inmueble
                            JOIN propietario on inmueble.IdPropietario = propietario.IdPropietario ORDER BY IdInmueble ASC;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    Inmueble i = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        Uso = reader.GetString(2),
                        Tipo = reader.GetString(3),
                        Ambientes = reader.GetInt32(4),
                        Latitud = reader.GetString(5),
                        Longitud = reader.GetString(6),
                        Precio = reader.GetDouble(7),
                        Estado = reader.GetBoolean(8),
                        dueño = new Propietario
                        {
                            IdPropietario = reader.GetInt32(9),
                            Nombre = reader.GetString(10),
                            Apellido = reader.GetString(11),

                        }
                        
                    };
                    res.Add(i);
                }
                conn.Close();
            }
            
        }
        return res;
    }


    public int Alta(Inmueble i)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @$"insert into Inmueble (IdInmueble,IdPropietario,Direccion,Uso,Tipo,Ambientes,Latitud,Longitud,Precio,Estado) 
                        Values (@{nameof(i.IdInmueble)},@{nameof(i.IdPropietario)},@{nameof(i.Direccion)},@{nameof(i.Uso)},@{nameof(i.Tipo)},@{nameof(i.Ambientes)},@{nameof(i.Latitud)},@{nameof(i.Longitud)},@{nameof(i.Precio)},@{nameof(i.Estado)});
                        Select last_Insert_Id();";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue($"@{nameof(i.IdInmueble)}",i.IdInmueble);
                comm.Parameters.AddWithValue($"@{nameof(i.IdPropietario)}",i.IdPropietario);
                comm.Parameters.AddWithValue($"@{nameof(i.Direccion)}",i.Direccion);
                comm.Parameters.AddWithValue($"@{nameof(i.Uso)}",i.Uso);
                comm.Parameters.AddWithValue($"@{nameof(i.Tipo)}",i.Tipo);
                comm.Parameters.AddWithValue($"@{nameof(i.Ambientes)}",i.Ambientes);
                comm.Parameters.AddWithValue($"@{nameof(i.Latitud)}",i.Latitud);
                comm.Parameters.AddWithValue($"@{nameof(i.Longitud)}",i.Longitud);
                comm.Parameters.AddWithValue($"@{nameof(i.Precio)}",i.Precio);
                comm.Parameters.AddWithValue($"@{nameof(i.Estado)}",i.Estado);
                conn.Open();
                res = Convert.ToInt32(comm.ExecuteScalar()); 
                conn.Close();
                i.IdInmueble = res;
            }
            return res;
        }
    }

    public Inmueble ObtenerInmueble(int id)
    {
        Inmueble res = null;
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdInmueble,Direccion,Uso,Tipo,Ambientes,Latitud,Longitud,Precio,Estado,propietario.IdPropietario,Nombre,Apellido 
                            FROM inmueble
                            JOIN propietario on inmueble.IdPropietario = propietario.IdPropietario 
                            where IdInmueble = @id ORDER BY IdInmueble ASC;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@id",id);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    res = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        Uso = reader.GetString(2),
                        Tipo = reader.GetString(3),
                        Ambientes = reader.GetInt32(4),
                        Latitud = reader.GetString(5),
                        Longitud = reader.GetString(6),
                        Precio = reader.GetDouble(7),
                        Estado = reader.GetBoolean(8),
                        dueño = new Propietario
                        {
                            IdPropietario = reader.GetInt32(9),
                            Nombre = reader.GetString(10),
                            Apellido = reader.GetString(11),

                        }
                    };
                }
                conn.Close();
            }
            
        }
        return res;
    }

    public int Baja(int id)
    {
        var res = -1;
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Delete from Inmueble where IdInmueble = @id;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@id",id);
                conn.Open();
                res = comm.ExecuteNonQuery();
                conn.Close();
            }
            
        }
        return res;
    }

    public int Actualizar(Inmueble i)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Update Inmueble Set IdPropietario=@IdPropietario,Direccion=@Direccion,Uso=@Uso,Tipo=@Tipo,Ambientes=@Ambientes,Latitud=@Latitud,Longitud=@Longitud,Precio=@Precio,Estado=@Estado where IdInmueble = @id";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@IdPropietario",i.IdPropietario);
                comm.Parameters.AddWithValue("@Direccion",i.Direccion);
                comm.Parameters.AddWithValue("@Uso",i.Uso);
                comm.Parameters.AddWithValue("@Tipo",i.Tipo);
                comm.Parameters.AddWithValue("@Ambientes",i.Ambientes);
                comm.Parameters.AddWithValue("@Latitud",i.Latitud);
                comm.Parameters.AddWithValue("@Longitud",i.Longitud);
                comm.Parameters.AddWithValue("@Precio",i.Precio);
                comm.Parameters.AddWithValue("@Estado",i.Estado);
                comm.Parameters.AddWithValue("@id",i.IdInmueble);
                conn.Open();
                res = comm.ExecuteNonQuery();
                conn.Close();
            }
            
        }
        return res;
    }

    public IList<Inmueble> ObtenerInmueblesDisponibles(int valor)
    {
        var res = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdInmueble,Direccion,Uso,Tipo,Ambientes,Latitud,Longitud,Precio,Estado,propietario.IdPropietario,Nombre,Apellido 
                            FROM inmueble 
                            JOIN propietario on inmueble.IdPropietario = propietario.IdPropietario 
                            WHERE inmueble.Estado = @valor
                            ORDER BY IdInmueble ASC;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@valor",valor);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    Inmueble i = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        Uso = reader.GetString(2),
                        Tipo = reader.GetString(3),
                        Ambientes = reader.GetInt32(4),
                        Latitud = reader.GetString(5),
                        Longitud = reader.GetString(6),
                        Precio = reader.GetDouble(7),
                        Estado = reader.GetBoolean(8),
                        dueño = new Propietario
                        {
                            IdPropietario = reader.GetInt32(9),
                            Nombre = reader.GetString(10),
                            Apellido = reader.GetString(11),

                        }
                        
                    };
                    res.Add(i);
                }
                conn.Close();
            }
            
        }
        return res;
    }


    public IList<Inmueble> ObtenerInmueblesPorPropietario(int id)
    {
        var res = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdInmueble,Direccion,Uso,Tipo,Ambientes,Latitud,Longitud,Precio,Estado,propietario.IdPropietario,Nombre,Apellido 
                            FROM inmueble
                            JOIN propietario on inmueble.IdPropietario = propietario.IdPropietario 
                            WHERE inmueble.IdPropietario = @id
                            ORDER BY IdInmueble ASC;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@id",id);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    Inmueble i = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        Uso = reader.GetString(2),
                        Tipo = reader.GetString(3),
                        Ambientes = reader.GetInt32(4),
                        Latitud = reader.GetString(5),
                        Longitud = reader.GetString(6),
                        Precio = reader.GetDouble(7),
                        Estado = reader.GetBoolean(8),
                        dueño = new Propietario
                        {
                            IdPropietario = reader.GetInt32(9),
                            Nombre = reader.GetString(10),
                            Apellido = reader.GetString(11),

                        }
                        
                    };
                    res.Add(i);
                }
                conn.Close();
            }
            
        }
        return res;
    }


}