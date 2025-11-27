using Clases;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para PantallaPrincipal.xaml
    /// </summary>
    public partial class PantallaPrincipal : Window
    {
        private Button botonSeleccionado = null;
        private Dictionary<string, string> imagenesPredefinidas;
        private List<Button> botones;
        private BaseDatos baseDatos = new BaseDatos();


        public PantallaPrincipal()
        {
            InitializeComponent();
            InicializarImagenesPredefinidas();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            botones = new List<Button> { btn1, btn2, btn3, btn4, btn5, btn6 };

            foreach (var boton in botones)
            {
                boton.Click += BotonSeleccionado_Click;
            }

            CargarAsignacionesGuardadas();

        }

        private int ObtenerNumeroBoton(Button boton)
        {
            return botones.IndexOf(boton) + 1;
        }
        private void AsignarMiembroYImagen(Button boton, Miembros miembro, string rutaImagen, string nombreImagen, bool guardarEnBD)
        {
            try
            {
                // Limpiar el contenido anterior primero
                boton.Content = null;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(rutaImagen, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                Image imagenRedonda = new Image { Source = bitmap, Width = 60, Height = 60 };
                double radio = imagenRedonda.Width / 2;
                imagenRedonda.Clip = new EllipseGeometry(new System.Windows.Point(radio, radio), radio, radio);

                StackPanel panel = new StackPanel();
                panel.Children.Add(imagenRedonda);

                TextBlock txtRol = new TextBlock
                {
                    Text = miembro.Rol.ToUpper(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 10,
                    FontWeight = FontWeights.Bold
                };
                panel.Children.Add(txtRol);

                boton.Content = panel; 
                boton.ToolTip = $"{miembro.Nombre} ({miembro.Rol})";

                // Guardar el objeto miembro completo en el Tag
                boton.Tag = miembro;

                // Color segun rol
                switch (miembro.Rol.ToUpper()) 
                {
                    case "ADMIN":
                        boton.Background = System.Windows.Media.Brushes.LightBlue;
                        txtRol.Foreground = System.Windows.Media.Brushes.DarkBlue;
                        break;
                    case "EDITOR":
                        boton.Background = System.Windows.Media.Brushes.LightGreen;
                        txtRol.Foreground = System.Windows.Media.Brushes.DarkGreen;
                        break;
                    case "LECTOR":
                        boton.Background = System.Windows.Media.Brushes.LightGray;
                        txtRol.Foreground = System.Windows.Media.Brushes.DarkGray;
                        break;
                    default:
                        boton.Background = System.Windows.Media.Brushes.White;
                        break;
                }

                // Guardar 
                if (guardarEnBD)
                {
                    int numeroBoton = ObtenerNumeroBoton(boton);
                    baseDatos.GuardarAsignacionBoton(miembro.ID_Miembros, numeroBoton, rutaImagen, nombreImagen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar imagen: {ex.Message}");
            }
        }

        private void CargarAsignacionesGuardadas()
        {
            try
            {

                var asignaciones = baseDatos.CargarAsignacionesBotones();

                foreach (var asignacion in asignaciones)
                {

                    if (asignacion.NumeroBoton >= 1 && asignacion.NumeroBoton <= botones.Count)
                    {
                        Button boton = botones[asignacion.NumeroBoton - 1];

                        // Verificar que la imagen exista
                        if (File.Exists(asignacion.RutaImagen) ||
                            File.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), asignacion.RutaImagen)))
                        {
                            AsignarMiembroYImagen(boton, asignacion.Miembro, asignacion.RutaImagen, asignacion.NombreImagen, false);
                        }
                        else
                        {
                            MessageBox.Show($"No se encuentra la imagen: {asignacion.RutaImagen}");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaciones guardadas: {ex.Message}\n\nStackTrace: {ex.StackTrace}");
            }
        }
        private void InicializarImagenesPredefinidas()
        {
            imagenesPredefinidas = new Dictionary<string, string>
        {
            {"Conejo", "Imagenes/conejo.jfif"},
            {"Elefante", "Imagenes/elefante.jfif"},
            {"Gallina", "Imagenes/gallina.jfif"},
            {"Pinguino", "Imagenes/pinguino.jfif"},
            {"Tiburon", "Imagenes/tiburon.jfif"},
            {"Zorro", "Imagenes/zorro.jfif"}
            };
        }

        private void btneditar_Click(object sender, RoutedEventArgs e)
        {

            MostrarSelectorImagenes();
        }

        private Button ObtenerPrimerBotonLibre()
        {
            foreach (var btn in botones)
            {
                if (btn.Tag == null) // si no tiene miembro asignado
                    return btn;
            }
            return null;
        }

        private void BotonSeleccionado_Click(object sender, RoutedEventArgs e)
        {
            botonSeleccionado = sender as Button;

            if (botonSeleccionado.Tag == null)
            {
                MessageBox.Show("Este botón no tiene un usuario asignado. Primero edita para asignar un usuario.");
                return;
            }

            foreach (Button btn in botones)
            {
                if (btn == botonSeleccionado)
                {
                    btn.BorderBrush = Brushes.Yellow;
                    btn.BorderThickness = new Thickness(3);
                }
                else
                {
                    btn.BorderBrush = Brushes.Transparent;
                    btn.BorderThickness = new Thickness(0);
                }
            }

            AbrirMenuGeneral();

        }
        private void AbrirMenuGeneral()
        {
            if (botonSeleccionado?.Tag != null)
            {
                Miembros miembroSeleccionado = botonSeleccionado.Tag as Miembros;
                MenuPrincipal menuGeneral = new MenuPrincipal(miembroSeleccionado);
                menuGeneral.Owner = this;
                menuGeneral.Show();
                this.Hide();
            }
        }
        private void MostrarSelectorImagenes()
        {
            Window selector = new Window
            {
                Title = "Seleccionar Miembro",
                Width = 350,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            StackPanel panel = new StackPanel();

            // Título
            TextBlock titulo = new TextBlock
            {
                Text = "Primero selecciona un miembro:",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            panel.Children.Add(titulo);

            // ComboBox miembros
            ComboBox cmbMiembros = new ComboBox
            {
                Margin = new Thickness(10),
                Height = 30,
                DisplayMemberPath = "Nombre"
            };

            try
            {
                BaseDatos db = new BaseDatos();
                var miembros = db.LlenarComboConMiembros();
                cmbMiembros.ItemsSource = miembros;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar miembros: {ex.Message}");
            }

            panel.Children.Add(cmbMiembros);

            // Panel info miembro
            StackPanel panelInfo = new StackPanel
            {
                Margin = new Thickness(10),
                Visibility = Visibility.Collapsed
            };
            TextBlock txtInfo = new TextBlock { FontSize = 12, FontWeight = FontWeights.Bold };
            panelInfo.Children.Add(txtInfo);
            panel.Children.Add(panelInfo);

            cmbMiembros.SelectionChanged += (s, args) =>
            {
                if (cmbMiembros.SelectedItem is Miembros miembroSeleccionado)
                {
                    txtInfo.Text = $"Rol: {miembroSeleccionado.Rol}";
                    panelInfo.Visibility = Visibility.Visible;
                }
            };

            // Separador
            Rectangle separador = new Rectangle
            {
                Height = 1,
                Fill = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(10, 20, 10, 10)
            };
            panel.Children.Add(separador);

            TextBlock tituloImagenes = new TextBlock
            {
                Text = "Ahora elige una imagen:",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10, 10, 10, 5),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            panel.Children.Add(tituloImagenes);


            WrapPanel wrapImagenes = new WrapPanel
            {
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            foreach (var imagen in imagenesPredefinidas)
            {
                Button btnImagen = new Button
                {
                    Content = CrearBotonImagen(imagen.Key, imagen.Value),
                    Margin = new Thickness(5),
                    Width = 80,
                    Height = 80,
                    Background = System.Windows.Media.Brushes.LightGray,

                    Tag = new { Ruta = imagen.Value, Nombre = imagen.Key }
                };

                btnImagen.Click += (s, args) =>
                {
                    if (cmbMiembros.SelectedItem == null)
                    {
                        MessageBox.Show("Primero selecciona un miembro");
                        return;
                    }

                    Miembros miembro = cmbMiembros.SelectedItem as Miembros;


                    dynamic tagInfo = (s as Button).Tag;
                    string rutaImagen = tagInfo.Ruta;    
                    string nombreImagen = tagInfo.Nombre;

                    Button botonLibre = ObtenerPrimerBotonLibre();
                    if (botonLibre == null)
                    {
                        MessageBox.Show("No hay más botones libres.");
                        return;
                    }

                    AsignarMiembroYImagen(botonLibre, miembro, rutaImagen, nombreImagen, true);
                    selector.Close();
                };

                wrapImagenes.Children.Add(btnImagen);
            }

            panel.Children.Add(wrapImagenes);

            selector.Content = panel;
            selector.Owner = this;
            selector.ShowDialog();
        }

        private Image CrearBotonImagen(string nombre, string rutaImagen)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(rutaImagen, UriKind.Relative);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                Image img = new Image
                {
                    Source = bitmap,
                    Width = 60,
                    Height = 60
                };

                // Hacer la imagen redonda
                double radio = img.Width / 2;
                img.Clip = new EllipseGeometry(new Point(radio, radio), radio, radio);

                return img;
            }
            catch
            {
                MessageBox.Show("Error al cargar la imagen: " + nombre);
                return null;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cerrar la aplicacion completamente
            Application.Current.Shutdown();
        }
    }
}
