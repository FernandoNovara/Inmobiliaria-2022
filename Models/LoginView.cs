using System.ComponentModel.DataAnnotations;

namespace Net.Models;

public class LoginView
{

    [DataType(DataType.EmailAddress)]
    public String Usuario {get;set;}

    [DataType(DataType.Password)]
    public String Clave {get;set;}

}