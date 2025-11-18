using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Mascotas_Salud
    {
        int id_registro_salud;
        int id_mascota;
        DateOnly fecha_revision;
        string evento;
        string notas;

        public int Id_Registro_Salud { get => id_registro_salud; set => id_registro_salud = value; }
        public int Id_Mascota { get => id_mascota; set => id_mascota = value; }
        public DateOnly Fecha_Revision { get => fecha_revision; set => fecha_revision = value; }
        public string Evento { get => evento; set => evento = value; }
        public string Notas { get => notas; set => notas = value; }

    }
}
