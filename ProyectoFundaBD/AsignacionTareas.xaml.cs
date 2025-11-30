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

            // Ocultar completamente botones de eliminación si no tiene permiso
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

    }
}
