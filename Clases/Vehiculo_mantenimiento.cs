using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Vehiculo_mantenimiento
    {
        int id_mantenimiento;
        int id_vehiculo;
        string tipo;
        string concepto;
        DateTime fecha_mantenimiento;
        int kilometraje;
        decimal monto;
        string taller;
        string notas;

        public int Id_Mantenimiento { get => id_mantenimiento; set => id_mantenimiento = value; }
        public int Id_Vehiculo { get => id_vehiculo; set => id_vehiculo = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string Concepto { get => concepto; set => concepto = value; }
        public DateTime Fecha_Mantenimiento { get => fecha_mantenimiento; set => fecha_mantenimiento = value; }
        public int Kilometraje { get => kilometraje; set => kilometraje = value; }
        public decimal Monto { get => monto; set => monto = value; }
        public string Taller { get => taller; set => taller = value; }
        public string Notas { get => notas; set => notas = value; }


    }
}
