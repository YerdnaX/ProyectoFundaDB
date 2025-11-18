using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Presupuesto
    {

        int id_presupuesto;
        int years;
        string mes;
        int id_cateogoria;
        decimal monto_planeado;
        decimal monto_gastado;

        public int Id_presupuesto { get => id_presupuesto; set => id_presupuesto = value; }
        public int Years { get => years; set => years = value; }
        public string Mes { get => mes; set => mes = value; }
        public int Id_cateogoria { get => id_cateogoria; set => id_cateogoria = value; }
        public decimal Monto_planeado { get => monto_planeado; set => monto_planeado = value; }
        public decimal Monto_gastado { get => monto_gastado; set => monto_gastado = value; }

    }
}
