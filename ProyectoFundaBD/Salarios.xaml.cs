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
    public partial class Salarios: Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Miembros> Miembros { get; set; }

        public Salarios(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarSalarios();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();

            Miembros = dataService.LlenarComboConMiembros();

            DataContext = this;

        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Salario - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
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
            txtdeducciones.IsEnabled = false;
            txtmonto.IsEnabled = false;


            // Deshabilitar ComboBox
            boxmiembro.IsEnabled = false;
            boxperio.IsEnabled = false;
            
        }
        private void CargarSalarios()
        {
            try
            {
                bd.MostrarSalario();

                if (bd.TablaSalarios != null && bd.TablaSalarios.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaSalarios.Rows)
                    {
                        Console.WriteLine($"nombre: {row["nombre"]}, monto: {row["monto"]}");
                    }

                    dbsalarios.ItemsSource = bd.TablaSalarios.DefaultView;

                    // Forzar actualizacion de la UI
                    dbsalarios.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de salarios");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar salarios: " + ex.Message);
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
