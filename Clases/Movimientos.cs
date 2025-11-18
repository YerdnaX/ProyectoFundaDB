using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Movimientos
    {
        int idMovimiento;
        DateOnly fecha;
        string tipo;
        int id_cateoria;
        decimal monto;
        string referencia;


        public int IDmovimiento { get => idMovimiento; set => idMovimiento = value; }
        public DateOnly Fecha { get => fecha; set => fecha = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public int Id_cateoria { get => id_cateoria; set => id_cateoria = value; }
        public decimal Monto { get => monto; set => monto = value; }
        public string Referencia { get => referencia; set => referencia = value; }
    }
}
