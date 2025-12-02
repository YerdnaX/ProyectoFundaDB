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

        private void dbsiembra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbsiembra.SelectedItem == null)
                return;

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbsiembra.SelectedItem;

                string nombreCultivo = filaSeleccionada["nombre"].ToString();
                foreach (Clases.Cultivos cultivo in boxcultivo.Items)
                {
                    if (cultivo.Nombre == nombreCultivo)
                    {
                        boxcultivo.SelectedValue = cultivo.ID_cultivo;
                        break;
                    }
                }

                if (DateTime.TryParse(filaSeleccionada["fecha_siembra"].ToString(), out DateTime fechaSiembra))
                {
                    fechasiembra.SelectedDate = fechaSiembra;
                }

                if (DateTime.TryParse(filaSeleccionada["fecha_estim_cosecha"]?.ToString(), out DateTime fechaEstimada))
                {
                    fechaestimado.SelectedDate = fechaEstimada;
                }

                txtsector.Text = filaSeleccionada["sector"]?.ToString() ?? "";
                txtnotas.Text = filaSeleccionada["notas"]?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de siembra: {ex.Message}");
            }
        }

        private void dbtratamiento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbtratamiento.SelectedItem == null)
                return;

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbtratamiento.SelectedItem;

                string sector = filaSeleccionada["sector"].ToString();
                foreach (Clases.Siembras siembra in boxsiembra.Items)
                {
                    if (siembra.Sector == sector)
                    {
                        boxsiembra.SelectedValue = siembra.ID_siembra;
                        break;
                    }
                }

                if (DateTime.TryParse(filaSeleccionada["fecha"].ToString(), out DateTime fechaTrat))
                {
                    fecha.SelectedDate = fechaTrat;
                }

                txtproducto.Text = filaSeleccionada["producto"]?.ToString() ?? "";
                txtdosis.Text = filaSeleccionada["dosis"]?.ToString() ?? "";
                txtnotas1.Text = filaSeleccionada["notas"]?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de tratamiento: {ex.Message}");
            }
        }

        private void dbinventario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbinventario.SelectedItem == null)
                return;

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbinventario.SelectedItem;

                txtnombre.Text = filaSeleccionada["nombre"]?.ToString() ?? "";

                string tipo = filaSeleccionada["tipo"].ToString();
                foreach (ComboBoxItem item in boxtipo.Items)
                {
                    if (item.Content.ToString() == tipo)
                    {
                        boxtipo.SelectedItem = item;
                        break;
                    }
                }

                if (decimal.TryParse(filaSeleccionada["cantidad"].ToString(), out decimal cantidad))
                {
                    txtcantidad.Text = cantidad.ToString();
                }

                txtunidad.Text = filaSeleccionada["unidad"]?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de inventario: {ex.Message}");
            }
        }

        private void btnagregarsiem_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar siembras.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (boxcultivo.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona un cultivo");
                    return;
                }

                if (fechasiembra.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha de siembra");
                    return;
                }

                int idCultivo = (int)boxcultivo.SelectedValue;
                DateTime fechaSiembra = fechasiembra.SelectedDate.Value;
                DateTime? fechaEstimada = fechaestimado.SelectedDate;
                string sector = txtsector.Text.Trim();
                string notas = txtnotas.Text.Trim();

                // Insertar siembra
                bd.InsertarSiembra(idCultivo, fechaSiembra, fechaEstimada, sector, notas);

                MessageBox.Show("Siembra agregada correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarSiembra();
                LimpiarCamposSiembra();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar siembra: {ex.Message}");
            }

        }

        private void btnmodificarsiem_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar siembras.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbsiembra.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona una siembra para modificar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbsiembra.SelectedItem;


                int idSiembra = (int)filaSeleccionada["id_siembra"];

                if (boxcultivo.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona un cultivo");
                    return;
                }

                if (fechasiembra.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha de siembra");
                    return;
                }

                int idCultivo = (int)boxcultivo.SelectedValue;
                DateTime fechaSiembra = fechasiembra.SelectedDate.Value;
                DateTime? fechaEstimada = fechaestimado.SelectedDate;
                string sector = txtsector.Text.Trim();
                string notas = txtnotas.Text.Trim();

                bd.ActualizarSiembra(idSiembra, idCultivo, fechaSiembra, fechaEstimada, sector, notas);

                MessageBox.Show("Siembra modificada correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarSiembra();
                LimpiarCamposSiembra();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar siembra: {ex.Message}");
            }
        }

        private void btneliminarsiem_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar siembras.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbsiembra.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona una siembra para eliminar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbsiembra.SelectedItem;

                int idSiembra = (int)filaSeleccionada["id_siembra"];
                string nombreCultivo = filaSeleccionada["nombre"].ToString();
                DateTime fechaSiembra = Convert.ToDateTime(filaSeleccionada["fecha_siembra"]);

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres eliminar la siembra de {nombreCultivo} del {fechaSiembra:dd/MM/yyyy}?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    bd.EliminarSiembra(idSiembra);

                    MessageBox.Show("Siembra eliminada correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarSiembra();
                    LimpiarCamposSiembra();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar siembra: {ex.Message}");
            }
        }

        private void LimpiarCamposSiembra()
        {
            boxcultivo.SelectedIndex = -1;
            fechasiembra.SelectedDate = null;
            fechaestimado.SelectedDate = null;
            txtsector.Clear();
            txtnotas.Clear();
            dbsiembra.SelectedItem = null;
        }

        private void LimpiarCamposTratamiento()
        {
            boxsiembra.SelectedIndex = -1;
            fecha.SelectedDate = null;
            txtproducto.Clear();
            txtdosis.Clear();
            txtnotas1.Clear();
            dbtratamiento.SelectedItem = null;
        }

        private void LimpiarCamposInventario()
        {
            txtnombre.Clear();
            boxtipo.SelectedIndex = -1;
            txtcantidad.Clear();
            txtunidad.Clear();
            dbinventario.SelectedItem = null;
        }

        private void btnagregartrat_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar tratamientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (boxsiembra.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona una siembra");
                    return;
                }

                if (fecha.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtproducto.Text))
                {
                    MessageBox.Show("Por favor ingresa un producto");
                    return;
                }

                int idSiembra = (int)boxsiembra.SelectedValue;
                DateTime fechaTrat = fecha.SelectedDate.Value;
                string producto = txtproducto.Text.Trim();
                string dosis = txtdosis.Text.Trim();
                string notas = txtnotas1.Text.Trim();

                // Insertar tratamiento
                bd.InsertarTratamiento(idSiembra, fechaTrat, producto, dosis, notas);

                MessageBox.Show("Tratamiento agregado correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarTratamiento();
                LimpiarCamposTratamiento();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar tratamiento: {ex.Message}");
            }
        }

        private void btnmodificartrat_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar tratamientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbtratamiento.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un tratamiento para modificar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbtratamiento.SelectedItem;


                int idTratamiento = (int)filaSeleccionada["id_tratamiento"];

                // Validaciones
                if (boxsiembra.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona una siembra");
                    return;
                }

                if (fecha.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtproducto.Text))
                {
                    MessageBox.Show("Por favor ingresa un producto");
                    return;
                }

                int idSiembra = (int)boxsiembra.SelectedValue;
                DateTime fechaTrat = fecha.SelectedDate.Value;
                string producto = txtproducto.Text.Trim();
                string dosis = txtdosis.Text.Trim();
                string notas = txtnotas1.Text.Trim();

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres modificar el tratamiento de '{producto}' del {fechaTrat:dd/MM/yyyy}?",
                    "Confirmar modificacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Actualizar tratamiento
                    bd.ActualizarTratamiento(idTratamiento, idSiembra, fechaTrat, producto, dosis, notas);

                    MessageBox.Show("Tratamiento modificado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarTratamiento();
                    LimpiarCamposTratamiento();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar tratamiento: {ex.Message}");
            }
        }

        private void btneliminartrat_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar tratamientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbtratamiento.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un tratamiento para eliminar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbtratamiento.SelectedItem;

                int idTratamiento = (int)filaSeleccionada["id_tratamiento"];
                string producto = filaSeleccionada["producto"].ToString();
                DateTime fechaTrat = Convert.ToDateTime(filaSeleccionada["fecha"]);

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres eliminar el tratamiento '{producto}' del {fechaTrat:dd/MM/yyyy}?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Eliminar tratamiento
                    bd.EliminarTratamiento(idTratamiento);

                    MessageBox.Show("Tratamiento eliminado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarTratamiento();
                    LimpiarCamposTratamiento();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar tratamiento: {ex.Message}");
            }
        }

        private void btnagregarinv_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar inventario.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    MessageBox.Show("Por favor ingresa un nombre");
                    return;
                }

                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un tipo");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtcantidad.Text) ||
                    !decimal.TryParse(txtcantidad.Text, out decimal cantidad))
                {
                    MessageBox.Show("Por favor ingresa una cantidad valida");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtunidad.Text))
                {
                    MessageBox.Show("Por favor ingresa una unidad");
                    return;
                }

                string nombre = txtnombre.Text.Trim();
                string tipo = ((ComboBoxItem)boxtipo.SelectedItem).Content.ToString();
                string unidad = txtunidad.Text.Trim();

                // Insertar inventario
                bd.InsertarInventario(nombre, tipo, cantidad, unidad);

                MessageBox.Show("Inventario agregado correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarInventario();
                LimpiarCamposInventario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar inventario: {ex.Message}");
            }
        }

        private void btnmodificarinv_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar inventario.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbinventario.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un item de inventario para modificar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbinventario.SelectedItem;

                int idItem = (int)filaSeleccionada["id_item"];

                // Validaciones
                if (string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    MessageBox.Show("Por favor ingresa un nombre");
                    return;
                }

                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un tipo");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtcantidad.Text) ||
                    !decimal.TryParse(txtcantidad.Text, out decimal cantidad))
                {
                    MessageBox.Show("Por favor ingresa una cantidad valida");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtunidad.Text))
                {
                    MessageBox.Show("Por favor ingresa una unidad");
                    return;
                }

                string nombre = txtnombre.Text.Trim();
                string tipo = ((ComboBoxItem)boxtipo.SelectedItem).Content.ToString();
                string unidad = txtunidad.Text.Trim();


                string nombreOriginal = filaSeleccionada["nombre"].ToString();
                decimal cantidadOriginal = Convert.ToDecimal(filaSeleccionada["cantidad"]);

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres modificar '{nombreOriginal}' ({cantidadOriginal} {filaSeleccionada["unidad"]})?",
                    "Confirmar modificacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Actualizar inventario
                    bd.ActualizarInventario(idItem, nombre, tipo, cantidad, unidad);

                    MessageBox.Show("Inventario modificado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarInventario();
                    LimpiarCamposInventario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar inventario: {ex.Message}");
            }
        }

        private void btneliminarinv_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar inventario.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbinventario.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un item de inventario para eliminar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbinventario.SelectedItem;


                int idItem = (int)filaSeleccionada["id_item"];
                string nombre = filaSeleccionada["nombre"].ToString();
                decimal cantidad = Convert.ToDecimal(filaSeleccionada["cantidad"]);
                string unidad = filaSeleccionada["unidad"].ToString();

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres eliminar '{nombre}' ({cantidad} {unidad}) del inventario?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Eliminar inventario
                    bd.EliminarInventario(idItem);

                    MessageBox.Show("Item eliminado del inventario correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarInventario();
                    LimpiarCamposInventario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar inventario: {ex.Message}");
            }
        }
    }
}
