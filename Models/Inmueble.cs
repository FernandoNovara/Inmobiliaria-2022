using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Models;

public class Inmueble
{
    [Display(Name = "Codigo")]
    [Key]
    public int IdInmueble {get;set;}
    [Display(Name = "Propietario")]
    public int IdPropietario {get;set;}
    
    [Display(Name = "Dueño")]
    [ForeignKey(nameof(IdPropietario))]
    public Propietario dueño {get;set;}
    public string Direccion {get;set;}
    public string Uso {get;set;}
    public string Tipo {get;set;}
    public int Ambientes {get;set;}
    public String Longitud {get;set;}
    public String Latitud {get;set;}
    public double Precio {get;set;}
    public Boolean Estado {get;set;}

    
}