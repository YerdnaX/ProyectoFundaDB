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
    public partial class Presupuesto : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Categorias_Finanzas> Categorias_Finanzas { get; set; }
        public Presupuesto(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CargarPresupuesto();
            miembroActual = miembro;
            AplicarPermisos();
            var dataService = new BaseDatos();
            Categorias_Finanzas = dataService.LlenarComboCategoriasFinanzas();
            MostrarInfoUsuario();
            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Presupuesto - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
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
            txtmontoejecutado.IsEnabled = false;
            txtmontoplaneado.IsEnabled = false;
            txtyears.IsEnabled = false;


            // Deshabilitar ComboBox
            boxcategoria.IsEnabled = false;
            boxmeses.IsEnabled = false;
        }

        private void CargarPresupuesto()
        {
            try
            {
                bd.MostrarPresupuesto();

                if (bd.TablaPresupuesto != null && bd.TablaPresupuesto.Rows.Count > 0)
                {
                    dbpresupuesto.ItemsSource = bd.TablaPresupuesto.DefaultView;
                    dbpresupuesto.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de presupuesto");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar presupuesto: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

        private void dbpresupuesto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbpresupuesto.SelectedItem == null)
                return;

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbpresupuesto.SelectedItem;

                txtyears.Text = filaSeleccionada["Año"].ToString();
                string mesSeleccionado = filaSeleccionada["Mes"].ToString();
                foreach (ComboBoxItem item in boxmeses.Items)
                {
                    if (item.Content.ToString() == mesSeleccionado)
                    {
                        boxmeses.SelectedItem = item;
                        break;
                    }
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

                if (decimal.TryParse(filaSeleccionada["Monto_Planeado"].ToString(), out decimal montoPlaneado))
                {
                    txtmontoplaneado.Text = montoPlaneado.ToString("F2");
                }

                if (decimal.TryParse(filaSeleccionada["Monto_Ejecutado"].ToString(), out decimal montoEjecutado))
                {
                    txtmontoejecutado.Text = montoEjecutado.ToString("F2");
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
                MessageBox.Show("No tienes permisos para agregar presupuesto.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtyears.Text) || !int.TryParse(txtyears.Text, out int anio))
                {
                    MessageBox.Show("Por favor ingresa un año valido");
                    return;
                }

                if (boxmeses.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un mes");
                    return;
                }

                if (boxcategoria.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona una categoria");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmontoplaneado.Text) ||
                    !decimal.TryParse(txtmontoplaneado.Text, out decimal montoPlaneado))
                {
                    MessageBox.Show("Por favor ingresa un monto planeado valido");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmontoejecutado.Text) ||
                    !decimal.TryParse(txtmontoejecutado.Text, out decimal montoEjecutado))
                {
                    MessageBox.Show("Por favor ingresa un monto ejecutado valido");
                    return;
                }

                int idCategoria = (int)boxcategoria.SelectedValue;
                string mes = ((ComboBoxItem)boxmeses.SelectedItem).Content.ToString();

                // Verificar si ya existe un presupuesto 
                if (bd.ExistePresupuesto(anio, mes, idCategoria))
                {
                    MessageBox.Show($"Ya existe un presupuesto para {mes}/{anio} en esta categoria",
                                  "Duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Insertar presupuesto
                bd.InsertarPresupuesto(anio, mes, idCategoria, montoPlaneado, montoEjecutado);

                MessageBox.Show("Presupuesto agregado correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar y limpiar
                CargarPresupuesto();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar presupuesto: {ex.Message}");
            }
        }

        private void btnmodificar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar presupuestos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbpresupuesto.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un presupuesto para modificar.");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbpresupuesto.SelectedItem;

  
                int idPresupuesto = (int)filaSeleccionada["ID"];

                // Validaciones
                if (string.IsNullOrWhiteSpace(txtyears.Text))
                {
                    MessageBox.Show("Por favor ingresa el año");
                    return;
                }

                if (boxmeses.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un mes");
                    return;
                }

                if (boxcategoria.SelectedValue == null)
                {
                    MessageBox.Show("Por favor selecciona una categoria");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmontoplaneado.Text))
                {
                    MessageBox.Show("Por favor ingresa el monto planeado");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtmontoejecutado.Text))
                {
                    MessageBox.Show("Por favor ingresa el monto ejecutado");
                    return;
                }

                // Convertir valores
                int anio = int.Parse(txtyears.Text);
                string mes = ((ComboBoxItem)boxmeses.SelectedItem).Content.ToString();
                int idCategoria = (int)boxcategoria.SelectedValue;
                decimal montoPlaneado = decimal.Parse(txtmontoplaneado.Text);
                decimal montoEjecutado = decimal.Parse(txtmontoejecutado.Text);

                // METODO ACTUALIZAR
                bd.ActualizarPresupuesto(idPresupuesto, anio, mes, idCategoria, montoPlaneado, montoEjecutado);

                // Recargar datos
                CargarPresupuesto();

                // Limpiar campos
                LimpiarCampos();

                MessageBox.Show("Presupuesto modificado correctamente.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor ingresa valores numéricos validos en los campos de monto y año.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar presupuesto: {ex.Message}");
            }
        }

        private void btneliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar presupuestos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbpresupuesto.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un presupuesto para eliminar.");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbpresupuesto.SelectedItem;

                // Obtener datos DIRECTAMENTE
                int idPresupuesto = (int)filaSeleccionada["ID"];
                string mes = filaSeleccionada["Mes"].ToString();
                int anio = (int)filaSeleccionada["Año"];
                string categoria = filaSeleccionada["Categoria"].ToString();

                MessageBoxResult resultado = MessageBox.Show(
                    $"Eliminar el presupuesto de {mes}/{anio} - {categoria}?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // METODO ELIMINAR
                    bd.EliminarPresupuesto(idPresupuesto);

                    // Recargar datos
                    CargarPresupuesto();

                    // Limpiar campos
                    LimpiarCampos();

                    MessageBox.Show("Presupuesto eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar presupuesto: {ex.Message}");
            }

        }

        private void LimpiarCampos()
        {
            txtyears.Clear();
            boxmeses.SelectedIndex = -1;
            boxcategoria.SelectedIndex = -1;
            txtmontoplaneado.Clear();
            txtmontoejecutado.Clear();
            dbpresupuesto.SelectedItem = null;
        }
    }
}
