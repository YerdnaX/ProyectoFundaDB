using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Eventos
    {
        int ID_Eventos;
        string tipo_evento;
        string titulo;
        DateTime fecha_evento;
        string lugar;
        string notas;
        int id_miembro;

        public int ID_eventos { get => ID_Eventos; set => ID_Eventos = value; }

        public string Tipo_evento { get => tipo_evento; set => tipo_evento = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public DateTime Fecha_evento { get => fecha_evento; set => fecha_evento = value; }
        public string Lugar { get => lugar; set => lugar = value; }
        public string Notas { get => notas; set => notas = value; }
        public int Id_miembro { get => id_miembro; set => id_miembro = value; }


    }
}
