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

                    foreach (DataRow row in bd.TablaListas.Rows)
                    {
                        Console.WriteLine($"ID_listas: {row["id_lista"]}, nombre_lista: {row["nombre"]}");
                    }

                    dblistas.ItemsSource = bd.TablaListas.DefaultView;

                    // Forzar actualizacion de la UI
                    dblistas.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de listas");
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

                    foreach (DataRow row in bd.TablaTareas.Rows)
                    {
                        Console.WriteLine($"ID_tareas: {row["id_lista"]}, Titulo: {row["titulo"]}");
                    }

                    dbtareas.ItemsSource = bd.TablaTareas.DefaultView;

                    // Forzar actualizacion de la UI
                    dbtareas.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de tareas");
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
    }
}
