using Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProyectoFundaBD
{
    public partial class PrediccionGastosMensuales : Window
    {
        BaseDatos bd = new BaseDatos();
        public PrediccionGastosMensuales()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            List<GastoMensual> datos = bd.ObtenerGastoMensual();
            dgGastoMensual.ItemsSource = datos;

            // Alertas sencillas: MoM > 15% o egreso supera forecast MA3
            var alertas = datos
                .Where(x =>
                    (x.EgresoMoM.HasValue && x.EgresoMoM.Value > 0.15m) ||
                    (x.EgresoForecastNaive.HasValue && x.Egreso > x.EgresoForecastNaive.Value))
                .Select(x =>
                    $"Anno {x.Anio}, Mes {x.Mes}: Egreso {x.Egreso:N0}, MoM {(x.EgresoMoM ?? 0):P1}, MA3 {x.EgresoForecastNaive ?? 0:N0}")
                .ToList();

            lstAlertas.ItemsSource = alertas;
        }
        
    }
}
