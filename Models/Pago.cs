using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria_2022.Models;

public class Pago
{
    [Display(Name="Codigo de Pago")]
    [Key]
    public int IdPago {get;set;}

    [Display(Name = "Direccion de Contrato")]
    public int IdContrato {get;set;}
    
    [ForeignKey(nameof(IdContrato))]
    public Contrato contrato {get;set;}

    [Display(Name = "Fecha de Emision")]
    public DateTime FechaEmision {get;set;}

    public Double Importe {get;set;}

}