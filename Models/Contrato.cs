namespace Net.Models;

public class Contrato
{
    public Int32 IdContrato {get;set;}

    public Int32 IdInmueble {get;set;}

    public Int32 IdInquilino {get;set;}

    public DateTime FechaInicio {get;set;}

    public DateTime FechaFinal {get;set;}

    public double Monto {get;set;}
}