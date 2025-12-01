using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Listas
    {
        int ID_listas;
        string nombre_lista;
        string tipo_lista;
        int id_area;
        int id_miembro;
        DateOnly fecha_creada;

        public int ID_Listas { get => ID_listas; set => ID_listas = value; }
        public string Nombre_Lista { get => nombre_lista; set => nombre_lista = value; }
        public string Tipo_Lista { get => tipo_lista; set => tipo_lista = value; }
        public int Id_Area { get => id_area; set => id_area = value; }
        public int Id_Miembro { get => id_miembro; set => id_miembro = value; }
        public DateOnly Fecha_Creada { get => fecha_creada; set => fecha_creada = value; }


    }
}
