namespace Net.Models;
using MySql.Data.MySqlClient;

public class RepositorioUsuario
{
    String ConnectionStrings = "Server=localhost;User=root;Password=;Database=inmobiliaria_2022;SslMode=none";

    public RepositorioUsuario()
    {

    }

    public IList<Usuario> ObtenerUsuarios()
    {
        var res = new List<Usuario>();
        using(MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            string sql = @"Select IdUsuario,Nombre,Apellido,Email,Rol,Avatar From Usuario;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    var p = new Usuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Email = reader.GetString(3),
                        Rol = reader.GetInt32(4),
                        Avatar = reader["Avatar"].ToString(),
                    };
                    res.Add(p);
                }
                conn.Close();
            }
        }
        return res;
    }


    public int Alta(Usuario u)
    {
        var res = -1;
        using(MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @$"insert into Usuario (Nombre,Apellido,Email,Clave,Rol,Avatar) 
                        Values (@{nameof(u.Nombre)},@{nameof(u.Apellido)},@{nameof(u.Email)},@{nameof(u.Clave)},@{nameof(u.Rol)},@{nameof(u.Avatar)});
                        Select last_Insert_Id();";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue($"@{nameof(u.Nombre)}",u.Nombre);
                comm.Parameters.AddWithValue($"@{nameof(u.Apellido)}",u.Apellido);
                comm.Parameters.AddWithValue($"@{nameof(u.Email)}",u.Email);
                comm.Parameters.AddWithValue($"@{nameof(u.Clave)}",u.Clave);
                comm.Parameters.AddWithValue($"@{nameof(u.Rol)}",u.Rol);
                if(String.IsNullOrEmpty(u.Avatar))
                {
                    comm.Parameters.AddWithValue($"@{nameof(u.Avatar)}",DBNull.Value);
                }
                else
                {
                    comm.Parameters.AddWithValue($"@{nameof(u.Avatar)}",u.Avatar);
                }
                conn.Open();
                res=Convert.ToInt32(comm.ExecuteScalar());
                u.IdUsuario=res;
                conn.Close();
                
            }
        }
        return res;
    }

    public Usuario ObtenerUsuario(int id)
    {
        Usuario res = null;
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdUsuario,Nombre,Apellido,Email,Clave,Rol,Avatar From Usuario where usuario.IdUsuario = @id;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@id",id);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    res = new Usuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Email = reader.GetString(3),
                        Clave = reader.GetString(4),
                        Rol = reader.GetInt32(5),
                        Avatar = reader["Avatar"].ToString(),
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
            String sql = @"Delete from Usuario where usuario.IdUsuario = @id;";
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

    public int Actualizar(Usuario u)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Update Usuario Set Nombre=@Nombre,Apellido=@Apellido,Email=@Email,Clave=@Clave,Rol=@Rol,Avatar=@Avatar where usuario.IdUsuario = @id;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@Nombre",u.Nombre);
                comm.Parameters.AddWithValue("@Apellido",u.Apellido);
                comm.Parameters.AddWithValue("@Email",u.Email);
                comm.Parameters.AddWithValue("@Clave",u.Clave);
                comm.Parameters.AddWithValue("@Rol",u.Rol);
                comm.Parameters.AddWithValue("@Avatar",u.Avatar);
                comm.Parameters.AddWithValue("@id",u.IdUsuario);
                conn.Open();
                res = comm.ExecuteNonQuery();
                conn.Close();
            }
            
        }
        return res;
    }


        public Usuario ObtenerUsuarioPorEmail(String email)
    {
        Usuario res = null;
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdUsuario,Nombre,Apellido,Email,Clave,Rol,Avatar From Usuario where usuario.Email = @email;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@email",email);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    res = new Usuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Email = reader.GetString(3),
                        Clave = reader.GetString(4),
                        Rol = reader.GetInt32(5),
                        Avatar = reader["Avatar"].ToString(),
                    };
                }
                conn.Close();
            }
            
        }
        return res;
    }


}