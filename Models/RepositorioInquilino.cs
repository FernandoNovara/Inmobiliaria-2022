namespace Net.Models;
using MySql.Data.MySqlClient;

public class RepositorioInquilino
{
    String ConnectionStrings = "Server=localhost;User=root;Password=;Database=inmobiliaria_2022;SslMode=none";

    public RepositorioInquilino()
    {

    }

    public IList<Inquilino> ObtenerInquilinos()
    {
        var res = new List<Inquilino>();
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdInquilino,Nombre,Dni,LugarTrabajo,Direccion,Email,Telefono from Inquilino;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    var i = new Inquilino
                    {
                        IdInquilino = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Dni = reader.GetString(2),
                        LugarTrabajo = reader.GetString(3),
                        Direccion = reader.GetString(4),
                        Email = reader.GetString(5),
                        Telefono = reader.GetString(6)
                    };
                    res.Add(i);
                }
                conn.Close();
            }
            
        }
        return res;
    }


    public int Alta(Inquilino i)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @$"insert into Inquilino (Nombre,Dni,LugarTrabajo,Direccion,Email,Telefono) 
                        Values (@{nameof(i.Nombre)},@{nameof(i.Dni)},@{nameof(i.LugarTrabajo)},@{nameof(i.Direccion)},@{nameof(i.Email)},@{nameof(i.Telefono)});
                        Select last_Insert_Id();";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue($"@{nameof(i.Nombre)}",i.Nombre);
                comm.Parameters.AddWithValue($"@{nameof(i.Dni)}",i.Dni);
                comm.Parameters.AddWithValue($"@{nameof(i.LugarTrabajo)}",i.LugarTrabajo);
                comm.Parameters.AddWithValue($"@{nameof(i.Direccion)}",i.Direccion);
                comm.Parameters.AddWithValue($"@{nameof(i.Email)}",i.Email);
                comm.Parameters.AddWithValue($"@{nameof(i.Telefono)}",i.Telefono);
                conn.Open();
                res = Convert.ToInt32(comm.ExecuteScalar()); 
                conn.Close();
                i.IdInquilino = res;
            }
            return res;
        }
    }

    public Inquilino ObtenerInquilino(int id)
    {
        Inquilino res = null;
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdInquilino,Nombre,Dni,LugarTrabajo,Direccion,Email,Telefono from Inquilino where IdInquilino = @id;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@id",id);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    res = new Inquilino
                    {
                        IdInquilino = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Dni = reader.GetString(2),
                        LugarTrabajo = reader.GetString(3),
                        Direccion = reader.GetString(4),
                        Email = reader.GetString(5),
                        Telefono = reader.GetString(6)
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
            String sql = @"Delete from Inquilino where IdInquilino = @id;";
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

    public int Actualizar(Inquilino i)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Update Inquilino Set Nombre=@Nombre,Dni=@Dni,LugarTrabajo=@LugarTrabajo,Direccion=@Direccion,Email=@Email,Telefono=@Telefono where IdInquilino = @id";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@Nombre",i.Nombre);
                comm.Parameters.AddWithValue("@Dni",i.Dni);
                comm.Parameters.AddWithValue("@LugarTrabajo",i.LugarTrabajo);
                comm.Parameters.AddWithValue("@Direccion",i.Direccion);
                comm.Parameters.AddWithValue("@Email",i.Email);
                comm.Parameters.AddWithValue("@Telefono",i.Telefono);
                comm.Parameters.AddWithValue("@id",i.IdInquilino);
                conn.Open();
                res = comm.ExecuteNonQuery();
                conn.Close();
            }
            
        }
        return res;
    }


}