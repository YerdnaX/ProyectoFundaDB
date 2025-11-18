using Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Lógica de interacción para ListasyTareas.xaml
    /// </summary>
    public partial class Vehiculo : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Vehiculo> Vehicu { get; set; }
        public Vehiculo(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();
            Vehicu = dataService.LlenarComboVehiculo();
            CargarMantenimiento();
            DataContext = this;

        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Vehiculo - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }
        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnagregar.IsEnabled = Permisos.PuedeAgregar(rol);
            btnmodificar.IsEnabled = Permisos.PuedeEditar(rol);
            btneliminar.IsEnabled = Permisos.PuedeEliminar(rol);

            // Si es LECTOR, deshabilitar todos los controles de entrada
            if (rol == "LECTOR")
            {
                DeshabilitarControlesEntrada();
            }

            // Ocultar completamente botones de eliminación si no tiene permiso
            if (!Permisos.PuedeEliminar(rol))
            {
                btneliminar.Visibility = Visibility.Collapsed;

            }
        }
        private void DeshabilitarControlesEntrada()
        {
            // Deshabilitar todos los TextBox
            txtconcepto.IsEnabled = false;
            txtcosto.IsEnabled = false;
            txtcosto_Copiar.IsEnabled = false;
            txtkilometraje.IsEnabled = false;
            txtnotas.IsEnabled = false;

            fecha.IsEnabled = false;

            // Deshabilitar ComboBox
            boxtipo.IsEnabled = false;
            boxvehiculo.IsEnabled = false;
        }
        private void CargarMantenimiento()
        {
            try
            {
                bd.MostrarMantenimiento();

                if (bd.TablaMantenimiento != null && bd.TablaMantenimiento.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaMantenimiento.Rows)
                    {
                        Console.WriteLine($"Tipo: {row["tipo"]}, Concepto: {row["concepto"]}");
                    }

                    dbmante.ItemsSource = bd.TablaMantenimiento.DefaultView;

                    // Forzar actualización de la UI
                    dbmante.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de mantenimiento");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mantenimiento: " + ex.Message);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menú principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }
    }
}
