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
    /// Lógica de interacción para Areas.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        private Miembros miembroActual;

        public MenuPrincipal(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            MostrarInfoUsuario();

        }
        private void MostrarInfoUsuario()
        {

            txtUsuarioInfo.Text = $"Usuario: {miembroActual.Nombre} - Rol: {miembroActual.Rol}";

            // Cambiar color 
            switch (miembroActual.Rol.ToUpper())
            {
                case "ADMIN":
                    txtUsuarioInfo.Foreground = Brushes.Black;
                    break;
                case "EDITOR":
                    txtUsuarioInfo.Foreground = Brushes.Black;
                    break;
                case "LECTOR":
                    txtUsuarioInfo.Foreground = Brushes.Black;
                    break;
            }
        }
        private void btngeneral_Click(object sender, RoutedEventArgs e)
        {

            Areas nuevaVentana = new Areas(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btnfinanzas_Click(object sender, RoutedEventArgs e)
        {
            ListasyTareas nuevaVentana = new ListasyTareas(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btnasignacion_Click(object sender, RoutedEventArgs e)
        {
            AsignacionTareas nuevaVentana = new AsignacionTareas(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btneventos_Click(object sender, RoutedEventArgs e)
        {
            Eventros nuevaVentana = new Eventros(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btnfacturas_Click(object sender, RoutedEventArgs e)
        {
            Finanzas nuevaVentana = new Finanzas(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btnpresupuesto_Click(object sender, RoutedEventArgs e)
        {
            Presupuesto nuevaVentana = new Presupuesto(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btnsalario_Click(object sender, RoutedEventArgs e)
        {
            Salarios nuevaVentana = new Salarios(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btnmovimientos_Click(object sender, RoutedEventArgs e)
        {
            Movimientos nuevaVentana = new Movimientos(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btncultimasvehi_Click(object sender, RoutedEventArgs e)
        {
            Culti_Masco_Vehi nuevaVentana = new Culti_Masco_Vehi(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btncultivo_Click(object sender, RoutedEventArgs e)
        {
            Cultivos nuevaVentana = new Cultivos(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btnmascotas_Click(object sender, RoutedEventArgs e)
        {
            Mascotas nuevaVentana = new Mascotas(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }

        private void btnvehiculo_Click(object sender, RoutedEventArgs e)
        {
            Vehiculo nuevaVentana = new Vehiculo(miembroActual);

            nuevaVentana.Show();

            this.Hide();
        }
        private void btngastomensual_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new PrediccionGastosMensuales();
            ventana.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            PantallaPrincipal menuPrincipal = new PantallaPrincipal();
            menuPrincipal.Show();


        }
    }
}
