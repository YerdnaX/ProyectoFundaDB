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
    /// Lógica de interacción para Eventros.xaml
    /// </summary>
    public partial class Eventros : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Miembros> Miembros { get; set; }

        public Eventros(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarEventos();
            CargarEventosDelMes();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();

            Miembros = dataService.LlenarComboConMiembros();

            tablaeventos.SelectionChanged += tablaeventos_SelectionChanged;

            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Eventos - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
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
            txtlugar.IsEnabled = false;
            txtnotas.IsEnabled = false;
            txttitulo.IsEnabled = false;

            fecha.IsEnabled = false;

            // Deshabilitar ComboBox
            boxmiembro.IsEnabled = false;
            boxtipo.IsEnabled = false;
        }

        private void CargarEventos()
        {
            try
            {
                bd.MostrarEventos();

                if (bd.TablaEventos != null && bd.TablaEventos.Rows.Count > 0)
                {
                    tablaeventos.ItemsSource = bd.TablaEventos.DefaultView;
                    tablaeventos.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de eventos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar eventos: " + ex.Message);
            }
        }

        private void CargarEventosDelMes()
        {
            try
            {
                bd.MostrarEventosDelMes();

                if (bd.TablaEventos != null && bd.TablaEventos.Rows.Count > 0)
                {
                    tablavistaeventos.ItemsSource = bd.TablaEventos.DefaultView;
                    tablavistaeventos.Items.Refresh();
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar eventos del mes: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }
        
        private void btnborrar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar eventos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (tablaeventos.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un evento de la lista para eliminar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)tablaeventos.SelectedItem;
                string titulo = filaSeleccionada["nombreevento"].ToString();
                DateTime fechaEvento = DateTime.Parse(filaSeleccionada["fecha"].ToString());
                string tipo = filaSeleccionada["tipo"].ToString();

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres eliminar el evento '{titulo}'?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Obtener ID y eliminar
                    int idEvento = bd.ObtenerIDEventoDesdeVista(titulo, fechaEvento,tipo);
                    bd.EliminarEvento(idEvento);

                    MessageBox.Show("Evento eliminado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar datos
                    CargarEventos();
                    CargarEventosDelMes();

                    // Limpiar campos
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar evento: {ex.Message}");
            }
        }

        private void tablaeventos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tablaeventos.SelectedItem == null)
                return;

            try
            {
                DataRowView filaSeleccionada = (DataRowView)tablaeventos.SelectedItem;

                string tipo = filaSeleccionada["tipo"].ToString();
                foreach (ComboBoxItem item in boxtipo.Items)
                {
                    if (item.Content.ToString() == tipo)
                    {
                        boxtipo.SelectedItem = item;
                        break;
                    }
                }

                txttitulo.Text = filaSeleccionada["nombreevento"].ToString();

                if (DateTime.TryParse(filaSeleccionada["fecha"].ToString(), out DateTime fechaEvento))
                {
                    fecha.SelectedDate = fechaEvento;
                }


                txtlugar.Text = filaSeleccionada["lugar"]?.ToString() ?? "";


                txtnotas.Text = filaSeleccionada["notas"]?.ToString() ?? "";

                string nombreMiembro = filaSeleccionada["nombre_miembro"]?.ToString() ?? "";
                foreach (Miembros miembro in boxmiembro.Items)
                {
                    if (miembro.Nombre == nombreMiembro)
                    {
                        boxmiembro.SelectedValue = miembro.ID_Miembros;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos seleccionados: {ex.Message}");
            }
        }

        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar eventos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Validaciones
                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un tipo de evento");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txttitulo.Text))
                {
                    MessageBox.Show("Por favor ingresa un titulo para el evento");
                    return;
                }

                if (fecha.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha para el evento");
                    return;
                }

                string tipo = ((ComboBoxItem)boxtipo.SelectedItem).Content.ToString();
                string titulo = txttitulo.Text.Trim();
                DateTime fechaEvento = fecha.SelectedDate.Value;
                string lugar = txtlugar.Text.Trim();
                string notas = txtnotas.Text.Trim();
                int idMiembro = boxmiembro.SelectedValue != null ? (int)boxmiembro.SelectedValue : 0;

                // INSERTAR EVENTO
                bd.InsertarEvento(tipo, titulo, fechaEvento, lugar, notas, idMiembro);

                MessageBox.Show("Evento agregado correctamente", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar datos
                CargarEventos();
                CargarEventosDelMes();

                // Limpiar campos
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar evento: {ex.Message}");
            }
        }

        private void btnmodificar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar eventos.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (tablaeventos.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un evento de la lista para modificar");
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)tablaeventos.SelectedItem;

                string tituloOriginal = filaSeleccionada["nombreevento"].ToString();
                DateTime fechaOriginal = DateTime.Parse(filaSeleccionada["fecha"].ToString());
                string tipoOriginal = filaSeleccionada["tipo"].ToString();

                // Validaciones
                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un tipo de evento");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txttitulo.Text))
                {
                    MessageBox.Show("Por favor ingresa un titulo para el evento");
                    return;
                }

                if (fecha.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha para el evento");
                    return;
                }

                // Obtener ID del evento
                int idEvento = bd.ObtenerIDEventoDesdeVista(tituloOriginal, fechaOriginal, tipoOriginal);

                string tipo = ((ComboBoxItem)boxtipo.SelectedItem).Content.ToString();
                string titulo = txttitulo.Text.Trim();
                DateTime fechaEvento = fecha.SelectedDate.Value;
                string lugar = txtlugar.Text.Trim();
                string notas = txtnotas.Text.Trim();
                int idMiembro = boxmiembro.SelectedValue != null ? (int)boxmiembro.SelectedValue : 0;

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres modificar el evento '{tituloOriginal}'?",
                    "Confirmar modificacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // ACTUALIZAR EVENTO
                    bd.ActualizarEvento(idEvento, tipo, titulo, fechaEvento, lugar, notas, idMiembro);

                    MessageBox.Show("Evento modificado correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar datos
                    CargarEventos();
                    CargarEventosDelMes();

                    // Limpiar campos
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar evento: {ex.Message}");
            }
        }

        private void LimpiarCampos()
        {
            boxtipo.SelectedIndex = -1;
            txttitulo.Clear();
            fecha.SelectedDate = null;
            txtlugar.Clear();
            txtnotas.Clear();
            boxmiembro.SelectedIndex = -1;
            tablaeventos.SelectedItem = null;
        }
    }
}
