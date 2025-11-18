using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Mascotas_meds
    {
        int id_med;
        int id_mascota;
        string nombre_med;
        string doses;
        string frecuencia;
        DateOnly fecha_inicio;
        DateOnly fecha_fin;

        public int Id_Med { get => id_med; set => id_med = value; }
        public int Id_Mascota { get => id_mascota; set => id_mascota = value; }
        public string Nombre_Med { get => nombre_med; set => nombre_med = value; }
        public string Doses { get => doses; set => doses = value; }
        public string Frecuencia { get => frecuencia; set => frecuencia = value; }
        public DateOnly Fecha_Inicio { get => fecha_inicio; set => fecha_inicio = value; }
        public DateOnly Fecha_Fin { get => fecha_fin; set => fecha_fin = value; }

    }
}
