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
    /// Lógica de interacción para Areas.xaml
    /// </summary>
    public partial class Areas : Window
    {

        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public Areas(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarAreas();
            Cargarfinanzas();
            Cargarproveedores();
            MostrarInfoUsuario();
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Areas - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }

        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnareas.IsEnabled = Permisos.PuedeAgregar(rol);
            btnmodificar.IsEnabled = Permisos.PuedeEditar(rol);
            btneliminar.IsEnabled = Permisos.PuedeEliminar(rol);

            btnfinanzas.IsEnabled = Permisos.PuedeAgregar(rol);
            btnmodificarfi.IsEnabled = Permisos.PuedeEditar(rol);
            btneliminarfi.IsEnabled = Permisos.PuedeEliminar(rol);

            btnproveedor.IsEnabled = Permisos.PuedeAgregar(rol);
            btnmodificarPro.IsEnabled = Permisos.PuedeEditar(rol);
            btneliminarpro.IsEnabled = Permisos.PuedeEliminar(rol);

            // Si es LECTOR, deshabilitar todos los controles de entrada
            if (rol == "LECTOR")
            {
                DeshabilitarControlesEntrada();
            }

            // Ocultar completamente botones de eliminación si no tiene permiso
            if (!Permisos.PuedeEliminar(rol))
            {
                btneliminar.Visibility = Visibility.Collapsed;
                btneliminarfi.Visibility = Visibility.Collapsed;
                btneliminarpro.Visibility = Visibility.Collapsed;
            }
        }

        private void DeshabilitarControlesEntrada()
        {
            // Deshabilitar todos los TextBox
            txtnombreares.IsEnabled = false;
            txtdetalle.IsEnabled = false;
            txtnombrefinanzas.IsEnabled = false;
            txtnombreproveedor.IsEnabled = false;

            // Deshabilitar ComboBox
            boxfinanzas.IsEnabled = false;
            boxproveedores.IsEnabled = false;
        }

        private void CargarAreas()
        {
            try
            {
                bd.MostrarAreas();

                if (bd.TablaAreas != null && bd.TablaAreas.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaAreas.Rows)
                    {
                        Console.WriteLine($"ID: {row["id_area"]}, Nombre: {row["nombre"]}");
                    }

                    dbares.ItemsSource = bd.TablaAreas.DefaultView;

                    // Forzar actualización de la UI
                    dbares.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de áreas");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar áreas: " + ex.Message);
            }
        }

        private void Cargarfinanzas()
        {
            try
            {
                bd.MostrarFinanzas();

                if (bd.TablaCategorias_Finanzas != null && bd.TablaCategorias_Finanzas.Rows.Count > 0)
                {


                    foreach (DataRow row in bd.TablaCategorias_Finanzas.Rows)
                    {
                        Console.WriteLine($"ID_categoria: {row["id_categoria"]}, Nombre_categoria: {row["nombre"]}");
                    }

                    dbfinanzas.ItemsSource = bd.TablaCategorias_Finanzas.DefaultView;

                    dbfinanzas.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de finanzas");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar finanzas: " + ex.Message);
            }
        }

        private void Cargarproveedores()
        {
            try
            {
                bd.MostrarProveedores();

                if (bd.TablaProveedores != null && bd.TablaProveedores.Rows.Count > 0)
                {


                    foreach (DataRow row in bd.TablaProveedores.Rows)
                    {
                        Console.WriteLine($"ID_Proveedor: {row["id_proveedor"]}, Nombre_Empresa: {row["nombre"]}");
                    }

                    dbproveedores.ItemsSource = bd.TablaProveedores.DefaultView;

                    dbproveedores.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de proveedores");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message);
            }
        }

        private void btnareas_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar areas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
             
                if (string.IsNullOrWhiteSpace(txtnombreares.Text))
                {
                    MessageBox.Show("Por favor ingrese un nombre para el area.");
                    return;
                }


                // Recargar datos
                CargarAreas();

                // Limpiar campos
                txtnombreares.Clear();
                txtdetalle.Clear();

                MessageBox.Show("Area agregada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar area: {ex.Message}");
            }
        }

        private void btnmodificar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar áreas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (dbares.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un área para modificar.");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar area: {ex.Message}");
            }
        }

        private void btneliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar areas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (dbares.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un area para eliminar.");
                    return;
                }

                var result = MessageBox.Show("Esta seguro de que desea eliminar esta area?",
                                           "Confirmar",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Area eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar area: {ex.Message}");
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
