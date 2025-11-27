using Clases;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Culti_Masco_Vehi.xaml
    /// </summary>
    public partial class Culti_Masco_Vehi : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public Culti_Masco_Vehi(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            MostrarInfoUsuario();
            CargarCultivos();
            CargarMascotas();
            CargarVehiculo();
        }
        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"- Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }

        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnagregarcult.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregarmasc.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregarveh.IsEnabled = Permisos.PuedeAgregar(rol);

            btneliminarcult.IsEnabled = Permisos.PuedeEliminar(rol);
            btneliminarmas.IsEnabled = Permisos.PuedeEliminar(rol);
            btneliminarveh.IsEnabled = Permisos.PuedeEliminar(rol);

            btnmodificarcult.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificarmas.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificarveh.IsEnabled = Permisos.PuedeEditar(rol);


            // Si es LECTOR, deshabilitar todos los controles de entrada
            if (rol == "LECTOR")
            {
                DeshabilitarControlesEntrada();
            }

            // Ocultar completamente botones de eliminacion si no tiene permiso
            if (!Permisos.PuedeEliminar(rol))
            {
                btneliminarcult.Visibility = Visibility.Collapsed;
                btneliminarveh.Visibility = Visibility.Collapsed;
                btneliminarmas.Visibility = Visibility.Collapsed;
            }
        }

        private void DeshabilitarControlesEntrada()
        {
            // Deshabilitar todos los TextBox
            txtnombrecultivo.IsEnabled = false;
            txtdetalle.IsEnabled = false;
            txtnombremascota.IsEnabled = false;
            txtpesp.IsEnabled = false;
            txtraza.IsEnabled = false;
            txtplaca.IsEnabled = false;
            txtmarca.IsEnabled = false;
            txtmodelo.IsEnabled = false;
            txtyear.IsEnabled = false;
            txtpoliza.IsEnabled = false;
            fechadekra.IsEnabled = false;
            fechamascota.IsEnabled = false;


            // Deshabilitar ComboBox
            boxespecie.IsEnabled = false;
        }
        private void CargarCultivos()
        {
            try
            {
                bd.MostrarCultivos();

                if (bd.TablaCultivos != null && bd.TablaCultivos.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaCultivos.Rows)
                    {
                        Console.WriteLine($"nombre: {row["nombre"]}, variedad: {row["variedad"]}");
                    }

                    dbcultivos.ItemsSource = bd.TablaCultivos.DefaultView;

                    // Forzar actualizacion de la UI
                    dbcultivos.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de cultivos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar cultivos: " + ex.Message);
            }
        }

        private void CargarMascotas()
        {
            try
            {
                bd.MostrarMascotas();

                if (bd.TablaMascotas != null && bd.TablaMascotas.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaMascotas.Rows)
                    {
                        Console.WriteLine($"nombre: {row["nombre"]}, especie: {row["especie"]}");
                    }

                    dbmascotas.ItemsSource = bd.TablaMascotas.DefaultView;

                    // Forzar actualizacion de la UI
                    dbmascotas.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de mascotas");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mascotas: " + ex.Message);
            }
        }
        private void CargarVehiculo()
        {
            try
            {
                bd.MostrarVehiculo();

                if (bd.TablaVehiculos != null && bd.TablaVehiculos.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaVehiculos.Rows)
                    {
                        Console.WriteLine($"placa: {row["placa"]}, marca: {row["marca"]}");
                    }

                    dbvehiculo.ItemsSource = bd.TablaVehiculos.DefaultView;

                    // Forzar actualizacion de la UI
                    dbvehiculo.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de vehiculo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar vehiculo: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

    }
}
