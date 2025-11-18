using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Vet_Visitas
    {
        int id_visitas;
        int id_mascota;
        DateOnly fecha;
        string motivo;
        decimal costo;
        string notas;

        public int Id_Visitas { get => id_visitas; set => id_visitas = value; }
        public int Id_Mascota { get => id_mascota; set => id_mascota = value; }
        public DateOnly Fecha { get => fecha; set => fecha = value; }
        public string Motivo { get => motivo; set => motivo = value; }
        public decimal Costo { get => costo; set => costo = value; }
        public string Notas { get => notas; set => notas = value; }
    }
}
