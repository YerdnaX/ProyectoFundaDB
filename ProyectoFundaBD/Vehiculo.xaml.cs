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
            CargarMantVehiculo();
            CargarResumenMantVehiculo();
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

            // Ocultar completamente botones de eliminacion si no tiene permiso
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
            txttaller.IsEnabled = false;
            txtkilometraje.IsEnabled = false;
            txtnotas.IsEnabled = false;

            fecha.IsEnabled = false;

            // Deshabilitar ComboBox
            boxtipo.IsEnabled = false;
            boxvehiculo.IsEnabled = false;
        }
        private void CargarMantVehiculo()
        {
            try
            {
                bd.MostrarManteVehiculo();

                if (bd.TablaMantenimiento != null && bd.TablaMantenimiento.Rows.Count > 0)
                {
                    dbmante.ItemsSource = bd.TablaMantenimiento.DefaultView;
                    dbmante.Items.Refresh();

                }
                else
                {
                    MessageBox.Show("No hay mantenimiento de vehiculo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mantenimiento vehiculo: " + ex.Message);
            }
        }
        private void CargarResumenMantVehiculo()
        {
            try
            {
                bd.MostrarResumenManteVehiculo();

                if (bd.TablaMantenimiento != null && bd.TablaMantenimiento.Rows.Count > 0)
                {
                    dbresumen.ItemsSource = bd.TablaMantenimiento.DefaultView;
                    dbresumen.Items.Refresh();

                }
                else
                {
                    MessageBox.Show("No hay mantenimiento de vehiculo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar resumen mantenimiento vehiculo: " + ex.Message);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

        private void dbmante_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbmante.SelectedItem == null)
            {
                LimpiarCampos();
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbmante.SelectedItem;

                string placa = filaSeleccionada["placa"].ToString();
                foreach (Clases.Vehiculo vehiculo in boxvehiculo.Items)
                {
                    if (vehiculo.Placa == placa)
                    {
                        boxvehiculo.SelectedValue = vehiculo.ID_vehiculo;
                        break;
                    }
                }
                string tipo = filaSeleccionada["Tipo Mantenimiento"].ToString();
                foreach (ComboBoxItem item in boxtipo.Items)
                {
                    if (item.Content.ToString() == tipo)
                    {
                        boxtipo.SelectedItem = item;
                        break;
                    }
                }

                txtconcepto.Text = filaSeleccionada["concepto"]?.ToString() ?? "";
                if (DateTime.TryParse(filaSeleccionada["fecha"].ToString(), out DateTime fechaMant))
                {
                    fecha.SelectedDate = fechaMant;
                }
                if (filaSeleccionada["kilometraje"] != DBNull.Value)
                {
                    txtkilometraje.Text = filaSeleccionada["kilometraje"].ToString();
                }
                else
                {
                    txtkilometraje.Text = "";
                }

                txtcosto.Text = filaSeleccionada["costo"]?.ToString() ?? "";

                txttaller.Text = filaSeleccionada["taller"]?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }

        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar mantenimientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (boxvehiculo.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona un vehiculo");
                    return;
                }

                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un tipo de mantenimiento");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtconcepto.Text))
                {
                    MessageBox.Show("Por favor ingresa un concepto");
                    return;
                }

                if (fecha.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtcosto.Text) ||
                    !decimal.TryParse(txtcosto.Text, out decimal costo))
                {
                    MessageBox.Show("Por favor ingresa un costo valido");
                    return;
                }


                int idVehiculo = (int)boxvehiculo.SelectedValue;
                string tipo = ((ComboBoxItem)boxtipo.SelectedItem).Content.ToString();
                string concepto = txtconcepto.Text.Trim();
                DateTime fechaMant = fecha.SelectedDate.Value;


                int? kilometraje = null;
                if (!string.IsNullOrWhiteSpace(txtkilometraje.Text) &&
                    int.TryParse(txtkilometraje.Text, out int km))
                {
                    kilometraje = km;
                }

                string taller = txttaller.Text.Trim();  
                string notas = txtnotas.Text.Trim();

                // Insertar mantenimiento
                bd.InsertarMantenimientoVehiculo(idVehiculo, tipo, concepto, fechaMant,
                                                kilometraje, costo, taller, notas);

                MessageBox.Show("Mantenimiento agregado correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarMantVehiculo();
                CargarResumenMantVehiculo();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar mantenimiento: {ex.Message}");
            }
        }

        private void btnmodificar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar mantenimientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbmante.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un mantenimiento para modificar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbmante.SelectedItem;

                // Obtener datos de la fila seleccionada
                string placa = filaSeleccionada["placa"].ToString();
                string conceptoOriginal = filaSeleccionada["concepto"].ToString();
                DateTime fechaOriginal = Convert.ToDateTime(filaSeleccionada["fecha"]);
                string tipoOriginal = filaSeleccionada["Tipo Mantenimiento"].ToString();


                int idMantenimiento = bd.ObtenerIDMantenimiento(placa, conceptoOriginal, fechaOriginal, tipoOriginal);

                // Validaciones de los nuevos valores
                if (boxvehiculo.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona un vehiculo");
                    return;
                }

                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un tipo de mantenimiento");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtconcepto.Text))
                {
                    MessageBox.Show("Por favor ingresa un concepto");
                    return;
                }

                if (fecha.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtcosto.Text) ||
                    !decimal.TryParse(txtcosto.Text, out decimal costo))
                {
                    MessageBox.Show("Por favor ingresa un costo valido");
                    return;
                }

 
                int idVehiculo = (int)boxvehiculo.SelectedValue;
                string tipo = ((ComboBoxItem)boxtipo.SelectedItem).Content.ToString();
                string concepto = txtconcepto.Text.Trim();
                DateTime fechaMant = fecha.SelectedDate.Value;

  
                int? kilometraje = null;
                if (!string.IsNullOrWhiteSpace(txtkilometraje.Text) &&
                    int.TryParse(txtkilometraje.Text, out int km))
                {
                    kilometraje = km;
                }

                string taller = txttaller.Text.Trim();
                string notas = txtnotas.Text.Trim();

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres modificar el mantenimiento '{conceptoOriginal}' del {fechaOriginal:dd/MM/yyyy}?",
                    "Confirmar modificacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Actualizar mantenimiento
                    bd.ActualizarMantenimientoVehiculo(idMantenimiento, idVehiculo, tipo,
                                                      concepto, fechaMant, kilometraje,
                                                      costo, taller, notas);

                    MessageBox.Show("Mantenimiento modificado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarMantVehiculo();
                    CargarResumenMantVehiculo();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar mantenimiento: {ex.Message}");
            }
        }

        private void btneliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar mantenimientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbmante.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un mantenimiento para eliminar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbmante.SelectedItem;


                string placa = filaSeleccionada["placa"].ToString();
                string concepto = filaSeleccionada["concepto"].ToString();
                DateTime fechaMant = Convert.ToDateTime(filaSeleccionada["fecha"]);
                string tipo = filaSeleccionada["Tipo Mantenimiento"].ToString();

                int idMantenimiento = bd.ObtenerIDMantenimiento(placa, concepto, fechaMant, tipo);

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres eliminar el mantenimiento '{concepto}' del {fechaMant:dd/MM/yyyy} para el vehiculo {placa}?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Eliminar mantenimiento
                    bd.EliminarMantenimientoVehiculo(idMantenimiento);

                    MessageBox.Show("Mantenimiento eliminado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarMantVehiculo();
                    CargarResumenMantVehiculo();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar mantenimiento: {ex.Message}");
            }
        }

        private void LimpiarCampos()
        {
            boxvehiculo.SelectedIndex = -1;
            boxtipo.SelectedIndex = -1;
            txtconcepto.Clear();
            fecha.SelectedDate = null;
            txtkilometraje.Clear();
            txtcosto.Clear();
            txttaller.Clear();  
            txtnotas.Clear();
            dbmante.SelectedItem = null;
        }
    }
}
