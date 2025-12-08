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
    /// Lógica de interacción para Areas.xaml
    /// </summary>
    public partial class Mascotas : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Mascotas> Mascota { get; set; }
        public Mascotas(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            var dataService = new BaseDatos();
            Mascota = dataService.LlenarComboMascotas();
            CargarVeterinaria();
            CargarMedicamentos();
            CargarSalud();
            MostrarInfoUsuario();

            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Mascotas - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }

        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnagregarmed.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregarsal.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregarvet.IsEnabled = Permisos.PuedeAgregar(rol);

            btneliminarmed.IsEnabled = Permisos.PuedeEliminar(rol);
            btneliminarsal.IsEnabled = Permisos.PuedeEliminar(rol);
            btneliminarvet.IsEnabled = Permisos.PuedeEliminar(rol);

            btnmodificarmed.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificarsal.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificarvet.IsEnabled = Permisos.PuedeEditar(rol);



            // Si es LECTOR, deshabilitar todos los controles de entrada
            if (rol == "LECTOR")
            {
                DeshabilitarControlesEntrada();
            }

            // Ocultar completamente botones de eliminacion si no tiene permiso
            if (!Permisos.PuedeEliminar(rol))
            {
                btneliminarmed.Visibility = Visibility.Collapsed;
                btneliminarsal.Visibility = Visibility.Collapsed;
                btneliminarvet.Visibility = Visibility.Collapsed;
            }
        }

        private void DeshabilitarControlesEntrada()
        {
            // Deshabilitar todos los TextBox
            txtcosto.IsEnabled = false;
            txtdosis1.IsEnabled = false;
            txtevento.IsEnabled = false;
            txtfrecuencia.IsEnabled = false;
            txtmotivo.IsEnabled = false;
            txtnombre.IsEnabled = false;
            txtnombre1.IsEnabled = false;
            txtnota.IsEnabled = false;
            txtnota1.IsEnabled = false;

            fecha1.IsEnabled = false;   
            fechafinal.IsEnabled = false;
            fechainicio.IsEnabled = false;
            fechasalud.IsEnabled = false;


            // Deshabilitar ComboBox
            boxmascota.IsEnabled = false;
            boxmascota1.IsEnabled = false;
            boxmascota2.IsEnabled = false;
        }

        private void CargarVeterinaria()
        {
            try
            {
                bd.MostrarVeterinaria();

                if (bd.TablaVeterinaria != null && bd.TablaVeterinaria.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaVeterinaria.Rows)
                    {
                        Console.WriteLine($"Fecha: {row["fecha"]}, Motivo: {row["motivo"]}");
                    }

                    dbveterinaria.ItemsSource = bd.TablaVeterinaria.DefaultView;

                    // Forzar actualizacion de la UI
                    dbveterinaria.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de veterinario");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar veterinaria: " + ex.Message);
            }
        }
        private void CargarMedicamentos()
        {
            try
            {
                bd.MostrarMedicamentos();

                if (bd.TablaMedicamentos != null && bd.TablaMedicamentos.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaMedicamentos.Rows)
                    {
                        Console.WriteLine($"Nombre: {row["nombre_med"]}, Dosis: {row["dosis"]}");
                    }

                    dbmedicamentos.ItemsSource = bd.TablaMedicamentos.DefaultView;

                    // Forzar actualizacion de la UI
                    dbmedicamentos.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de medicamento");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar medicamento: " + ex.Message);
            }
        }
        private void CargarSalud()
        {
            try
            {
                bd.MostrarSalud();

                if (bd.TablaSalud != null && bd.TablaSalud.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaSalud.Rows)
                    {
                        Console.WriteLine($"Fecha: {row["fecha"]}, Evento: {row["evento"]}");
                    }

                    dbsalud.ItemsSource = bd.TablaSalud.DefaultView;

                    // Forzar actualizacion de la UI
                    dbsalud.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de salud");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar salud: " + ex.Message);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }


        private void btnInformacionDetallada_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InformacionDetallada ventanaInfo = new InformacionDetallada();
                ventanaInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la ventana de informacion: " + ex.Message,
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
