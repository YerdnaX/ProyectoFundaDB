using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Miembros
    {
        int ID_miembros;
        string nombre;
        string apellido;
        string email;
        string rol;
        DateTime fecha;

        public int ID_Miembros { get => ID_miembros; set => ID_miembros = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Email { get => email; set => email = value; }
        public string Rol { get => rol; set => rol = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }

      
    }
}
