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
    /// Lógica de interacción para AsignacionTareas.xaml
    /// </summary>
    public partial class AsignacionTareas : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Tareas> Tareas { get; set; }
        public ObservableCollection<Clases.Miembros> Miembros { get; set; }

        public AsignacionTareas(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            MostrarInfoUsuario();
            CargarTareasPendientes();
            var dataService = new BaseDatos();
            Tareas = dataService.LlenarComboConTareas();
            Miembros = dataService.LlenarComboConMiembros();

            DataContext = this;
        }
        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Asignacion Tareas - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }

        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnagregar.IsEnabled = Permisos.PuedeAgregar(rol);
            btnmodi.IsEnabled = Permisos.PuedeEditar(rol);
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

            // Deshabilitar ComboBox
            boxmiembro.IsEnabled = false;
            boxtarea.IsEnabled = false;
        }
        private void CargarTareasPendientes()
        {
            try
            {
                bd.MostrarTareasPendientes();

                if (bd.TablaAsignacion_Tareas != null && bd.TablaAsignacion_Tareas.Rows.Count > 0)
                {
                    dbtareaspendientes.ItemsSource = bd.TablaAsignacion_Tareas.DefaultView;
                    dbtareaspendientes.Items.Refresh();

                }
                else
                {
                    MessageBox.Show("No hay tareas pendientes");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar tareas pendientes: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();

        }

        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar asignaciones.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (boxtarea.SelectedValue == null || boxmiembro.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona una tarea y un miembro", "Datos incompletos",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int idTarea = (int)boxtarea.SelectedValue;
                int idMiembro = (int)boxmiembro.SelectedValue;

                // METODO INSERTAR
                bd.InsetarAsginarTarea(idTarea, idMiembro);

                MessageBox.Show("Tarea asignada correctamente al miembro", "Exito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar datos
                CargarTareasPendientes();

                // Limpiar campos
                boxtarea.SelectedIndex = -1;
                boxmiembro.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar tarea: {ex.Message}");
            }
        }

        private void btnmodi_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar asignaciones.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbtareaspendientes.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona una tarea de la lista para modificar", "Seleccion requerida",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (boxmiembro.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona un nuevo miembro", "Seleccion requerida",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbtareaspendientes.SelectedItem;
                string nombreTarea = filaSeleccionada["Tarea"].ToString();
                string nombreMiembroActual = filaSeleccionada["Miembro"].ToString();

                int nuevoIdMiembro = (int)boxmiembro.SelectedValue;
                string nombreNuevoMiembro = boxmiembro.Text;


                int idTarea = (int)boxtarea.SelectedValue;

                MessageBoxResult resultado = MessageBox.Show(
                    $"Quieres reasignar la tarea '{nombreTarea}'?\n\n" +
                    $"De: {nombreMiembroActual}\n" +
                    $"A: {nombreNuevoMiembro}",
                    "Confirmar modificacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    bd.ActualizarAsignarTarea(idTarea, nuevoIdMiembro);

                    MessageBox.Show("Asignacion modificada correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar datos
                    CargarTareasPendientes();

                    // Limpiar selecciones
                    boxtarea.SelectedIndex = -1;
                    boxmiembro.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar asignacion: {ex.Message}");
            }
        }

        private void btneliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar asignaciones.", "Permiso denegado",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dbtareaspendientes.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona una tarea de la lista para eliminar", "Seleccion requerida",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView filaSeleccionada = (DataRowView)dbtareaspendientes.SelectedItem;
                string nombreTarea = filaSeleccionada["Tarea"].ToString();
                string nombreMiembro = filaSeleccionada["Miembro"].ToString();

                int idTarea = (int)boxtarea.SelectedValue;
                int idMiembro = (int)boxmiembro.SelectedValue;

                MessageBoxResult resultado = MessageBox.Show(
                    $"Eliminar la asignacion de la tarea '{nombreTarea}' al miembro '{nombreMiembro}'?",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // METODO ELIMINAR con IDs directos
                    bd.EliminarAsignarTarea(idTarea, idMiembro);

                    MessageBox.Show("Asignacion eliminada correctamente", "Exito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargar datos
                    CargarTareasPendientes();

                    // Limpiar selecciones
                    dbtareaspendientes.SelectedItem = null;
                    boxtarea.SelectedIndex = -1;
                    boxmiembro.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar asignacion: {ex.Message}");
            }
        }

        private void dbtareaspendientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbtareaspendientes.SelectedItem == null)
                return;

            try
            {

                DataRowView filaSeleccionada = (DataRowView)dbtareaspendientes.SelectedItem;

                string nombreTarea = filaSeleccionada["Tarea"].ToString();
                string nombreMiembro = filaSeleccionada["Miembro"].ToString();

                foreach (Tareas tarea in boxtarea.Items)
                {
                    if (tarea.Titulo1 == nombreTarea)
                    {
                        boxtarea.SelectedValue = tarea.ID_Tareas;
                        break;
                    }
                }

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
    }
}
