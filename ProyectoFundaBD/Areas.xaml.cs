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

            dbares.SelectionChanged += dbares_SelectionChanged;
            dbfinanzas.SelectionChanged += dbfinanzas_SelectionChanged;
            dbproveedores.SelectionChanged += dbproveedores_SelectionChanged;

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
                    // Limpiar seleccion antes de cargar
                    dbares.SelectedItem = null;

                    dbares.ItemsSource = bd.TablaAreas.DefaultView;
                    dbares.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de areas");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar areas: " + ex.Message);
            }
        }

        private void Cargarfinanzas()
        {
            try
            {
                bd.MostrarFinanzas();

                if (bd.TablaCategorias_Finanzas != null && bd.TablaCategorias_Finanzas.Rows.Count > 0)
                {
                    // Limpiar selección antes de cargar
                    dbfinanzas.SelectedItem = null;

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
                    // Limpiar selección antes de cargar
                    dbproveedores.SelectedItem = null;

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

                Clases.Areas nuevaArea = new Clases.Areas()
                {
                    Nombre_Area = txtnombreares.Text.Trim(),
                    Detalle_Area = txtdetalle.Text.Trim()
                };

                // Validaciones
                if (string.IsNullOrWhiteSpace(nuevaArea.Nombre_Area))
                {
                    MessageBox.Show("Por favor ingrese un nombre para el area.");
                    return;
                }

                // METODO INSERTAR
                bd.InsertarAreas(nuevaArea.Nombre_Area, nuevaArea.Detalle_Area);

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
                MessageBox.Show("No tienes permisos para modificar areas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbares.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un area para modificar.");
                return;
            }

            try
            {

                DataRowView filaSeleccionada = (DataRowView)dbares.SelectedItem;

                Clases.Areas area = new Clases.Areas()
                {
                    ID_Area = (int)filaSeleccionada["id_area"],
                    Nombre_Area = txtnombreares.Text.Trim(),
                    Detalle_Area = txtdetalle.Text.Trim()
                };

                // Validaciones
                if (string.IsNullOrWhiteSpace(area.Nombre_Area))
                {
                    MessageBox.Show("Por favor ingrese un nombre para el area.");
                    return;
                }

                if (area.Nombre_Area.Length > 60)
                {
                    MessageBox.Show("El nombre no puede tener mas de 60 caracteres.");
                    return;
                }

                if (area.Detalle_Area.Length > 200)
                {
                    MessageBox.Show("El detalle no puede tener mas de 200 caracteres.");
                    return;
                }

                // METODO ACTUALIZAR
                bd.ActualizarArea(area.ID_Area, area.Nombre_Area, area.Detalle_Area);

                // Recargar datos
                CargarAreas();

                // Limpiar campos
                txtnombreares.Clear();
                txtdetalle.Clear();

                MessageBox.Show("Área modificada correctamente.");
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

            if (dbares.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un area para eliminar.");
                return;
            }

            try
            {


                DataRowView filaSeleccionada = (DataRowView)dbares.SelectedItem;

                Clases.Areas area = new Clases.Areas()
                {
                    ID_Area = (int)filaSeleccionada["id_area"],
                    Nombre_Area = filaSeleccionada["nombre"].ToString()
                };

                MessageBoxResult resultado = MessageBox.Show(
                    $"Desea eliminar el area: {area.Nombre_Area}?",
                    "Confirmar Eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // METODO ELIMINAR
                    bd.EliminarArea(area.ID_Area);

                    // Recargar datos
                    CargarAreas();

                    // Limpiar campos
                    txtnombreares.Clear();
                    txtdetalle.Clear();

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
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

        private void dbares_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbares.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dbares.SelectedItem;

                // Cargar datos en los TextBox
                txtnombreares.Text = fila["nombre"].ToString();
                txtdetalle.Text = fila["detalle"].ToString();
            }
        }

        private void btnfinanzas_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar categorías de finanzas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Categorias_Finanzas nuevaCategoria = new Categorias_Finanzas()
                {
                    Nombre_categoria = txtnombrefinanzas.Text.Trim(),
                    Tipo1 = (boxfinanzas.SelectedItem as ComboBoxItem)?.Content.ToString()
                };

                // Validaciones
                if (string.IsNullOrWhiteSpace(nuevaCategoria.Nombre_categoria))
                {
                    MessageBox.Show("Por favor ingrese un nombre para la categoría.");
                    return;
                }

                if (boxfinanzas.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un tipo.");
                    return;
                }

                // METODO INSERTAR
                bd.InsertarCategoriasFinanzas(nuevaCategoria.Nombre_categoria, nuevaCategoria.Tipo1);

                // Recargar datos
                Cargarfinanzas();

                // Limpiar campos
                txtnombrefinanzas.Clear();
                boxfinanzas.SelectedIndex = -1;

                MessageBox.Show("Categoria de finanzas agregada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar categoria: {ex.Message}");
            }
        }

        private void btnmodificarfi_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar categorias de finanzas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbfinanzas.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una categoria para modificar.");
                return;
            }

            try
            {

                DataRowView filaSeleccionada = (DataRowView)dbfinanzas.SelectedItem;

                Categorias_Finanzas categoria = new Categorias_Finanzas()
                {
                    ID_categoria = (int)filaSeleccionada["id_categoria"],
                    Nombre_categoria = txtnombrefinanzas.Text.Trim(),
                    Tipo1 = (boxfinanzas.SelectedItem as ComboBoxItem)?.Content.ToString()
                };

                // Validaciones
                if (string.IsNullOrWhiteSpace(categoria.Nombre_categoria))
                {
                    MessageBox.Show("Por favor ingrese un nombre para la categoria.");
                    return;
                }

                if (boxfinanzas.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un tipo.");
                    return;
                }

                // METODO ACTUALIZAR
                bd.ActualizarCategoriaFinanzas(categoria.ID_categoria, categoria.Nombre_categoria, categoria.Tipo1);

                // Recargar datos
                Cargarfinanzas();

                // Limpiar campos
                txtnombrefinanzas.Clear();
                boxfinanzas.SelectedIndex = -1;

                MessageBox.Show("Categoria de finanzas modificada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar categoria: {ex.Message}");
            }
        }

        private void btneliminarfi_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar categorias de finanzas.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbfinanzas.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una categoria para eliminar.");
                return;
            }

            try
            {

                DataRowView filaSeleccionada = (DataRowView)dbfinanzas.SelectedItem;

                Categorias_Finanzas categoria = new Categorias_Finanzas()
                {
                    ID_categoria = (int)filaSeleccionada["id_categoria"],
                    Nombre_categoria = filaSeleccionada["nombre"].ToString()
                };

                // Confirmacion de eliminacion
                MessageBoxResult resultado = MessageBox.Show(
                    $"Desea eliminar la categoria: {categoria.Nombre_categoria}?",
                    "Confirmar Eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // METODO ELIMINAR
                    bd.EliminarCategoriaFinanzas(categoria.ID_categoria);

                    // Recargar datos
                    Cargarfinanzas();

                    // Limpiar campos
                    txtnombrefinanzas.Clear();
                    boxfinanzas.SelectedIndex = -1;

                    MessageBox.Show("Categoría de finanzas eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar categoria: {ex.Message}");
            }
        }

        private void btnproveedor_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar proveedores.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {

                Proveedores nuevoProveedor = new Proveedores()
                {
                    Nombre_Empresa = txtnombreproveedor.Text.Trim(),
                    Tipo = (boxproveedores.SelectedItem as ComboBoxItem)?.Content.ToString()
                };

                // Validaciones
                if (string.IsNullOrWhiteSpace(nuevoProveedor.Nombre_Empresa))
                {
                    MessageBox.Show("Por favor ingrese un nombre para el proveedor.");
                    return;
                }

                if (boxproveedores.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un tipo.");
                    return;
                }

                // METODO INSERTAR
                bd.InsertarProveedores(nuevoProveedor.Nombre_Empresa, nuevoProveedor.Tipo);

                // Recargar datos
                Cargarproveedores();

                // Limpiar campos
                txtnombreproveedor.Clear();
                boxproveedores.SelectedIndex = -1;

                MessageBox.Show("Proveedor agregado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar proveedor: {ex.Message}");
            }
        }

        private void btnmodificarPro_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar proveedores.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbproveedores.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un proveedor para modificar.");
                return;
            }

            try
            {

                DataRowView filaSeleccionada = (DataRowView)dbproveedores.SelectedItem;

                Proveedores proveedor = new Proveedores()
                {
                    ID_Proveedor = (int)filaSeleccionada["id_proveedor"],
                    Nombre_Empresa = txtnombreproveedor.Text.Trim(),
                    Tipo = (boxproveedores.SelectedItem as ComboBoxItem)?.Content.ToString()
                };

                // Validaciones
                if (string.IsNullOrWhiteSpace(proveedor.Nombre_Empresa))
                {
                    MessageBox.Show("Por favor ingrese un nombre para el proveedor.");
                    return;
                }

                if (boxproveedores.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un tipo.");
                    return;
                }

                // METODO ACTUALIZAR
                bd.ActualizarProveedor(proveedor.ID_Proveedor, proveedor.Nombre_Empresa, proveedor.Tipo);

                // Recargar datos
                Cargarproveedores();

                // Limpiar campos
                txtnombreproveedor.Clear();
                boxproveedores.SelectedIndex = -1;

                MessageBox.Show("Proveedor modificado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar proveedor: {ex.Message}");
            }
        }

        private void btneliminarpro_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar proveedores.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbproveedores.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un proveedor para eliminar.");
                return;
            }

            try
            {

                DataRowView filaSeleccionada = (DataRowView)dbproveedores.SelectedItem;

                Proveedores proveedor = new Proveedores()
                {
                    ID_Proveedor = (int)filaSeleccionada["id_proveedor"],
                    Nombre_Empresa = filaSeleccionada["nombre"].ToString()
                };

                // Confirmación de eliminacion
                MessageBoxResult resultado = MessageBox.Show(
                    $"Desea eliminar el proveedor: {proveedor.Nombre_Empresa}?",
                    "Confirmar Eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // METODO ELIMINAR
                    bd.EliminarProveedor(proveedor.ID_Proveedor);

                    // Recargar datos
                    Cargarproveedores();

                    // Limpiar campos
                    txtnombreproveedor.Clear();
                    boxproveedores.SelectedIndex = -1;

                    MessageBox.Show("Proveedor eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar proveedor: {ex.Message}");
            }
        }

        private void dbfinanzas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbfinanzas.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dbfinanzas.SelectedItem;
                txtnombrefinanzas.Text = fila["nombre"].ToString();

                // Seleccionar el tipo en el ComboBox
                string tipo = fila["tipo"].ToString();
                foreach (ComboBoxItem item in boxfinanzas.Items)
                {
                    if (item.Content.ToString() == tipo)
                    {
                        boxfinanzas.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void dbproveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbproveedores.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dbproveedores.SelectedItem;
                txtnombreproveedor.Text = fila["nombre"].ToString();

                // Seleccionar el tipo en el ComboBox
                string tipo = fila["tipo"].ToString();
                foreach (ComboBoxItem item in boxproveedores.Items)
                {
                    if (item.Content.ToString() == tipo)
                    {
                        boxproveedores.SelectedItem = item;
                        break;
                    }
                }
            }
        }
    }
}
