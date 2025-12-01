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
    public partial class ListasyTareas : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Miembros> Miembros { get; set; }

        public ObservableCollection<Clases.Areas> Areas { get; set; }

        public ObservableCollection<Clases.Listas> Listas { get; set; }

        public ListasyTareas(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarListas();
            CargarTareas();
            MostrarInfoUsuario();

            var dataService = new BaseDatos();
            Miembros = dataService.LlenarComboConMiem();
            Areas = dataService.LlenarComboConAreas();
            Listas = dataService.LlenarComboConListas();

            DataContext = this;

            dblistas.SelectionChanged += dblistas_SelectionChanged;
            dbtareas.SelectionChanged += dbtareas_SelectionChanged;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Lista y Tareas - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }

        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnagregarlista.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregartarea.IsEnabled = Permisos.PuedeAgregar(rol);

            btneliminarlista.IsEnabled = Permisos.PuedeEditar(rol);
            btneliminartarea.IsEnabled = Permisos.PuedeEditar(rol);

            btnmodificarlista.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificartarea.IsEnabled = Permisos.PuedeEditar(rol);


            // Si es LECTOR, deshabilitar todos los controles de entrada
            if (rol == "LECTOR")
            {
                DeshabilitarControlesEntrada();
            }

            // Ocultar completamente botones de eliminación si no tiene permiso
            if (!Permisos.PuedeEliminar(rol))
            {
                btneliminarlista.Visibility = Visibility.Collapsed;
                btneliminartarea.Visibility = Visibility.Collapsed;
            }
        }

        private void DeshabilitarControlesEntrada()
        {
            // Deshabilitar todos los TextBox
            txtdescrip.IsEnabled = false;
            txtnombre.IsEnabled = false;
            txttitulo.IsEnabled = false;

            fecha.IsEnabled = false;
            fechacreacion.IsEnabled = false;
            fechalimite.IsEnabled = false;


            // Deshabilitar ComboBox
            boxarea.IsEnabled = false;
            boxarea2.IsEnabled = false;
            boxestado.IsEnabled = false;    
            boxlista.IsEnabled = false;
            boxmiembro.IsEnabled = false;
            boxprioridad.IsEnabled = false;
            boxtipo.IsEnabled = false;

        }
        private void CargarListas()
        {
            try
            {
                bd.MostrarListas();
                if (bd.TablaListas != null && bd.TablaListas.Rows.Count > 0)
                {
                    dblistas.SelectedItem = null;
                    dblistas.ItemsSource = bd.TablaListas.DefaultView;
                    dblistas.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar listas: " + ex.Message);
            }
        }
        private void CargarTareas()
        {
            try
            {
                bd.MostrarTareas();
                if (bd.TablaTareas != null && bd.TablaTareas.Rows.Count > 0)
                {
                    dbtareas.SelectedItem = null;
                    dbtareas.ItemsSource = bd.TablaTareas.DefaultView;
                    dbtareas.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar tareas: " + ex.Message);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

        private void dblistas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dblistas.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dblistas.SelectedItem;
                txtnombre.Text = fila["nombre"].ToString();

                string tipo = fila["tipo"].ToString();
                foreach (ComboBoxItem item in boxtipo.Items)
                {
                    if (item.Content.ToString() == tipo)
                    {
                        boxtipo.SelectedItem = item;
                        break;
                    }
                }

                if (fila["id_area"] != DBNull.Value)
                {
                    int idArea = (int)fila["id_area"];
                    boxarea.SelectedValue = idArea;
                }


                if (fila["creada_por"] != DBNull.Value)
                {
                    int idMiembro = (int)fila["creada_por"];
                    boxmiembro.SelectedValue = idMiembro;
                }

                if (fila["fecha_creada"] != DBNull.Value)
                {
                    fecha.SelectedDate = (DateTime)fila["fecha_creada"];
                }
            }
        }

        private void dbtareas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbtareas.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dbtareas.SelectedItem;

                if (fila["id_lista"] != DBNull.Value)
                {
                    int idLista = (int)fila["id_lista"];
                    boxlista.SelectedValue = idLista;
                }

                txttitulo.Text = fila["titulo"].ToString();
                txtdescrip.Text = fila["descripcion"].ToString();

                string prioridad = fila["prioridad"].ToString();
                foreach (ComboBoxItem item in boxprioridad.Items)
                {
                    if (item.Content.ToString() == prioridad)
                    {
                        boxprioridad.SelectedItem = item;
                        break;
                    }
                }

                string estado = fila["estado"].ToString();
                foreach (ComboBoxItem item in boxestado.Items)
                {
                    if (item.Content.ToString() == estado)
                    {
                        boxestado.SelectedItem = item;
                        break;
                    }
                }

                string repeticion = fila["repeticion"].ToString();
                foreach (ComboBoxItem item in boxrepeticion.Items)
                {
                    if (item.Content.ToString() == repeticion)
                    {
                        boxrepeticion.SelectedItem = item;
                        break;
                    }
                }

                if (fila["id_area"] != DBNull.Value)
                {
                    int idArea = (int)fila["id_area"];
                    boxarea2.SelectedValue = idArea;
                }


                if (fila["fecha_creacion"] != DBNull.Value)
                {
                    fechacreacion.SelectedDate = (DateTime)fila["fecha_creacion"];
                }

                if (fila["fecha_limite"] != DBNull.Value)
                {
                    fechalimite.SelectedDate = (DateTime)fila["fecha_limite"];
                }
            }
        }

        private void LimpiarCamposListas()
        {
            txtnombre.Clear();
            boxtipo.SelectedIndex = -1;
            boxarea.SelectedIndex = -1;
            boxmiembro.SelectedIndex = -1;
            fecha.SelectedDate = null;
        }

        private void LimpiarCamposTareas()
        {
            boxlista.SelectedIndex = -1;
            txttitulo.Clear();
            txtdescrip.Clear();
            boxprioridad.SelectedIndex = -1;
            boxestado.SelectedIndex = -1;
            boxrepeticion.SelectedIndex = -1;
            boxarea2.SelectedIndex = -1;
            fechacreacion.SelectedDate = null;
            fechalimite.SelectedDate = null;
        }

        private void btnagregarlista_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar listas.");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    MessageBox.Show("Por favor ingrese un nombre para la lista.");
                    return;
                }

                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un tipo.");
                    return;
                }

                if (!fecha.SelectedDate.HasValue)
                {
                    MessageBox.Show("Por favor seleccione una fecha.");
                    return;
                }
                DateOnly fechaCreada = DateOnly.FromDateTime(fecha.SelectedDate.Value);

                int? idArea = boxarea.SelectedValue as int?;
                int? idMiembro = boxmiembro.SelectedValue as int?;

                bd.InsertarLista(txtnombre.Text.Trim(),
                                (boxtipo.SelectedItem as ComboBoxItem).Content.ToString(),
                                idArea, idMiembro, fechaCreada);

                CargarListas();
                LimpiarCamposListas();
                MessageBox.Show("Lista agregada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar lista: {ex.Message}");
            }
        }

        private void btnmodificarlista_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar listas.");
                return;
            }

            if (dblistas.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una lista para modificar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dblistas.SelectedItem;
                int id = (int)fila["id_lista"];

                if (string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    MessageBox.Show("Por favor ingrese un nombre para la lista.");
                    return;
                }

                if (boxtipo.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un tipo.");
                    return;
                }

                if (!fecha.SelectedDate.HasValue)
                {
                    MessageBox.Show("Por favor seleccione una fecha.");
                    return;
                }

                int? idArea = boxarea.SelectedValue as int?;
                int? idMiembro = boxmiembro.SelectedValue as int?;

                DateOnly fechaCreada = DateOnly.FromDateTime(fecha.SelectedDate.Value);

                bd.ActualizarLista(id, txtnombre.Text.Trim(),
                                  (boxtipo.SelectedItem as ComboBoxItem).Content.ToString(),
                                  idArea, idMiembro, fechaCreada);

                CargarListas();
                LimpiarCamposListas();
                MessageBox.Show("Lista modificada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar lista: {ex.Message}");
            }
        }

        private void btneliminarlista_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar listas.");
                return;
            }

            if (dblistas.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una lista para eliminar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dblistas.SelectedItem;
                int id = (int)fila["id_lista"];
                string nombre = fila["nombre"].ToString();

                if (MessageBox.Show($"Eliminar la lista: {nombre}?", "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bd.EliminarLista(id);
                    CargarListas();
                    LimpiarCamposListas();
                    MessageBox.Show("Lista eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar lista: {ex.Message}");
            }

        }

        private void btnagregartarea_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar tareas.");
                return;
            }

            try
            {
                if (boxlista.SelectedValue == null)
                {
                    MessageBox.Show("Por favor seleccione una lista.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txttitulo.Text))
                {
                    MessageBox.Show("Por favor ingrese un titulo para la tarea.");
                    return;
                }

                if (boxprioridad.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione una prioridad.");
                    return;
                }

                if (boxestado.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione un estado.");
                    return;
                }

                if (boxrepeticion.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione una repeticion.");
                    return;
                }

                if (!fechacreacion.SelectedDate.HasValue)
                {
                    MessageBox.Show("Por favor seleccione una fecha de creacion.");
                    return;
                }

                int idLista = (int)boxlista.SelectedValue;
                int? idArea = boxarea2.SelectedValue as int?;
                DateOnly fechaCreacion = DateOnly.FromDateTime(fechacreacion.SelectedDate.Value);
                DateOnly? fechaLimite = fechalimite.SelectedDate.HasValue ?
                                       DateOnly.FromDateTime(fechalimite.SelectedDate.Value) :
                                       (DateOnly?)null;

                bd.InsertarTarea(idLista, txttitulo.Text.Trim(), txtdescrip.Text.Trim(),
                                (boxprioridad.SelectedItem as ComboBoxItem).Content.ToString(),
                                (boxestado.SelectedItem as ComboBoxItem).Content.ToString(),
                                fechaCreacion, fechaLimite,
                                (boxrepeticion.SelectedItem as ComboBoxItem).Content.ToString(),
                                idArea);

                CargarTareas();
                LimpiarCamposTareas();
                MessageBox.Show("Tarea agregada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar tarea: {ex.Message}");
            }
        }

        private void btnmodificartarea_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar tareas.");
                return;
            }

            if (dbtareas.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una tarea para modificar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dbtareas.SelectedItem;
                int id = (int)fila["id_tarea"];

                if (boxlista.SelectedValue == null)
                {
                    MessageBox.Show("Por favor seleccione una lista.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txttitulo.Text))
                {
                    MessageBox.Show("Por favor ingrese un titulo para la tarea.");
                    return;
                }

                int idLista = (int)boxlista.SelectedValue;
                int? idArea = boxarea2.SelectedValue as int?;

                DateOnly fechaCreacion = fechacreacion.SelectedDate.HasValue ?
                                DateOnly.FromDateTime(fechacreacion.SelectedDate.Value) :
                                DateOnly.FromDateTime(DateTime.Now);

                DateOnly? fechaLimite = fechalimite.SelectedDate.HasValue ?
                                       DateOnly.FromDateTime(fechalimite.SelectedDate.Value) :
                                       (DateOnly?)null;

                bd.ActualizarTarea(id, idLista, txttitulo.Text.Trim(), txtdescrip.Text.Trim(),
                                  (boxprioridad.SelectedItem as ComboBoxItem).Content.ToString(),
                                  (boxestado.SelectedItem as ComboBoxItem).Content.ToString(),
                                  fechaCreacion, fechaLimite,
                                  (boxrepeticion.SelectedItem as ComboBoxItem).Content.ToString(),
                                  idArea);

                CargarTareas();
                LimpiarCamposTareas();
                MessageBox.Show("Tarea modificada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar tarea: {ex.Message}");
            }
        }

        private void btneliminartarea_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar tareas.");
                return;
            }

            if (dbtareas.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una tarea para eliminar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dbtareas.SelectedItem;
                int id = (int)fila["id_tarea"];
                string titulo = fila["titulo"].ToString();

                if (MessageBox.Show($"Eliminar la tarea: {titulo}?", "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bd.EliminarTarea(id);
                    CargarTareas();
                    LimpiarCamposTareas();
                    MessageBox.Show("Tarea eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar tarea: {ex.Message}");
            }

        }
    }
}
