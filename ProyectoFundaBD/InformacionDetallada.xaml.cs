using Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoFundaBD
{
    /// <summary>
    /// Lógica de interacción para InformacionDetallada.xaml
    /// </summary>
    public partial class InformacionDetallada : Window
    {
        BaseDatos bd = new BaseDatos();
        public InformacionDetallada()
        {
            InitializeComponent();
            CargarDatos();
        }
        private void CargarDatos()
        {

            bd.MostrarSaludMascota();
            bd.MostrarMedicamentosActivos();
            bd.MostrarGastosVeterinaria();


            dgSaludMascota.ItemsSource = bd.TablaSalud?.DefaultView;
            dgMedicamentosActivos.ItemsSource = bd.TablaMedicamentos?.DefaultView;
            dgGastosVeterinaria.ItemsSource = bd.TablaVeterinaria?.DefaultView;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (bd != null)
            {
                bd.TablaSalud?.Clear();
                bd.TablaMedicamentos?.Clear();
                bd.TablaVeterinaria?.Clear();
            }
        }

    }
}
