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
    /// Lógica de interacción para Areas.xaml
    /// </summary>
    public partial class Cultivos : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;

        public ObservableCollection<Clases.Cultivos> Cultivo { get; set; }
        public ObservableCollection<Clases.Siembras> Siembras { get; set; }
        public Cultivos(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarSiembra();
            CargarTratamiento();
            CargarInventario();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();
            Cultivo = dataService.LlenarComboCultivos();
            Siembras = dataService.LlenarComboSimebras();

            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Cultivo - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }

        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnagregarinv.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregartrat.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregarsiem.IsEnabled = Permisos.PuedeAgregar(rol);

            btneliminarinv.IsEnabled = Permisos.PuedeEliminar(rol);
            btneliminartrat.IsEnabled = Permisos.PuedeEliminar(rol);
            btneliminarsiem.IsEnabled = Permisos.PuedeEliminar(rol);

            btnmodificarinv.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificartrat.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificarsiem.IsEnabled = Permisos.PuedeEditar(rol);

            // Si es LECTOR, deshabilitar todos los controles de entrada
            if (rol == "LECTOR")
            {
                DeshabilitarControlesEntrada();
            }

            // Ocultar completamente botones de eliminacion si no tiene permiso
            if (!Permisos.PuedeEliminar(rol))
            {
                btneliminarsiem.Visibility = Visibility.Collapsed;
                btneliminartrat.Visibility = Visibility.Collapsed;
                btneliminarinv.Visibility = Visibility.Collapsed;
            }
        }

        private void DeshabilitarControlesEntrada()
        {
            // Deshabilitar todos los TextBox
            txtsector.IsEnabled = false;
            txtnotas.IsEnabled = false;
            txtcantidad.IsEnabled = false;
            txtdosis.IsEnabled = false;
            txtnombre.IsEnabled = false;
            txtnotas1.IsEnabled = false;
            txtproducto.IsEnabled = false;
            txtsector.IsEnabled = false;
            txtunidad.IsEnabled = false;
            
            fecha.IsEnabled = false;
            fechaestimado.IsEnabled = false;
            fechasiembra.IsEnabled = false;


            // Deshabilitar ComboBox
            boxcultivo.IsEnabled = false;
            boxsiembra.IsEnabled = false;
            boxtipo.IsEnabled = false;
        }

        private void CargarSiembra()
        {
            try
            {
                bd.MostrarSiembras();

                if (bd.TablaSiembra != null && bd.TablaSiembra.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaSiembra.Rows)
                    {
                        Console.WriteLine($"Nombre: {row["nombre"]}");
                    }

                    dbsiembra.ItemsSource = bd.TablaSiembra.DefaultView;

                    // Forzar actualizacion de la UI
                    dbsiembra.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de siembra");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar siembra: " + ex.Message);
            }
        }

        private void CargarTratamiento()
        {
            try
            {
                bd.MostrarTratamiento();

                if (bd.TablaTratamiento != null && bd.TablaTratamiento.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaTratamiento.Rows)
                    {
                        Console.WriteLine($"Producto: {row["producto"]}");
                    }

                    dbtratamiento.ItemsSource = bd.TablaTratamiento.DefaultView;

                    // Forzar actualizacion de la UI
                    dbtratamiento.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de tratamiento");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar tratamiento: " + ex.Message);
            }
        }

        private void CargarInventario()
        {
            try
            {
                bd.MostrarInventario();

                if (bd.TablaInventarioJardin != null && bd.TablaInventarioJardin.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaInventarioJardin.Rows)
                    {
                        Console.WriteLine($"Nombre: {row["nombre"]}");
                    }

                    dbinventario.ItemsSource = bd.TablaInventarioJardin.DefaultView;

                    // Forzar actualizacion de la UI
                    dbinventario.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de inventario");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar inventario: " + ex.Message);
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
