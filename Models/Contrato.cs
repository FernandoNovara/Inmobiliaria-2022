using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria_2022.Models;

public class Contrato
{

    [Display(Name = "Codigo")]
    [Key]
    public Int32 IdContrato {get;set;}

    [Display(Name = "Direccion Inmueble")]
    public Int32 IdInmueble {get;set;}

    [ForeignKey(nameof(IdInmueble))]
    public Inmueble inmueble {get;set;}

    [Display(Name = "Inquilino")]
    public Int32 IdInquilino {get;set;}

    [ForeignKey(nameof(IdInquilino))]
    public Inquilino inquilino {get;set;}

    [Display(Name = "Fecha Inicial")]
    public DateTime FechaInicio {get;set;}

    [Display(Name = "Fecha de Finalizacion")]
    public DateTime FechaFinal {get;set;}

    public double Monto {get;set;}
}