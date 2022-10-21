using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_2022.Models;

public class Propietario
{
    [Display(Name = "Cod. Propietario")]
    [Key]
    public int IdPropietario {get;set;}

    public string Nombre {get;set;}

    public string Apellido {get;set;}

    public string Dni {get;set;}

    public string Telefono {get;set;}

    public string Email {get;set;}

    public string Clave {get;set;}
}