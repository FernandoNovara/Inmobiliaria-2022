using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_2022.Models;

public class Inquilino
{
    [Display(Name = "Cod. inquilino")]
    [Key]
    public int IdInquilino {get;set;}

    public string Nombre {get;set;}

    public string Dni {get;set;}

    [Display(Name = "Lugar de Trabajo")]
    public string LugarTrabajo {get;set;}

    public string Direccion {get;set;}

    public string Email {get;set;}

    public string Telefono {get;set;}
}