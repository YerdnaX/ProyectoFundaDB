using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Facturas
    {
        int id_factura;
        int id_proveedor;
        decimal monto;
        int id_categoria;
        DateOnly fecha_emision;
        DateOnly fecha_vencimiento;
        string estado;


        public int Id_factura { get => id_factura; set => id_factura = value; }
        public int Id_proveedor { get => id_proveedor; set => id_proveedor = value; }
        public decimal Monto { get => monto; set => monto = value; }
        public int Id_categoria { get => id_categoria; set => id_categoria = value; }
        public DateOnly Fecha_emision { get => fecha_emision; set => fecha_emision = value; }
        public DateOnly Fecha_vencimiento { get => fecha_vencimiento; set => fecha_vencimiento = value; }
        public string Estado { get => estado; set => estado = value; }


    }
}
