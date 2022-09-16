using System.ComponentModel.DataAnnotations;

namespace Net.Models;

public class Pago
{
    [Display(Name="Codigo de Pago")]
    public int IdPago {get;set;}

    [Display(Name = "Direccion de Contrato")]
    public int IdContrato {get;set;}

    public Contrato contrato {get;set;}

    [Display(Name = "Fecha de Emision")]
    public DateTime FechaEmision {get;set;}

    public Double Importe {get;set;}

}