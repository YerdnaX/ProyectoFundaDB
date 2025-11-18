using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Tareas
    {
        int ID_tareas;
        int ID_lista;
        string Titulo;
        string Descripcion;
        string prioridad;
        string estado;
        DateTime fecha_creacion;
        DateTime fecha_vencimiento;
        string repeticion;
        int ID_area;

        public int ID_Tareas { get => ID_tareas; set => ID_tareas = value; }
        public int ID_Lista { get => ID_lista; set => ID_lista = value; }
        public string Titulo1 { get => Titulo; set => Titulo = value; }
        public string Descripcion1 { get => Descripcion; set => Descripcion = value; }
        public string Prioridad { get => prioridad; set => prioridad = value; }
        public string Estado { get => estado; set => estado = value; }
        public DateTime Fecha_creacion { get => fecha_creacion; set => fecha_creacion = value; }
        public DateTime Fecha_vencimiento { get => fecha_vencimiento; set => fecha_vencimiento = value; }
        public string Repeticion { get => repeticion; set => repeticion = value; }
        public int ID_Area { get => ID_area; set => ID_area = value; }

    }
}
