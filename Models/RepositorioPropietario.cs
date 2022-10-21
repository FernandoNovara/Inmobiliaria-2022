namespace Inmobiliaria_2022.Models;
using MySql.Data.MySqlClient;

public class RepositorioPropietario
{
    String ConnectionStrings = "Server=localhost;User=root;Password=;Database=inmobiliaria_2022;SslMode=none";

    public RepositorioPropietario()
    {

    }

    public IList<Propietario> ObtenerPropietarios()
    {
        var res = new List<Propietario>();
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdPropietario,Nombre,Apellido,Dni,Email,Telefono,Clave from Propietario;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    var p = new Propietario
                    {
                        IdPropietario = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Dni = reader.GetString(3),
                        Email = reader.GetString(4),
                        Telefono = reader.GetString(5),
                        Clave = reader.GetString(6)
                    };
                    res.Add(p);
                }
                conn.Close();
            }
            
        }
        return res;
    }


    public int Alta(Propietario p)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @$"insert into propietario (Nombre,Apellido,Dni,Email,Telefono,Clave)
                         Value (@{nameof(p.Nombre)},@{nameof(p.Apellido)},@{nameof(p.Dni)},@{nameof(p.Email)},@{nameof(p.Telefono)},@{nameof(p.Clave)});
                         Select last_Insert_Id();";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue($"@{nameof(p.Nombre)}",p.Nombre);
                comm.Parameters.AddWithValue($"@{nameof(p.Apellido)}",p.Apellido);
                comm.Parameters.AddWithValue($"@{nameof(p.Dni)}",p.Dni);
                comm.Parameters.AddWithValue($"@{nameof(p.Email)}",p.Email);
                comm.Parameters.AddWithValue($"@{nameof(p.Telefono)}",p.Telefono);
                comm.Parameters.AddWithValue($"@{nameof(p.Clave)}",p.Clave);
                conn.Open();
                res = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
                p.IdPropietario = res;
            }
            return res;
        }
    }

    public Propietario ObtenerPropietario(int id)
    {
        Propietario res = null;
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdPropietario,Nombre,Apellido,Dni,Email,Telefono,Clave from Propietario where IdPropietario = @id;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@id",id);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    res = new Propietario
                    {
                        IdPropietario = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Dni = reader.GetString(3),
                        Email = reader.GetString(4),
                        Telefono = reader.GetString(5),
                        Clave = reader.GetString(6)
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
            String sql = @"Delete from Propietario where IdPropietario = @id;";
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

    public int Actualizar(Propietario p)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Update propietario Set Nombre=@Nombre,Apellido=@Apellido,Dni=@Dni,Email=@Email,Telefono=@Telefono,Clave=@Clave where IdPropietario = @id";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@Nombre",p.Nombre);
                comm.Parameters.AddWithValue("@Apellido",p.Apellido);
                comm.Parameters.AddWithValue("@Dni",p.Dni);
                comm.Parameters.AddWithValue("@Email",p.Email);
                comm.Parameters.AddWithValue("@Telefono",p.Telefono);
                comm.Parameters.AddWithValue("@Clave",p.Clave);
                comm.Parameters.AddWithValue("@id",p.IdPropietario);
                conn.Open();
                res = comm.ExecuteNonQuery();
                conn.Close();
            }
            
        }
        return res;
    }


}