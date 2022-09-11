namespace Net.Models;

using MySql.Data.MySqlClient;

public class RepositorioContrato
{
    String ConnectionStrings = "Server=localhost;User=root;Password=;Database=inmobiliaria_2022;SslMode=none";

    public RepositorioContrato()
    {

    }

    public IList<Contrato> ObtenerContratos()
    {
        var res = new List<Contrato>();
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdContrato,inmueble.IdInmueble,inmueble.Direccion,inquilino.IdInquilino,inquilino.Nombre,FechaInicio,FechaFinal,Monto 
                            FROM Contrato
                            JOIN inmueble ON contrato.IdInmueble = inmueble.IdInmueble
                            JOIN inquilino ON contrato.IdInquilino = inquilino.IdInquilino
                            ORDER BY IdInmueble ASC;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    Contrato i = new Contrato
                    {
                        IdContrato = reader.GetInt32(0),
                        inmueble = new Inmueble
                            {
                              IdInmueble = reader.GetInt32(1),  
                              Direccion = reader.GetString(2),
                            },
                        inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(3),
                                Nombre = reader.GetString(4),
                            },
                        FechaInicio = reader.GetDateTime(5),
                        FechaFinal = reader.GetDateTime(6),
                        Monto = reader.GetDouble(7),
                    };
                    res.Add(i);
                }
                conn.Close();
            }
            
        }
        return res;
    }


    public int Alta(Contrato i)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @$"insert into Contrato (IdContrato,IdInmueble,IdInquilino,FechaInicio,FechaFinal,Monto) 
                        Values (@{nameof(i.IdContrato)},@{nameof(i.IdInmueble)},@{nameof(i.IdInquilino)},@{nameof(i.FechaInicio)},@{nameof(i.FechaFinal)},@{nameof(i.Monto)});
                        Select last_Insert_Id();";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue($"@{nameof(i.IdContrato)}",i.IdContrato);
                comm.Parameters.AddWithValue($"@{nameof(i.IdInmueble)}",i.IdInmueble);
                comm.Parameters.AddWithValue($"@{nameof(i.IdInquilino)}",i.IdInquilino);
                comm.Parameters.AddWithValue($"@{nameof(i.FechaInicio)}",i.FechaInicio);
                comm.Parameters.AddWithValue($"@{nameof(i.FechaFinal)}",i.FechaFinal);
                comm.Parameters.AddWithValue($"@{nameof(i.Monto)}",i.Monto);
                conn.Open();
                res = Convert.ToInt32(comm.ExecuteScalar()); 
                conn.Close();
                i.IdContrato = res;
            }
            return res;
        }
    }

    public Contrato ObtenerContrato(int id)
    {
        Contrato res = null;
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Select IdContrato,inmueble.IdInmueble,inmueble.Direccion,inquilino.IdInquilino,inquilino.Nombre,FechaInicio,FechaFinal,Monto 
                            FROM Contrato
                            JOIN inmueble ON contrato.IdInmueble = inmueble.IdInmueble
                            JOIN inquilino ON contrato.IdInquilino = inquilino.IdInquilino
                            where IdContrato = @id;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@id",id);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    res = new Contrato
                    {
                        IdContrato = reader.GetInt32(0),
                        inmueble = new Inmueble
                            {
                              IdInmueble = reader.GetInt32(1),  
                              Direccion = reader.GetString(2),
                            },
                        inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(3),
                                Nombre = reader.GetString(4),
                            },
                        FechaInicio = reader.GetDateTime(5),
                        FechaFinal = reader.GetDateTime(6),
                        Monto = reader.GetDouble(7),
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
            String sql = @"Delete from Contrato where IdContrato = @id;";
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

    public int Actualizar(Contrato i)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Update Contrato Set IdInmueble=@IdInmueble,IdInquilino=@IdInquilino,FechaInicio=@FechaInicio,FechaFinal=@FechaFinal,Monto=@Monto where IdContrato = @id";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@IdInmueble",i.IdInmueble);
                comm.Parameters.AddWithValue("@IdInquilino",i.IdInquilino);
                comm.Parameters.AddWithValue("@FechaInicio",i.FechaInicio);
                comm.Parameters.AddWithValue("@FechaFinal",i.FechaFinal);
                comm.Parameters.AddWithValue("@Monto",i.Monto);
                comm.Parameters.AddWithValue("@id",i.IdContrato);
                conn.Open();
                res = comm.ExecuteNonQuery();
                conn.Close();
            }
            
        }
        return res;
    }


}