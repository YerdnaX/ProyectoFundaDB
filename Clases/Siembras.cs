using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Siembras
    {
        int id_siembra;
        int id_cultivo;
        DateOnly fechasiembra;
        DateOnly fechaestimada;
        string sector;
        string notas;

        public int ID_siembra { get => id_siembra; set => id_siembra = value; }
        public int ID_cultivo { get => id_cultivo; set => id_cultivo = value; }
        public DateOnly Fechasiembra { get => fechasiembra; set => fechasiembra = value; }
        public DateOnly Fechaestimada { get => fechaestimada; set => fechaestimada = value; }
        public string Sector { get => sector; set => sector = value; }
        public string Notas { get => notas; set => notas = value; }




    }
}
