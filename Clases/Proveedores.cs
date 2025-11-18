using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Proveedores
    {
        int ID_proveedor;
        string nombre_empresa;
        string tipo;

        public int ID_Proveedor { get => ID_proveedor; set => ID_proveedor = value; }
        public string Nombre_Empresa { get => nombre_empresa; set => nombre_empresa = value; }
        public string Tipo { get => tipo; set => tipo = value; }
    }
}
