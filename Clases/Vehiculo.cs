using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Vehiculo
    {
        int id_vehiculo;
        string placa;
        string marca;
        string modelo;
        int year;
        string poliza;
        DateOnly dekra;

        public int ID_vehiculo { get => id_vehiculo; set => id_vehiculo = value; }
        public string Placa { get => placa; set => placa = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public int Year { get => year; set => year = value;}
        public string Poliza { get => poliza; set => poliza = value; }
        public DateOnly Dekra { get => dekra; set => dekra = value; }


    }
}
