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

                    foreach (DataRow row in bd.TablaEventos.Rows)
                    {
                        Console.WriteLine($"Tipo: {row["tipo"]}, Nombre: {row["nombreevento"]}");
                    }

                    tablaeventos.ItemsSource = bd.TablaEventos.DefaultView;

                    // Forzar actualizacion de la UI
                    tablaeventos.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de evento");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar evento: " + ex.Message);
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
                    MessageBox.Show("No hay eventos programados para este mes");
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

        }
    }
}
