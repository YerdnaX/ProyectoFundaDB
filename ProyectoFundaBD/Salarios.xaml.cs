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
    /// Lógica de interacción para Finanzas.xaml
    /// </summary>
    public partial class Salarios: Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Miembros> Miembros { get; set; }

        public Salarios(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarSalarios();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();

            Miembros = dataService.LlenarComboConMiembros();

            DataContext = this;

        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Salario - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
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
            txtdeducciones.IsEnabled = false;
            txtmonto.IsEnabled = false;


            // Deshabilitar ComboBox
            boxmiembro.IsEnabled = false;
            boxperio.IsEnabled = false;
            
        }
        private void CargarSalarios()
        {
            try
            {
                bd.MostrarSalario();

                if (bd.TablaSalarios != null && bd.TablaSalarios.Rows.Count > 0)
                {
                    dbsalarios.ItemsSource = bd.TablaSalarios.DefaultView;
                    dbsalarios.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No hay salarios registrados");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar salarios: " + ex.Message);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

        private void dbsalarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbsalarios.SelectedItem == null)
                return;

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbsalarios.SelectedItem;

                string nombreMiembro = filaSeleccionada["nombre"].ToString();
                foreach (Miembros miembro in boxmiembro.Items)
                {
                    if (miembro.Nombre == nombreMiembro)
                    {
                        boxmiembro.SelectedValue = miembro.ID_Miembros;
                        break;
                    }
                }

                if (decimal.TryParse(filaSeleccionada["monto"].ToString(), out decimal monto))
                {
                    txtmonto.Text = monto.ToString("F2");
                }

                string periodicidad = filaSeleccionada["periodicidad"].ToString();
                foreach (ComboBoxItem item in boxperio.Items)
                {
                    if (item.Content.ToString() == periodicidad)
                    {
                        boxperio.SelectedItem = item;
                        break;
                    }
                }

                if (decimal.TryParse(filaSeleccionada["deducciones"].ToString(), out decimal deducciones))
                {
                    txtdeducciones.Text = deducciones.ToString("F2");
                }

                if (DateTime.TryParse(filaSeleccionada["fecha_inicio"].ToString(), out DateTime fechaInicio))
                {
                    fechainicio.SelectedDate = fechaInicio;
                }
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
                MessageBox.Show("No tienes permisos para agregar salarios.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (boxmiembro.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona un miembro");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmonto.Text) ||
                    !decimal.TryParse(txtmonto.Text, out decimal monto))
                {
                    MessageBox.Show("Por favor ingresa un monto valido");
                    return;
                }

                if (boxperio.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona una periodicidad");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtdeducciones.Text) ||
                    !decimal.TryParse(txtdeducciones.Text, out decimal deducciones))
                {
                    MessageBox.Show("Por favor ingresa las deducciones");
                    return;
                }

                if (fechainicio.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha de inicio");
                    return;
                }

                // Obtener valores
                int idMiembro = (int)boxmiembro.SelectedValue;
                string periodicidad = ((ComboBoxItem)boxperio.SelectedItem).Content.ToString();
                DateTime fechaInicio = fechainicio.SelectedDate.Value;

                // Insertar salario
                bd.InsertarSalario(idMiembro, monto, periodicidad, deducciones, fechaInicio);

                MessageBox.Show("Salario agregado correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarSalarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar salario: {ex.Message}");
            }
        }

        private void btnmodificar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar salarios.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbsalarios.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un salario para modificar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbsalarios.SelectedItem;

                // Obtener ID directamente
                int idSalario = (int)filaSeleccionada["id_salario"];

                // Validaciones
                if (boxmiembro.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona un miembro");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmonto.Text) ||
                    !decimal.TryParse(txtmonto.Text, out decimal monto))
                {
                    MessageBox.Show("Por favor ingresa un monto valido");
                    return;
                }

                if (boxperio.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona una periodicidad");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtdeducciones.Text) ||
                    !decimal.TryParse(txtdeducciones.Text, out decimal deducciones))
                {
                    MessageBox.Show("Por favor ingresa las deducciones");
                    return;
                }

                if (fechainicio.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha de inicio");
                    return;
                }

                // Obtener valores
                int idMiembro = (int)boxmiembro.SelectedValue;
                string periodicidad = ((ComboBoxItem)boxperio.SelectedItem).Content.ToString();
                DateTime fechaInicio = fechainicio.SelectedDate.Value;

                // Obtener datos originales para el mensaje de confirmación
                string nombreMiembro = filaSeleccionada["nombre"].ToString();
                decimal montoOriginal = Convert.ToDecimal(filaSeleccionada["monto"]);

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres modificar el salario de {nombreMiembro} por {montoOriginal:F2}?",
                    "Confirmar modificacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Actualizar salario
                    bd.ActualizarSalario(idSalario, idMiembro, monto, periodicidad, deducciones, fechaInicio);

                    MessageBox.Show("Salario modificado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarSalarios();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar salario: {ex.Message}");
            }

        }

        private void btneliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar salarios.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbsalarios.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un salario para eliminar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbsalarios.SelectedItem;

                // Obtener datos
                int idSalario = (int)filaSeleccionada["id_salario"];
                string nombreMiembro = filaSeleccionada["nombre"].ToString();
                decimal monto = Convert.ToDecimal(filaSeleccionada["monto"]);

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres eliminar el salario de {nombreMiembro} por {monto:F2}?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Eliminar salario
                    bd.EliminarSalario(idSalario);

                    MessageBox.Show("Salario eliminado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarSalarios();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar salario: {ex.Message}");
            }

        }

        private void LimpiarCampos()
        {
            boxmiembro.SelectedIndex = -1;
            txtmonto.Clear();
            boxperio.SelectedIndex = -1;
            txtdeducciones.Clear();
            fechainicio.SelectedDate = null;
            dbsalarios.SelectedItem = null;
        }
    }
}
