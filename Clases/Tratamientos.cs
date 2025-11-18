using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Tratamientos
    {
        int id_tratamiento;
        int id_siembra;
        DateOnly fecha;
        string producto;
        string dosis;
        string notas;

        public int ID_tratamiento {get => id_tratamiento; set=> id_tratamiento = value; }

        public int ID_siembra { get => id_siembra; set => id_siembra = value; }

        public DateOnly Fecha { get => fecha; set => fecha = value;}

        public string Producto { get => producto; set => producto = value; }
        public string Dosis { get => dosis; set => dosis = value; }
        public string Notas { get => notas; set => notas = value; }

    }
}
