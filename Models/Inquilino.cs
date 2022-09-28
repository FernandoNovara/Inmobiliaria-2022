using System.ComponentModel.DataAnnotations;

namespace Net.Models;

public class Inquilino
{
    [Display(Name = "Cod. inquilino")]
    public int IdInquilino {get;set;}

    public string Nombre {get;set;}

    public string Dni {get;set;}

    [Display(Name = "Lugar de Trabajo")]
    public string LugarTrabajo {get;set;}

    public string Direccion {get;set;}

    public string Email {get;set;}

    public string Telefono {get;set;}
}