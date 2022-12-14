using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_2022.Models;

public interface IRepositorioPropietario : IRepositorio<Propietario>
{
    Propietario ObtenerPorEmail(string email);
    IList<Propietario> BuscarPorNombre(string nombre);
}
