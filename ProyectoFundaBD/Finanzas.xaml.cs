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
    public partial class Finanzas : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Proveedores> Proveedores { get; set; }
        public ObservableCollection<Clases.Categorias_Finanzas> Categorias_Finanzas { get; set; }
        public Finanzas(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarFacturasAVencer();
            CargarTotalDeFacturas();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();
            Proveedores = dataService.LlenarComboConProveedores();
            Categorias_Finanzas = dataService.LlenarComboCategoriasFinanzas();



            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Finanzas - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }

        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnagregar.IsEnabled = Permisos.PuedeAgregar(rol);
            btnmodificar.IsEnabled = Permisos.PuedeEditar(rol);
            btnborrar.IsEnabled = Permisos.PuedeEliminar(rol);

            // Si es LECTOR, deshabilitar todos los controles de entrada
            if (rol == "LECTOR")
            {
                DeshabilitarControlesEntrada();
            }

            // Ocultar completamente botones de eliminación si no tiene permiso
            if (!Permisos.PuedeEliminar(rol))
            {
                btnborrar.Visibility = Visibility.Collapsed;

            }
        }

        private void DeshabilitarControlesEntrada()
        {
            // Deshabilitar todos los TextBox
            txtmonto.IsEnabled = false;
            fechaemision.IsEnabled = false;
            fechavencimiento.IsEnabled = false;


            // Deshabilitar ComboBox
            boxcategoria.IsEnabled = false;
            boxestado.IsEnabled = false;
            boxproveedor.IsEnabled = false;
        }

        private void CargarFacturasAVencer()
        {
            try
            {
                bd.MostrarFacturasAVencer();

                if (bd.TablaFacturas != null && bd.TablaFacturas.Rows.Count > 0)
                {
                    dbfacturasvencer.ItemsSource = bd.TablaFacturas.DefaultView;
                    dbfacturasvencer.Items.Refresh();

                }
                else
                {
                    MessageBox.Show("No hay facturas a vencer");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar facturas a vencer: " + ex.Message);
            }
        }

        private void CargarTotalDeFacturas()
        {
            try
            {
                bd.MostrarTotalFacturasPagar();

                if (bd.TablaFacturas != null && bd.TablaFacturas.Rows.Count > 0)
                {
                    dbfacturaspagar.ItemsSource = bd.TablaFacturas.DefaultView;
                    dbfacturaspagar.Items.Refresh();

                }
                else
                {
                    MessageBox.Show("No hay facturas a pagar");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar totoal de facturas: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

        private void dbfacturasvencer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbfacturasvencer.SelectedItem == null)
                return;

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbfacturasvencer.SelectedItem;

                string nombreProveedor = filaSeleccionada["Proveedor"].ToString();
                foreach (Proveedores proveedor in boxproveedor.Items)
                {
                    if (proveedor.Nombre_Empresa == nombreProveedor)
                    {
                        boxproveedor.SelectedValue = proveedor.ID_Proveedor;
                        break;
                    }
                }

                if (decimal.TryParse(filaSeleccionada["monto"].ToString(), out decimal monto))
                {
                    txtmonto.Text = monto.ToString("F2");
                }

                string nombreCategoria = filaSeleccionada["Categoria"].ToString();
                foreach (Categorias_Finanzas categoria in boxcategoria.Items)
                {
                    if (categoria.Nombre_categoria == nombreCategoria)
                    {
                        boxcategoria.SelectedValue = categoria.ID_categoria;
                        break;
                    }
                }

                if (fechaemision.SelectedDate == null)
                {
                    fechaemision.SelectedDate = DateTime.Today;
                }

                if (DateTime.TryParse(filaSeleccionada["fecha_venc"].ToString(), out DateTime fechaVenc))
                {
                    fechavencimiento.SelectedDate = fechaVenc;
                }

                if (filaSeleccionada.Row.Table.Columns.Contains("Estado Pago"))
                {
                    string estado = filaSeleccionada["Estado Pago"].ToString();
                    foreach (ComboBoxItem item in boxestado.Items)
                    {
                        if (item.Content.ToString() == estado)
                        {
                            boxestado.SelectedItem = item;
                            break;
                        }
                    }
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
                MessageBox.Show("No tienes permisos para agregar facturas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (boxproveedor.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona un proveedor");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmonto.Text) || !decimal.TryParse(txtmonto.Text, out decimal monto))
                {
                    MessageBox.Show("Por favor ingresa un monto valido");
                    return;
                }

                if (boxcategoria.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona una categoria");
                    return;
                }

                if (fechaemision.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha de emision");
                    return;
                }

                if (fechavencimiento.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha de vencimiento");
                    return;
                }

                if (boxestado.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un estado");
                    return;
                }

                int idProveedor = (int)boxproveedor.SelectedValue;
                int idCategoria = (int)boxcategoria.SelectedValue;
                DateTime fechaEmision = fechaemision.SelectedDate.Value;
                DateTime fechaVencimiento = fechavencimiento.SelectedDate.Value;
                string estado = ((ComboBoxItem)boxestado.SelectedItem).Content.ToString();

                // INSERTAR FACTURA
                bd.InsertarFactura(idProveedor, monto, idCategoria, fechaEmision, fechaVencimiento, estado);

                MessageBox.Show("Factura agregada correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarFacturasAVencer();
                CargarTotalDeFacturas();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar factura: {ex.Message}");
            }
        }

        private void btnmodificar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar facturas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbfacturasvencer.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona una factura de la lista para modificar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbfacturasvencer.SelectedItem;

                string proveedor = filaSeleccionada["Proveedor"].ToString();
                decimal montoOriginal = decimal.Parse(filaSeleccionada["monto"].ToString());
                DateTime fechaVencOriginal = DateTime.Parse(filaSeleccionada["fecha_venc"].ToString());
                string categoria = filaSeleccionada["Categoria"].ToString();

                // Validaciones 
                if (boxproveedor.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona un proveedor");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmonto.Text) || !decimal.TryParse(txtmonto.Text, out decimal monto))
                {
                    MessageBox.Show("Por favor ingresa un monto valido");
                    return;
                }

                if (boxcategoria.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona una categoria");
                    return;
                }

                if (fechaemision.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha de emision");
                    return;
                }

                if (fechavencimiento.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha de vencimiento");
                    return;
                }

                if (boxestado.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un estado");
                    return;
                }

                // Obtener ID de la factura
                int idFactura = bd.ObtenerIDFacturaDesdeVista(proveedor, montoOriginal, fechaVencOriginal, categoria);

                int idProveedor = (int)boxproveedor.SelectedValue;
                int idCategoria = (int)boxcategoria.SelectedValue;
                DateTime fechaEmision = fechaemision.SelectedDate.Value;
                DateTime fechaVencimiento = fechavencimiento.SelectedDate.Value;
                string estado = ((ComboBoxItem)boxestado.SelectedItem).Content.ToString();

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres modificar la factura de {proveedor} por ${montoOriginal:F2}?",
                    "Confirmar modificacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // ACTUALIZAR FACTURA
                    bd.ActualizarFactura(idFactura, idProveedor, monto, idCategoria, fechaEmision, fechaVencimiento, estado);

                    MessageBox.Show("Factura modificada correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarFacturasAVencer();
                    CargarTotalDeFacturas();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar factura: {ex.Message}");
            }

        }

        private void btnborrar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar facturas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbfacturasvencer.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona una factura de la lista para eliminar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbfacturasvencer.SelectedItem;
                string proveedor = filaSeleccionada["Proveedor"].ToString();
                decimal monto = decimal.Parse(filaSeleccionada["monto"].ToString());
                DateTime fechaVenc = DateTime.Parse(filaSeleccionada["fecha_venc"].ToString());
                string categoria = filaSeleccionada["Categoria"].ToString();

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres eliminar la factura de {proveedor} por ${monto:F2}?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    int idFactura = bd.ObtenerIDFacturaDesdeVista(proveedor, monto, fechaVenc, categoria);
                    bd.EliminarFactura(idFactura);

                    MessageBox.Show("Factura eliminada correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar y limpiar
                    CargarFacturasAVencer();
                    CargarTotalDeFacturas();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar factura: {ex.Message}");
            }
        }

        private void LimpiarCampos()
        {
            boxproveedor.SelectedIndex = -1;
            txtmonto.Clear();
            boxcategoria.SelectedIndex = -1;
            fechaemision.SelectedDate = null;
            fechavencimiento.SelectedDate = null;
            boxestado.SelectedIndex = -1;
            dbfacturasvencer.SelectedItem = null;
        }
    }
}
