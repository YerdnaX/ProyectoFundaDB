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
    public partial class Movimientos : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Categorias_Finanzas> Categorias_Finanzas { get; set; }

        public Movimientos(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarMovimientos();
            CargarResumenFinanciero();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();
            Categorias_Finanzas = dataService.LlenarComboCategoriasFinanzas();

            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Movimientos - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
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
            txtmonto.IsEnabled = false;
            txtreferencia.IsEnabled = false;

            fecha.IsEnabled = false;


            // Deshabilitar ComboBox
            boxcategoria.IsEnabled = false;
            boxtipo.IsEnabled = false;
        }

        private void CargarMovimientos()
        {
            try
            {
                bd.MostrarMovimientos();

                if (bd.TablaMovimiento != null && bd.TablaMovimiento.Rows.Count > 0)
                {
                    dbmovimiento.ItemsSource = bd.TablaMovimiento.DefaultView;
                    dbmovimiento.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No hay movimientos registrados");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar movimientos: " + ex.Message);
            }
        }

        private void CargarResumenFinanciero()
        {
            try
            {
                bd.MostrarResumenFinanciera();

                if (bd.TablaMovimiento != null && bd.TablaMovimiento.Rows.Count > 0)
                {
                    dbresumen.ItemsSource = bd.TablaMovimiento.DefaultView;
                    dbresumen.Items.Refresh();

                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar resumen financiero: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

        private void dbmovimiento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dbmovimiento.SelectedItem == null)
                return;

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbmovimiento.SelectedItem;

                if (DateTime.TryParse(filaSeleccionada["fecha"].ToString(), out DateTime fechaMov))
                {
                    fecha.SelectedDate = fechaMov;
                }

                string tipo = filaSeleccionada["tipo"].ToString();
                foreach (ComboBoxItem item in boxtipo.Items)
                {
                    if (item.Content.ToString() == tipo)
                    {
                        boxtipo.SelectedItem = item;
                        break;
                    }
                }

                string nombreCategoria = filaSeleccionada["nombre"].ToString();
                foreach (Categorias_Finanzas categoria in boxcategoria.Items)
                {
                    if (categoria.Nombre_categoria == nombreCategoria)
                    {
                        boxcategoria.SelectedValue = categoria.ID_categoria;
                        break;
                    }
                }

 
                if (decimal.TryParse(filaSeleccionada["monto"].ToString(), out decimal monto))
                {
                    txtmonto.Text = monto.ToString("F2");
                }

                txtreferencia.Text = filaSeleccionada["referencia"]?.ToString() ?? "";
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
                MessageBox.Show("No tienes permisos para agregar movimientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (fecha.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha");
                    return;
                }

                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un tipo");
                    return;
                }

                if (boxcategoria.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona una categoria");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmonto.Text) ||
                    !decimal.TryParse(txtmonto.Text, out decimal monto))
                {
                    MessageBox.Show("Por favor ingresa un monto valido");
                    return;
                }

                // Obtener valores
                DateTime fechaMov = fecha.SelectedDate.Value;
                string tipo = ((ComboBoxItem)boxtipo.SelectedItem).Content.ToString();
                int idCategoria = (int)boxcategoria.SelectedValue;
                string referencia = txtreferencia.Text.Trim();

                // Insertar movimiento
                bd.InsertarMovimiento(fechaMov, tipo, idCategoria, monto, referencia);

                MessageBox.Show("Movimiento agregado correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarMovimientos();
                CargarResumenFinanciero();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar movimiento: {ex.Message}");
            }

        }

        private void btnmodificar_Click(object sender, RoutedEventArgs e)
        {

            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar movimientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbmovimiento.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un movimiento para modificar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbmovimiento.SelectedItem;

                int idMovimiento = (int)filaSeleccionada["id_movimiento"];

                if (fecha.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha");
                    return;
                }

                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un tipo");
                    return;
                }

                if (boxcategoria.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona una categoria");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmonto.Text) ||
                    !decimal.TryParse(txtmonto.Text, out decimal monto))
                {
                    MessageBox.Show("Por favor ingresa un monto valido");
                    return;
                }

                DateTime nuevaFecha = fecha.SelectedDate.Value;
                string nuevoTipo = ((ComboBoxItem)boxtipo.SelectedItem).Content.ToString();
                int nuevaCategoriaId = (int)boxcategoria.SelectedValue;
                string nuevaReferencia = txtreferencia.Text.Trim();


                DateTime fechaOriginal = Convert.ToDateTime(filaSeleccionada["fecha"]);
                string tipoOriginal = filaSeleccionada["tipo"].ToString();
                decimal montoOriginal = Convert.ToDecimal(filaSeleccionada["monto"]);

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres modificar el movimiento del {fechaOriginal:dd/MM/yyyy} - {tipoOriginal} por {montoOriginal:F2}?",
                    "Confirmar modificacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Actualizar movimiento
                    bd.ActualizarMovimiento(idMovimiento, nuevaFecha, nuevoTipo, nuevaCategoriaId, monto, nuevaReferencia);

                    MessageBox.Show("Movimiento modificado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarMovimientos();
                    CargarResumenFinanciero();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar movimiento: {ex.Message}");
            }

        }

        private void btneliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar movimientos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbmovimiento.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un movimiento para eliminar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbmovimiento.SelectedItem;


                int idMovimiento = (int)filaSeleccionada["id_movimiento"];
                DateTime fechaMov = Convert.ToDateTime(filaSeleccionada["fecha"]);
                string tipo = filaSeleccionada["tipo"].ToString();
                decimal monto = Convert.ToDecimal(filaSeleccionada["monto"]);

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres eliminar el movimiento del {fechaMov:dd/MM/yyyy} - {tipo} por {monto:F2}?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Eliminar movimiento
                    bd.EliminarMovimiento(idMovimiento);

                    MessageBox.Show("Movimiento eliminado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarMovimientos();
                    CargarResumenFinanciero();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar movimiento: {ex.Message}");
            }
        }

        private void LimpiarCampos()
        {
            fecha.SelectedDate = null;
            boxtipo.SelectedIndex = -1;
            boxcategoria.SelectedIndex = -1;
            txtmonto.Clear();
            txtreferencia.Clear();
            dbmovimiento.SelectedItem = null;
        }
    }
}
