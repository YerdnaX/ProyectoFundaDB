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
    public partial class Finanzas : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Proveedores> Proveedores { get; set; }
        public ObservableCollection<Clases.Categorias_Finanzas> Categorias_Finanzas { get; set; }
        public Finanzas(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarFacturas();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();
            Proveedores = dataService.LlenarComboConProveedores();
            Categorias_Finanzas = dataService.LlenarComboCategoriasFinanzas();

            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Finanzas - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
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
            txtmonto.IsEnabled = false;
            fechaemision.IsEnabled = false;
            fechavencimiento.IsEnabled = false;


            // Deshabilitar ComboBox
            boxcategoria.IsEnabled = false;
            boxestado.IsEnabled = false;
            boxproveedor.IsEnabled = false;
        }

        private void CargarFacturas()
        {
            try
            {
                bd.MostrarFacturas();

                if (bd.TablaFacturas != null && bd.TablaFacturas.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaFacturas.Rows)
                    {
                        Console.WriteLine($"Monto: {row["monto"]}");
                    }

                    dbfacturas.ItemsSource = bd.TablaFacturas.DefaultView;

                    // Forzar actualizacion de la UI
                    dbfacturas.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de facturas");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar facturas: " + ex.Message);
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
