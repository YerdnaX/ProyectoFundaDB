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
    public partial class Presupuesto : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public ObservableCollection<Clases.Categorias_Finanzas> Categorias_Finanzas { get; set; }
        public Presupuesto(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CargarPresupuesto();
            miembroActual = miembro;
            AplicarPermisos();
            var dataService = new BaseDatos();
            Categorias_Finanzas = dataService.LlenarComboCategoriasFinanzas();
            MostrarInfoUsuario();
            DataContext = this;
        }

        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"Presupuesto - Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
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
            txtmontoejecutado.IsEnabled = false;
            txtmontoplaneado.IsEnabled = false;
            txtyears.IsEnabled = false;


            // Deshabilitar ComboBox
            boxcategoria.IsEnabled = false;
            boxmeses.IsEnabled = false;
        }

        private void CargarPresupuesto()
        {
            try
            {
                bd.MostrarPresupuesto();

                if (bd.TablaPresupuesto != null && bd.TablaPresupuesto.Rows.Count > 0)
                {

                    foreach (DataRow row in bd.TablaPresupuesto.Rows)
                    {
                        Console.WriteLine($"Año: {row["Año"]}");
                    }

                    dbpresupuesto.ItemsSource = bd.TablaPresupuesto.DefaultView;

                    // Forzar actualizacion de la UI
                    dbpresupuesto.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros en la tabla de presupuesto");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar presupuesto: " + ex.Message);
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
