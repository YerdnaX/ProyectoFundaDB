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
    public partial class Movimientos : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Categorias_Finanzas> Categorias_Finanzas { get; set; }

        public Movimientos(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            CargarMovimientos();
            CargarResumenFinanciero();
            MostrarInfoUsuario();
            var dataService = new BaseDatos();
            Categorias_Finanzas = dataService.LlenarComboCategoriasFinanzas();

            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Movimientos - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
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
            txtmonto.IsEnabled = false;
            txtreferencia.IsEnabled = false;

            fecha.IsEnabled = false;


            // Deshabilitar ComboBox
            boxcategoria.IsEnabled = false;
            boxtipo.IsEnabled = false;
        }

        private void CargarMovimientos()
        {
            try
            {
                bd.MostrarMovimientos();

                if (bd.TablaMovimiento != null && bd.TablaMovimiento.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaMovimiento.Rows)
                    {
                        Console.WriteLine($"tipo: {row["tipo"]}, monto: {row["monto"]}");
                    }

                    dbmovimiento.ItemsSource = bd.TablaMovimiento.DefaultView;

                    // Forzar actualizacion de la UI
                    dbmovimiento.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de movimientos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar movimientos: " + ex.Message);
            }
        }

        private void CargarResumenFinanciero()
        {
            try
            {
                bd.MostrarResumenFinanciera();

                if (bd.TablaMovimiento != null && bd.TablaMovimiento.Rows.Count > 0)
                {
                    dbresumen.ItemsSource = bd.TablaMovimiento.DefaultView;
                    dbresumen.Items.Refresh();

                }
                else
                {
                    MessageBox.Show("No hay resumen financiero");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar resumen financiero: " + ex.Message);
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
