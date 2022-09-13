namespace Net.Models;

public class Pago
{
    public int IdPago {get;set;}

    public int IdContrato {get;set;}

    public Contrato contrato {get;set;}

    public DateTime FechaEmision {get;set;}

    public Double Importe {get;set;}

}