using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Mascotas
    {
        int id_mascota;
        string nombre;
        string especie;
        string raza;
        DateOnly fecha_nac;
        decimal Peso;

        public int ID_mascota{ get => id_mascota; set => id_mascota = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Especie { get => especie; set => especie = value; }
        public string Raza { get => raza; set => raza = value; }
        public DateOnly Fecha_nac { get => fecha_nac; set => fecha_nac = value; }
        public decimal peso { get => Peso; set => Peso = value; }

    }
}
