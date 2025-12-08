using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class GastoMensual
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal Egreso { get; set; }
        public decimal? EgresoPrev { get; set; }
        public decimal? EgresoMoM { get; set; }
        public decimal? EgresoForecastNaive { get; set; }
    }
}
