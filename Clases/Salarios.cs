using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Salarios
    {
        int id_salarios;
        int id_miembro;
        decimal monto;
        string periodicidad;
        decimal deducciones;
        DateOnly fecha_inicio;

        public int Id_salarios { get => id_salarios; set => id_salarios = value; }
        public int Id_miembro { get => id_miembro; set => id_miembro = value; }
        public decimal Monto { get => monto; set => monto = value; }
        public string Periodicidad { get => periodicidad; set => periodicidad = value; }
        public decimal Deducciones { get => deducciones; set => deducciones = value; }
        public DateOnly Fecha_inicio { get => fecha_inicio; set => fecha_inicio = value; }

    }
}
