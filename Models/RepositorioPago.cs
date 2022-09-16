namespace Net.Models;
using MySql.Data.MySqlClient;

public class RepositorioPago
{
    String ConnectionStrings = "Server=localhost;User=root;Password=;Database=inmobiliaria_2022;SslMode=none";

    public RepositorioPago()
    {

    }

    public IList<Pago> ObtenerPagos()
    {
        var res = new List<Pago>();
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"SELECT IdPago,contrato.IdContrato,contrato.IdInmueble,contrato.IdInquilino,pago.FechaEmision,pago.Importe
                            FROM pago
                            JOIN contrato On pago.IdContrato = contrato.IdContrato;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    var p = new Pago
                    {
                        IdPago = reader.GetInt32(0),
                        contrato = new Contrato
                        {
                            IdContrato = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            IdInquilino = reader.GetInt32(3),
                        },
                        FechaEmision = reader.GetDateTime(4),
                        Importe = reader.GetDouble(5)
                    };
                    res.Add(p);
                }
                conn.Close();
            }
            
        }
        return res;
    }


    public int Alta(Pago p)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @$"insert into Pago (IdPago,IdContrato,FechaEmision,Importe)
                         Value (@{nameof(p.IdPago)},@{nameof(p.IdContrato)},@{nameof(p.FechaEmision)},@{nameof(p.Importe)});
                         Select last_Insert_Id();";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue($"@{nameof(p.IdPago)}",p.IdPago);
                comm.Parameters.AddWithValue($"@{nameof(p.IdContrato)}",p.IdContrato);
                comm.Parameters.AddWithValue($"@{nameof(p.FechaEmision)}",p.FechaEmision);
                comm.Parameters.AddWithValue($"@{nameof(p.Importe)}",p.Importe);
                conn.Open();
                res = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
                p.IdPago = res;
            }
            return res;
        }
    }

    public Pago ObtenerPago(int id)
    {
        Pago res = null;
        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"SELECT IdPago,contrato.IdContrato,contrato.IdInmueble,contrato.IdInquilino,pago.FechaEmision,pago.Importe
                            FROM pago
                            JOIN contrato On pago.IdContrato = contrato.IdContrato 
                            WHERE IdPago = @id;";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@id",id);
                conn.Open();
                var reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    res = new Pago
                    {
                        IdPago = reader.GetInt32(0),
                        contrato = new Contrato
                        {
                            IdContrato = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            IdInquilino = reader.GetInt32(3),
                        },
                        FechaEmision = reader.GetDateTime(4),
                        Importe = reader.GetDouble(5)
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
            String sql = @"Delete from Pago where IdPago = @id;";
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

    public int Actualizar(Pago p)
    {
        int res = -1;

        using (MySqlConnection conn = new MySqlConnection(ConnectionStrings))
        {
            String sql = @"Update Pago Set IdPago=@IdPago,IdContrato=@IdContrato,FechaEmision=@FechaEmision,Importe=@Importe where IdPago = @id";
            using(MySqlCommand comm = new MySqlCommand(sql,conn))
            {
                comm.Parameters.AddWithValue("@IdPago",p.IdPago);
                comm.Parameters.AddWithValue("@IdContrato",p.IdContrato);
                comm.Parameters.AddWithValue("@FechaEmision",p.FechaEmision);
                comm.Parameters.AddWithValue("@Importe",p.Importe);
                comm.Parameters.AddWithValue("@id",p.IdPago);
                conn.Open();
                res = comm.ExecuteNonQuery();
                conn.Close();
            }
            
        }
        return res;
    }


}