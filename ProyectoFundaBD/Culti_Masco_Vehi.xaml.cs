using Clases;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Culti_Masco_Vehi.xaml
    /// </summary>
    public partial class Culti_Masco_Vehi : Window
    {
        BaseDatos bd = new BaseDatos();
        private Miembros miembroActual;
        public Culti_Masco_Vehi(Miembros miembro)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            miembroActual = miembro;
            AplicarPermisos();
            MostrarInfoUsuario();
            CargarCultivos();
            CargarMascotas();
            CargarVehiculo();

            dbcultivos.SelectionChanged += dbcultivos_SelectionChanged;
            dbmascotas.SelectionChanged += dbmascotas_SelectionChanged;
            dbvehiculo.SelectionChanged += dbvehiculo_SelectionChanged;
        }
        private void MostrarInfoUsuario()
        {
            // Mostrar 
            Title = $"- Usuario: {miembroActual.Nombre} ({miembroActual.Rol})";
        }

        private void AplicarPermisos()
        {
            string rol = miembroActual.Rol.ToUpper();

            // Aplicar permisos a los botones 
            btnagregarcult.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregarmasc.IsEnabled = Permisos.PuedeAgregar(rol);
            btnagregarveh.IsEnabled = Permisos.PuedeAgregar(rol);

            btneliminarcult.IsEnabled = Permisos.PuedeEliminar(rol);
            btneliminarmas.IsEnabled = Permisos.PuedeEliminar(rol);
            btneliminarveh.IsEnabled = Permisos.PuedeEliminar(rol);

            btnmodificarcult.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificarmas.IsEnabled = Permisos.PuedeEditar(rol);
            btnmodificarveh.IsEnabled = Permisos.PuedeEditar(rol);


            // Si es LECTOR, deshabilitar todos los controles de entrada
            if (rol == "LECTOR")
            {
                DeshabilitarControlesEntrada();
            }

            // Ocultar completamente botones de eliminacion si no tiene permiso
            if (!Permisos.PuedeEliminar(rol))
            {
                btneliminarcult.Visibility = Visibility.Collapsed;
                btneliminarveh.Visibility = Visibility.Collapsed;
                btneliminarmas.Visibility = Visibility.Collapsed;
            }
        }
        private void DeshabilitarControlesEntrada()
        {
            // Deshabilitar todos los TextBox
            txtnombrecultivo.IsEnabled = false;
            txtdetalle.IsEnabled = false;
            txtnombremascota.IsEnabled = false;
            txtpesp.IsEnabled = false;
            txtraza.IsEnabled = false;
            txtplaca.IsEnabled = false;
            txtmarca.IsEnabled = false;
            txtmodelo.IsEnabled = false;
            txtyear.IsEnabled = false;
            txtpoliza.IsEnabled = false;
            fechadekra.IsEnabled = false;
            fechamascota.IsEnabled = false;


            // Deshabilitar ComboBox
            boxespecie.IsEnabled = false;
        }
        private void CargarCultivos()
        {
            try
            {
                bd.MostrarCultivos();
                if (bd.TablaCultivos != null && bd.TablaCultivos.Rows.Count > 0)
                {
                    dbcultivos.SelectedItem = null;
                    dbcultivos.ItemsSource = bd.TablaCultivos.DefaultView;
                    dbcultivos.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar cultivos: " + ex.Message);
            }
        }
        private void CargarMascotas()
        {
            try
            {
                bd.MostrarMascotas();
                if (bd.TablaMascotas != null && bd.TablaMascotas.Rows.Count > 0)
                {
                    dbmascotas.SelectedItem = null;
                    dbmascotas.ItemsSource = bd.TablaMascotas.DefaultView;
                    dbmascotas.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mascotas: " + ex.Message);
            }
        }
        private void CargarVehiculo()
        {
            try
            {
                bd.MostrarVehiculo();
                if (bd.TablaVehiculos != null && bd.TablaVehiculos.Rows.Count > 0)
                {
                    dbvehiculo.SelectedItem = null;
                    dbvehiculo.ItemsSource = bd.TablaVehiculos.DefaultView;
                    dbvehiculo.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar vehículos: " + ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el menu principal
            MenuPrincipal menuPrincipal = new MenuPrincipal(miembroActual);
            menuPrincipal.Show();


        }

        private void dbcultivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dbcultivos.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dbcultivos.SelectedItem;
                txtnombrecultivo.Text = fila["nombre"].ToString();
                txtdetalle.Text = fila["variedad"].ToString();
            }
        }

        private void dbmascotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbmascotas.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dbmascotas.SelectedItem;
                txtnombremascota.Text = fila["nombre"].ToString();

                string especie = fila["especie"].ToString();
                boxespecie.SelectedItem = especie;

                txtraza.Text = fila["raza"].ToString();

                if (fila["fecha_nac"] != DBNull.Value)
                {
                    DateTime fecha = (DateTime)fila["fecha_nac"];
                    fechamascota.SelectedDate = fecha;
                }

                if (fila["peso_kg"] != DBNull.Value)
                {
                    txtpesp.Text = fila["peso_kg"].ToString();
                }
            }

        }

        private void dbvehiculo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbvehiculo.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dbvehiculo.SelectedItem;
                txtplaca.Text = fila["placa"].ToString();
                txtmarca.Text = fila["marca"].ToString();
                txtmodelo.Text = fila["modelo"].ToString();
                txtyear.Text = fila["anio"].ToString();
                txtpoliza.Text = fila["poliza"].ToString();

                if (fila["dekra_fecha"] != DBNull.Value)
                {
                    DateTime fecha = (DateTime)fila["dekra_fecha"];
                    fechadekra.SelectedDate = fecha;
                }
            }
        }

        private void LimpiarCamposCultivos()
        {
            txtnombrecultivo.Clear();
            txtdetalle.Clear();
        }

        private void LimpiarCamposMascotas()
        {
            txtnombremascota.Clear();
            boxespecie.SelectedIndex = -1;
            txtraza.Clear();
            fechamascota.SelectedDate = null;
            txtpesp.Clear();
        }

        private void LimpiarCamposVehiculos()
        {
            txtplaca.Clear();
            txtmarca.Clear();
            txtmodelo.Clear();
            txtyear.Clear();
            txtpoliza.Clear();
            fechadekra.SelectedDate = null;
        }

        private void btnagregarcult_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar cultivos.");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtnombrecultivo.Text))
                {
                    MessageBox.Show("Por favor ingrese un nombre para el cultivo.");
                    return;
                }

                bd.InsertarCultivo(txtnombrecultivo.Text.Trim(), txtdetalle.Text.Trim());
                CargarCultivos();
                LimpiarCamposCultivos();
                MessageBox.Show("Cultivo agregado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar cultivo: {ex.Message}");
            }

        }

        private void btnmodificarcult_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar cultivos.");
                return;
            }

            if (dbcultivos.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un cultivo para modificar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dbcultivos.SelectedItem;
                int id = (int)fila["id_cultivo"];

                bd.ActualizarCultivo(id, txtnombrecultivo.Text.Trim(), txtdetalle.Text.Trim());
                CargarCultivos();
                LimpiarCamposCultivos();
                MessageBox.Show("Cultivo modificado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar cultivo: {ex.Message}");
            }
        }

        private void btneliminarcult_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar cultivos.");
                return;
            }

            if (dbcultivos.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un cultivo para eliminar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dbcultivos.SelectedItem;
                int id = (int)fila["id_cultivo"];
                string nombre = fila["nombre"].ToString();

                if (MessageBox.Show($"Eliminar el cultivo: {nombre}?", "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bd.EliminarCultivo(id);
                    CargarCultivos();
                    LimpiarCamposCultivos();
                    MessageBox.Show("Cultivo eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar cultivo: {ex.Message}");
            }

        }

        private void btnagregarmasc_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar mascotas.");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtnombremascota.Text))
                {
                    MessageBox.Show("Por favor ingrese un nombre para la mascota.");
                    return;
                }

                if (boxespecie.SelectedItem == null)
                {
                    MessageBox.Show("Por favor seleccione una especie.");
                    return;
                }

                DateOnly fechaNac = fechamascota.SelectedDate.HasValue ?
                         DateOnly.FromDateTime(fechamascota.SelectedDate.Value) :
                         DateOnly.MinValue;
                decimal peso = decimal.TryParse(txtpesp.Text, out decimal p) ? p : 0;

                bd.InsertarMascota(txtnombremascota.Text.Trim(), boxespecie.SelectedItem.ToString(),
                                  txtraza.Text.Trim(), fechaNac, peso);
                CargarMascotas();
                LimpiarCamposMascotas();
                MessageBox.Show("Mascota agregada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar mascota: {ex.Message}");
            }

        }

        private void btnmodificarmas_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar mascotas.");
                return;
            }

            if (dbmascotas.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una mascota para modificar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dbmascotas.SelectedItem;
                int id = (int)fila["id_mascota"];

                DateOnly fechaNac = fechamascota.SelectedDate.HasValue ?
                           DateOnly.FromDateTime(fechamascota.SelectedDate.Value) :
                           DateOnly.MinValue;
                decimal peso = decimal.TryParse(txtpesp.Text, out decimal p) ? p : 0;

                bd.ActualizarMascota(id, txtnombremascota.Text.Trim(), boxespecie.SelectedItem.ToString(),
                                   txtraza.Text.Trim(), fechaNac, peso);
                CargarMascotas();
                LimpiarCamposMascotas();
                MessageBox.Show("Mascota modificada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar mascota: {ex.Message}");
            }

        }

        private void btneliminarmas_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar mascotas.");
                return;
            }

            if (dbmascotas.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una mascota para eliminar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dbmascotas.SelectedItem;
                int id = (int)fila["id_mascota"];
                string nombre = fila["nombre"].ToString();

                if (MessageBox.Show($"Eliminar la mascota: {nombre}?", "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bd.EliminarMascota(id);
                    CargarMascotas();
                    LimpiarCamposMascotas();
                    MessageBox.Show("Mascota eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar mascota: {ex.Message}");
            }
        }

        private void btnagregarveh_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeAgregar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para agregar vehiculos.");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtplaca.Text))
                {
                    MessageBox.Show("Por favor ingrese la placa del vehiculo.");
                    return;
                }

                if (!int.TryParse(txtyear.Text, out int year))
                {
                    MessageBox.Show("Por favor ingrese un año valido.");
                    return;
                }

                DateOnly dekra = fechadekra.SelectedDate.HasValue ?
                         DateOnly.FromDateTime(fechadekra.SelectedDate.Value) :
                         DateOnly.FromDateTime(DateTime.Now);

                bd.InsertarVehiculo(txtplaca.Text.Trim(), txtmarca.Text.Trim(), txtmodelo.Text.Trim(),
                                   year, txtpoliza.Text.Trim(), dekra);
                CargarVehiculo();
                LimpiarCamposVehiculos();
                MessageBox.Show("Vehiculo agregado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar vehiculo: {ex.Message}");
            }

        }

        private void btnmodificarveh_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEditar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para modificar vehiculos.");
                return;
            }

            if (dbvehiculo.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un vehiculo para modificar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dbvehiculo.SelectedItem;
                int id = (int)fila["id_vehiculo"];

                if (!int.TryParse(txtyear.Text, out int year))
                {
                    MessageBox.Show("Por favor ingrese un año valido.");
                    return;
                }

                DateOnly dekra = fechadekra.SelectedDate.HasValue ?
                        DateOnly.FromDateTime(fechadekra.SelectedDate.Value) :
                        DateOnly.FromDateTime(DateTime.Now);

                bd.ActualizarVehiculo(id, txtplaca.Text.Trim(), txtmarca.Text.Trim(), txtmodelo.Text.Trim(),
                                     year, txtpoliza.Text.Trim(), dekra);
                CargarVehiculo();
                LimpiarCamposVehiculos();
                MessageBox.Show("Vehiculo modificado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar vehiculo: {ex.Message}");
            }
        }

        private void btneliminarveh_Click(object sender, RoutedEventArgs e)
        {
            if (!Permisos.PuedeEliminar(miembroActual.Rol))
            {
                MessageBox.Show("No tienes permisos para eliminar vehiculos.");
                return;
            }

            if (dbvehiculo.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un vehiculo para eliminar.");
                return;
            }

            try
            {
                DataRowView fila = (DataRowView)dbvehiculo.SelectedItem;
                int id = (int)fila["id_vehiculo"];
                string placa = fila["placa"].ToString();

                if (MessageBox.Show($"Eliminar el vehiculo con placa: {placa}?", "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bd.EliminarVehiculo(id);
                    CargarVehiculo();
                    LimpiarCamposVehiculos();
                    MessageBox.Show("Vehiculo eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar vehiculo: {ex.Message}");
            }
        }
    }
}
