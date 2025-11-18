using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Inventario_Jardin
    {
        int id_inventario;
        string nombre;
        string tipo;
        decimal cantidad;
        string unidad;

        public int ID_inventario { get => id_inventario; set => id_inventario = value; }

        public string Nombre { get=> nombre; set => nombre = value; }
        public string Tipo { get=> tipo; set => tipo = value; }
        public decimal Cantidad { get => cantidad; set => cantidad = value; }
        public string Unidad { get => unidad; set => unidad = value; }
    }
}
