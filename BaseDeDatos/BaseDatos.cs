using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Clases
{
    public class BaseDatos
    {
        DataTable Miembros;
        DataTable Areas;
        DataTable Categorias_Finanzas;
        DataTable Proveedores;
        DataTable Listas;
        DataTable Tareas;
        DataTable Asignacion_Tareas;
        DataTable Eventos;
        DataTable Facturas;
        DataTable Presupeusto;
        DataTable Salarios;
        DataTable Movimiento;
        DataTable Cultivos;
        DataTable Mascotas;
        DataTable Vehiculos;
        DataTable Siembra;
        DataTable Tratamiento;
        DataTable InventarioJardin;
        DataTable Veterinaria;
        DataTable Medicamentos;
        DataTable Salud;
        DataTable Mantenimiento;
        DataTable GastoMensualTabla;

        DataSet dsTablas = new DataSet();
        SqlConnection _conexion;

        string _StrConexion = @"Server=localhost;Database=domus_hogar;Integrated Security=true;TrustServerCertificate=true;";

        public DataTable TablaMiembros { get => Miembros; set => Miembros = value; }
        public DataTable TablaAreas { get => Areas; set => Areas = value; }
        public DataTable TablaCategorias_Finanzas { get => Categorias_Finanzas; set => Categorias_Finanzas = value; }
        public DataTable TablaProveedores { get => Proveedores; set => Proveedores = value; }
        public DataTable TablaListas { get => Listas; set => Listas = value; }
        public DataTable TablaTareas { get => Tareas; set => Tareas = value; }
        public DataTable TablaAsignacion_Tareas { get => Asignacion_Tareas; set => Asignacion_Tareas = value; }
        public DataTable TablaEventos { get => Eventos; set => Eventos = value; }
        public DataTable TablaFacturas { get => Facturas; set => Facturas = value; }
        public DataTable TablaPresupuesto { get => Presupeusto; set => Presupeusto = value; }
        public DataTable TablaSalarios { get => Salarios; set => Salarios = value; }
        public DataTable TablaMovimiento { get => Movimiento; set=> Movimiento = value; }
        public DataTable TablaCultivos { get => Cultivos; set => Cultivos = value; }
        public DataTable TablaMascotas { get => Mascotas; set => Mascotas = value; }
        public DataTable TablaVehiculos { get => Vehiculos; set => Vehiculos = value; }
        public DataTable TablaSiembra { get => Siembra; set => Siembra = value; }
        public DataTable TablaTratamiento { get => Tratamiento; set => Tratamiento = value; }
        public DataTable TablaInventarioJardin { get => InventarioJardin; set => InventarioJardin = value; }
        public DataTable TablaVeterinaria { get => Veterinaria; set => Veterinaria = value; }
        public DataTable TablaMedicamentos { get => Medicamentos; set => Medicamentos = value; }
        public DataTable TablaSalud { get => Salud; set => Salud = value; }
        public DataTable TablaMantenimiento { get => Mantenimiento; set => Mantenimiento = value; }
        public DataTable TablaGastoMensual { get => GastoMensualTabla; set => GastoMensualTabla = value; }

        private void EstablecerConexion()
        {
            _conexion = new SqlConnection(_StrConexion);
            _conexion.Open();
        }

        //MOSTRAR DATOS COMBOBOX 

        public ObservableCollection<Miembros> LlenarComboConMiembros()
        {
            ObservableCollection<Miembros> miembros = new ObservableCollection<Miembros>();

            using (SqlConnection connection = new SqlConnection(_StrConexion))
            {
                string query = "SELECT id_miembro, nombre, rol FROM miembros";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        miembros.Add(new Miembros
                        {
                            ID_Miembros = reader.GetInt32(reader.GetOrdinal("id_miembro")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Rol = reader.GetString(reader.GetOrdinal("rol"))
                        });
                    }
                }
            }
            return miembros;
        }
        public ObservableCollection<Miembros> LlenarComboConMiem()
        {
            var miembros = new ObservableCollection<Miembros>();

            try
            {
                using (var con = new SqlConnection(_StrConexion)) 
                {
                    con.Open();
                    string query = "SELECT id_miembro, nombre FROM miembros ORDER BY nombre";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            miembros.Add(new Miembros()
                            {
                                ID_Miembros = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar miembros: {ex.Message}");
            }

            return miembros;
        }
        public ObservableCollection<Areas> LlenarComboConAreas()
        {
            var areas = new ObservableCollection<Areas>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_area, nombre FROM areas ORDER BY nombre";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            areas.Add(new Areas()
                            {
                                ID_Area = reader.GetInt32(0),
                                Nombre_Area = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar areas: {ex.Message}");
            }

            return areas;
        }
        public ObservableCollection<Listas> LlenarComboConListas()
        {
            var listas = new ObservableCollection<Listas>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_lista, nombre FROM listas ORDER BY nombre";
  

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listas.Add(new Listas()
                            {
                                ID_Listas = reader.GetInt32(0),
                                Nombre_Lista = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar listas: {ex.Message}");
            }

            return listas;
        }
        public ObservableCollection<Tareas> LlenarComboConTareas()
        {
            var tareas = new ObservableCollection<Tareas>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_tarea, titulo FROM tareas ORDER BY titulo";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tareas.Add(new Tareas()
                            {
                                ID_Tareas = reader.GetInt32(0),
                                Titulo1 = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar tareas: {ex.Message}");
            }

            return tareas;
        }
        public ObservableCollection<Proveedores> LlenarComboConProveedores()
        {
            var proveedores = new ObservableCollection<Proveedores>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_proveedor, nombre FROM proveedores ORDER BY nombre";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedores.Add(new Proveedores()
                            {
                                ID_Proveedor = reader.GetInt32(0),
                                Nombre_Empresa = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar proveedores: {ex.Message}");
            }

            return proveedores;
        }
        public ObservableCollection<Categorias_Finanzas> LlenarComboCategoriasFinanzas()
        {
            var categorias = new ObservableCollection<Categorias_Finanzas>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_categoria, nombre FROM categorias_finanzas ORDER BY nombre";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorias.Add(new Categorias_Finanzas()
                            {
                                ID_categoria = reader.GetInt32(0),
                                Nombre_categoria = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar categorias finanzas: {ex.Message}");
            }

            return categorias;
        }
        public ObservableCollection<Cultivos> LlenarComboCultivos()
        {
            var cultivos = new ObservableCollection<Cultivos>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_cultivo, nombre FROM cultivos ORDER BY nombre";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cultivos.Add(new Cultivos()
                            {
                                ID_cultivo = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar cultivos: {ex.Message}");
            }

            return cultivos;
        }
        public ObservableCollection<Siembras> LlenarComboSimebras()
        {
            var siembras = new ObservableCollection<Siembras>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_siembra, sector FROM siembras ORDER BY sector";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            siembras.Add(new Siembras()
                            {
                                ID_siembra = reader.GetInt32(0),
                                Sector = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar siembra: {ex.Message}");
            }

            return siembras;
        }
        public ObservableCollection<Mascotas> LlenarComboMascotas()
        {
            var mascota = new ObservableCollection<Mascotas>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_mascota, nombre FROM mascotas ORDER BY nombre";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mascota.Add(new Mascotas()
                            {
                                ID_mascota = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar mascota: {ex.Message}");
            }

            return mascota;
        }
        public ObservableCollection<Vehiculo> LlenarComboVehiculo()
        {
            var vehiculo = new ObservableCollection<Vehiculo>();

            try
            {
                using (var con = new SqlConnection(_StrConexion))
                {
                    con.Open();
                    string query = "SELECT id_vehiculo, placa FROM vehiculos ORDER BY placa";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculo.Add(new Vehiculo()
                            {
                                ID_vehiculo = reader.GetInt32(0),
                                Placa = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error al cargar vehiculo: {ex.Message}");
            }

            return vehiculo;
        }

        public void GuardarAsignacionBoton(int idMiembro, int numeroBoton, string rutaImagen, string nombreImagen)
        {
            using (SqlConnection connection = new SqlConnection(_StrConexion))
            {
                SqlCommand command = new SqlCommand("spGuardarAsignacionBoton", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@IdMiembro", idMiembro);
                command.Parameters.AddWithValue("@NumeroBoton", numeroBoton);
                command.Parameters.AddWithValue("@RutaImagen", rutaImagen);
                command.Parameters.AddWithValue("@NombreImagen", nombreImagen);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<AsignacionBoton> CargarAsignacionesBotones()
        {
            List<AsignacionBoton> asignaciones = new List<AsignacionBoton>();

            using (SqlConnection connection = new SqlConnection(_StrConexion))
            {
                // USAR LOS NOMBRES EXACTOS DE LAS COLUMNAS - id_miembro con guión bajo
                string query = @"SELECT ab.NumeroBoton, ab.RutaImagen, ab.NombreImagen, 
                                ab.IdMiembro, m.id_miembro, m.nombre, m.rol 
                         FROM AsignacionesBotones ab
                         INNER JOIN miembros m ON ab.IdMiembro = m.id_miembro";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        asignaciones.Add(new AsignacionBoton
                        {
                            IdMiembro = reader.GetInt32(reader.GetOrdinal("IdMiembro")),
                            NumeroBoton = reader.GetInt32(reader.GetOrdinal("NumeroBoton")),
                            RutaImagen = reader.GetString(reader.GetOrdinal("RutaImagen")),
                            NombreImagen = reader.GetString(reader.GetOrdinal("NombreImagen")),
                            Miembro = new Miembros
                            {
                                ID_Miembros = reader.GetInt32(reader.GetOrdinal("id_miembro")),  
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Rol = reader.GetString(reader.GetOrdinal("rol"))
                            }
                        });
                    }
                }
            }
            return asignaciones;
        }

        //INSERTAR DATOS A LA BASE DE DATOS
        public void InsertarAreas(string nombre_area, string detalle)
        {

            SqlCommand _instruccionSQL;

            try
            {
                EstablecerConexion();

                _instruccionSQL = new SqlCommand("spInsertarArea", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre_area);
                _instruccionSQL.Parameters.AddWithValue("@detalle", detalle);

                _instruccionSQL.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el area: " + ex.Message);
            }
            finally
            {

                _conexion.Close();
            }

        }
        public void InsertarCategoriasFinanzas(string nombre_categoria, string tipo)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarCategoriaFinanzas", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre_categoria);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la categoria de finanzas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarProveedores(string nombre_proveedor, string tipo)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarProveedor", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre_proveedor);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el proveedor: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarCultivo(string nombre, string variedad)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarCultivo", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre);
                _instruccionSQL.Parameters.AddWithValue("@variedad", variedad);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el cultivo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarMascota(string nombre, string especie, string raza, DateOnly fecha_nac, decimal peso)
        {
            SqlCommand _instruccionSQL;

            try 
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarMascota", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre);
                _instruccionSQL.Parameters.AddWithValue("@especie", especie);
                _instruccionSQL.Parameters.AddWithValue("@raza", raza ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@fecha_nac", fecha_nac);
                _instruccionSQL.Parameters.AddWithValue("@peso", peso);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la mascota: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void InsertarVehiculo(string placa, string marca, string modelo, int year, string poliza, DateOnly dekra)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarVehiculo", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@placa", placa);
                _instruccionSQL.Parameters.AddWithValue("@marca", marca);
                _instruccionSQL.Parameters.AddWithValue("@modelo", modelo);
                _instruccionSQL.Parameters.AddWithValue("@year", year);
                _instruccionSQL.Parameters.AddWithValue("@poliza", poliza ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@dekra", dekra);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarLista(string nombre, string tipo, int? id_area, int? creada_por, DateOnly fecha_creada)
        {
            SqlCommand _instruccionSQL;

            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarLista", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@id_area", id_area ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@creada_por", creada_por ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@fecha_creada", fecha_creada.ToDateTime(TimeOnly.MinValue));

                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar lista: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarTarea(int id_lista, string titulo, string descripcion, string prioridad, string estado, DateOnly fecha_creacion, DateOnly? fecha_limite, string repeticion, int? id_area)
        {
            SqlCommand _instruccionSQL;

            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarTarea", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _instruccionSQL.Parameters.AddWithValue("@id_lista", id_lista);
                _instruccionSQL.Parameters.AddWithValue("@titulo", titulo);
                _instruccionSQL.Parameters.AddWithValue("@descripcion", descripcion ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@prioridad", prioridad);
                _instruccionSQL.Parameters.AddWithValue("@estado", estado);
                _instruccionSQL.Parameters.AddWithValue("@fecha_creacion", fecha_creacion.ToDateTime(TimeOnly.MinValue));
                _instruccionSQL.Parameters.AddWithValue("@fecha_limite", fecha_limite?.ToDateTime(TimeOnly.MinValue) ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@repeticion", repeticion);
                _instruccionSQL.Parameters.AddWithValue("@id_area", id_area ?? (object)DBNull.Value);

                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar tarea: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsetarAsginarTarea(int idTarea, int idMiembro) {
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarAsignacionTarea", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@id_tarea", idTarea);
                _instruccionSQL.Parameters.AddWithValue("@id_miembro", idMiembro);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al asignar tarea: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarEvento(string tipo, string titulo, DateTime fecha, string lugar, string notas, int idMiembro) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarEvento", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@titulo", titulo);
                _instruccionSQL.Parameters.AddWithValue("@fecha", fecha);
                _instruccionSQL.Parameters.AddWithValue("@lugar", lugar ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@notas", notas ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@id_miembro", idMiembro);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar evento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarFactura(int idProveedor, decimal monto, int idCategoria, DateTime fechaEmision, DateTime fechaVencimiento, string estado) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarFactura", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@id_proveedor", idProveedor);
                _instruccionSQL.Parameters.AddWithValue("@monto", monto);
                _instruccionSQL.Parameters.AddWithValue("@id_categoria", idCategoria);
                _instruccionSQL.Parameters.AddWithValue("@fecha_emision", fechaEmision);
                _instruccionSQL.Parameters.AddWithValue("@fecha_vencimiento", fechaVencimiento);
                _instruccionSQL.Parameters.AddWithValue("@estado", estado);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar factura: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarPresupuesto(int anio, string mes, int idCategoria, decimal montoPlaneado, decimal montoEjecutado) {

            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarPresupuesto", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@anio", anio);
                _instruccionSQL.Parameters.AddWithValue("@mes", mes);
                _instruccionSQL.Parameters.AddWithValue("@id_categoria", idCategoria);
                _instruccionSQL.Parameters.AddWithValue("@monto_planeado", montoPlaneado);
                _instruccionSQL.Parameters.AddWithValue("@monto_ejecutado", montoEjecutado);
                _instruccionSQL.ExecuteNonQuery();

            }
            catch (Exception ex) { 
            
                throw new Exception("Error al insertar presupuesto: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarSalario(int idMiembro, decimal monto, string periodicidad, decimal deducciones, DateTime fechaInicio) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarSalario", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@id_miembro", idMiembro);
                _instruccionSQL.Parameters.AddWithValue("@monto", monto);
                _instruccionSQL.Parameters.AddWithValue("@periodicidad", periodicidad);
                _instruccionSQL.Parameters.AddWithValue("@deducciones", deducciones);
                _instruccionSQL.Parameters.AddWithValue("@fecha_inicio", fechaInicio);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar salario: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarMovimiento(DateTime fecha, string tipo, int idCategoria, decimal monto, string referencia) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarMovimiento", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@fecha", fecha);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@id_categoria", idCategoria);
                _instruccionSQL.Parameters.AddWithValue("@monto", monto);
                _instruccionSQL.Parameters.AddWithValue("@referencia", referencia ?? (object)DBNull.Value);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar movimiento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarSiembra(int idCultivo, DateTime fechaSiembra, DateTime? fechaEstimada, string sector, string notas) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarSiembra", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@id_cultivo", idCultivo);
                _instruccionSQL.Parameters.AddWithValue("@fecha_siembra", fechaSiembra);
                _instruccionSQL.Parameters.AddWithValue("@fecha_estimada", fechaEstimada ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@sector", sector);
                _instruccionSQL.Parameters.AddWithValue("@notas", notas ?? (object)DBNull.Value);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar siembra: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarTratamiento(int idSiembra, DateTime fecha, string producto, string dosis, string notas) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarTratamiento", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@id_siembra", idSiembra);
                _instruccionSQL.Parameters.AddWithValue("@fecha", fecha);
                _instruccionSQL.Parameters.AddWithValue("@producto", producto);
                _instruccionSQL.Parameters.AddWithValue("@dosis", dosis);
                _instruccionSQL.Parameters.AddWithValue("@notas", notas ?? (object)DBNull.Value);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar tratamiento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarInventario(string nombre, string tipo, decimal cantidad, string unidad) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarInventarioJardin", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@cantidad", cantidad);
                _instruccionSQL.Parameters.AddWithValue("@unidad", unidad);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar inventario: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void InsertarMantenimientoVehiculo(int idVehiculo, string tipo, string concepto,DateTime fecha, int? kilometraje, decimal costo,string taller, string notas)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spInsertarMantenimientoVehiculo", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@id_vehiculo", idVehiculo);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@concepto", concepto);
                _instruccionSQL.Parameters.AddWithValue("@fecha", fecha);
                _instruccionSQL.Parameters.AddWithValue("@kilometraje", kilometraje ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@costo", costo);
                _instruccionSQL.Parameters.AddWithValue("@taller", taller ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@notas", notas ?? (object)DBNull.Value);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar mantenimiento de vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
       
        //ACTUALIZAR DATOS EN LA BASE DE DATOS

        public void ActualizarArea(int id_area, string nombre_area, string detalle)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarArea", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_area", id_area);
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre_area);
                _instruccionSQL.Parameters.AddWithValue("@detalle", detalle);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el area: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        } 
        public void ActualizarCategoriaFinanzas(int id_categoria, string nombre_categoria,  string tipo)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarCategoriaFinanzas", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_categoria", id_categoria);
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre_categoria);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la categoria de finanzas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarProveedor(int id_proveedor, string nombre_proveedor, string tipo)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarProveedor", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_proveedor", id_proveedor);
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre_proveedor);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el proveedor: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarCultivo(int id_cultivo, string nombre, string variedad)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarCultivo", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_cultivo", id_cultivo);
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre);
                _instruccionSQL.Parameters.AddWithValue("@variedad", variedad);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el cultivo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarMascota(int id_mascota, string nombre, string especie, string raza, DateOnly fecha_nac, decimal peso)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarMascota", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_mascota", id_mascota);
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre);
                _instruccionSQL.Parameters.AddWithValue("@especie", especie);
                _instruccionSQL.Parameters.AddWithValue("@raza", raza ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@fecha_nac", fecha_nac);
                _instruccionSQL.Parameters.AddWithValue("@peso", peso);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la mascota: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarVehiculo(int id_vehiculo, string placa, string marca, string modelo, int year, string poliza, DateOnly dekra)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarVehiculo", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_vehiculo", id_vehiculo);
                _instruccionSQL.Parameters.AddWithValue("@placa", placa);
                _instruccionSQL.Parameters.AddWithValue("@marca", marca);
                _instruccionSQL.Parameters.AddWithValue("@modelo", modelo);
                _instruccionSQL.Parameters.AddWithValue("@year", year);
                _instruccionSQL.Parameters.AddWithValue("@poliza", poliza ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@dekra", dekra);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarLista(int id_lista, string nombre, string tipo, int? id_area, int? creada_por, DateOnly fecha_creada)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarLista", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _instruccionSQL.Parameters.AddWithValue("@id_lista", id_lista);
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@id_area", id_area ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@creada_por", creada_por ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@fecha_creada", fecha_creada.ToDateTime(TimeOnly.MinValue));
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la lista: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarTarea(int id_tarea, int nuevo_id_lista, string nuevo_titulo, string nueva_descripcion, string nueva_prioridad, string nuevo_estado, DateOnly nueva_fecha_creacion, DateOnly? nueva_fecha_limite, string nueva_repeticion, int? nuevo_id_area)
        {
            SqlCommand _instruccionSQL;

            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarTarea", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _instruccionSQL.Parameters.AddWithValue("@id_lista", nuevo_id_lista);
                _instruccionSQL.Parameters.AddWithValue("@titulo", nuevo_titulo);
                _instruccionSQL.Parameters.AddWithValue("@descripcion", nueva_descripcion ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@prioridad", nueva_prioridad);
                _instruccionSQL.Parameters.AddWithValue("@estado", nuevo_estado);
                _instruccionSQL.Parameters.AddWithValue("@fecha_creacion", nueva_fecha_creacion.ToDateTime(TimeOnly.MinValue));
                _instruccionSQL.Parameters.AddWithValue("@fecha_limite", nueva_fecha_limite?.ToDateTime(TimeOnly.MinValue) ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@repeticion", nueva_repeticion);
                _instruccionSQL.Parameters.AddWithValue("@id_area", nuevo_id_area ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@id_tarea", id_tarea);

                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar tarea: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarAsignarTarea(int idTarea, int nuevoMiembro) {
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarAsignacionTarea", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_tarea", idTarea);
                _instruccionSQL.Parameters.AddWithValue("@id_miembro", nuevoMiembro);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la asignacion de tarea: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarEvento(int id_evento, string tipo, string titulo, DateTime fecha, string lugar, string notas, int idMiembro)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarEvento", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_evento", id_evento);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@titulo", titulo);
                _instruccionSQL.Parameters.AddWithValue("@fecha", fecha);
                _instruccionSQL.Parameters.AddWithValue("@lugar", lugar ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@notas", notas ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@id_miembro", idMiembro > 0 ? (object)idMiembro : DBNull.Value);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el evento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarFactura(int id_factura, int idProveedor, decimal monto, int idCategoria, DateTime fechaEmision, DateTime fechaVencimiento, string estado)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarFactura", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_factura", id_factura);
                _instruccionSQL.Parameters.AddWithValue("@id_proveedor", idProveedor);
                _instruccionSQL.Parameters.AddWithValue("@monto", monto);
                _instruccionSQL.Parameters.AddWithValue("@id_categoria", idCategoria);
                _instruccionSQL.Parameters.AddWithValue("@fecha_emision", fechaEmision);
                _instruccionSQL.Parameters.AddWithValue("@fecha_vencimiento", fechaVencimiento);
                _instruccionSQL.Parameters.AddWithValue("@estado", estado);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la factura: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarPresupuesto(int id_presupuesto, int anio, string mes, int idCategoria, decimal montoPlaneado, decimal montoEjecutado)
        {
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarPresupuesto", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_presupuesto", id_presupuesto);
                _instruccionSQL.Parameters.AddWithValue("@anio", anio);
                _instruccionSQL.Parameters.AddWithValue("@mes", mes);
                _instruccionSQL.Parameters.AddWithValue("@id_categoria", idCategoria);
                _instruccionSQL.Parameters.AddWithValue("@monto_planeado", montoPlaneado);
                _instruccionSQL.Parameters.AddWithValue("@monto_ejecutado", montoEjecutado);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el presupuesto: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarSalario(int idSalario, int idMiembro, decimal monto, string periocidad, decimal deducciones, DateTime dechaInicio) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarSalario", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_salario", idSalario);
                _instruccionSQL.Parameters.AddWithValue("@id_miembro", idMiembro);
                _instruccionSQL.Parameters.AddWithValue("@monto", monto);
                _instruccionSQL.Parameters.AddWithValue("@periodicidad", periocidad);
                _instruccionSQL.Parameters.AddWithValue("@deducciones", deducciones);
                _instruccionSQL.Parameters.AddWithValue("@fecha_inicio", dechaInicio);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el salario: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarMovimiento(int idmov,DateTime fecha, string tipo, int idCategoria, decimal monto, string referencia) {
        
           SqlCommand sqlCommand;
                try
                {
                 EstablecerConexion();
                 sqlCommand = new SqlCommand("spActualizarMovimiento", _conexion)
                 {
                     CommandType = CommandType.StoredProcedure
                 };
                 //PARAMETROS
                 sqlCommand.Parameters.AddWithValue("@id_mov", idmov);
                 sqlCommand.Parameters.AddWithValue("@fecha", fecha);
                 sqlCommand.Parameters.AddWithValue("@tipo", tipo);
                 sqlCommand.Parameters.AddWithValue("@id_categoria", idCategoria);
                 sqlCommand.Parameters.AddWithValue("@monto", monto);
                 sqlCommand.Parameters.AddWithValue("@referencia", referencia ?? (object)DBNull.Value);
                 sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                 throw new Exception("Error al actualizar el movimiento: " + ex.Message);
                }
                finally
                {
                 _conexion.Close();
            }
        }
        public void ActualizarSiembra(int id_siembra, int idCultivo, DateTime fechaSiembra, DateTime? fechaEstimada, string sector, string notas) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarSiembra", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_siembra", id_siembra);
                _instruccionSQL.Parameters.AddWithValue("@id_cultivo", idCultivo);
                _instruccionSQL.Parameters.AddWithValue("@fecha_siembra", fechaSiembra);
                _instruccionSQL.Parameters.AddWithValue("@fecha_estim_cosecha", fechaEstimada ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@sector", sector);
                _instruccionSQL.Parameters.AddWithValue("@notas", notas ?? (object)DBNull.Value);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la siembra: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarTratamiento(int idTratamiento, int idSiembra, DateTime fecha, string producto, string dosis, string notas) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarTratamiento", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_tratamiento", idTratamiento);
                _instruccionSQL.Parameters.AddWithValue("@id_siembra", idSiembra);
                _instruccionSQL.Parameters.AddWithValue("@fecha", fecha);
                _instruccionSQL.Parameters.AddWithValue("@producto", producto);
                _instruccionSQL.Parameters.AddWithValue("@dosis", dosis);
                _instruccionSQL.Parameters.AddWithValue("@notas", notas ?? (object)DBNull.Value);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el tratamiento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarInventario(int idItem, string nombre, string tipo, decimal cantidad, string unidad) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarInventarioJardin", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_item", idItem);
                _instruccionSQL.Parameters.AddWithValue("@nombre", nombre);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@cantidad", cantidad);
                _instruccionSQL.Parameters.AddWithValue("@unidad", unidad);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el inventario: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void ActualizarMantenimientoVehiculo(int idMantenimiento, int idVehiculo, string tipo, string concepto, DateTime fecha, int? kilometraje, decimal costo, string taller, string notas) { 
        
            SqlCommand _instruccionSQL;
            try
            {
                EstablecerConexion();
                _instruccionSQL = new SqlCommand("spActualizarMantenimientoVehiculo", _conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_mant", idMantenimiento);
                _instruccionSQL.Parameters.AddWithValue("@id_vehiculo", idVehiculo);
                _instruccionSQL.Parameters.AddWithValue("@tipo", tipo);
                _instruccionSQL.Parameters.AddWithValue("@concepto", concepto);
                _instruccionSQL.Parameters.AddWithValue("@fecha", fecha);
                _instruccionSQL.Parameters.AddWithValue("@kilometraje", kilometraje ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@costo", costo);
                _instruccionSQL.Parameters.AddWithValue("@taller", taller ?? (object)DBNull.Value);
                _instruccionSQL.Parameters.AddWithValue("@notas", notas ?? (object)DBNull.Value);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el mantenimiento de vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        //ELIMINAR DATOS DE LA BASE DE DATOS
        public void EliminarArea(int id_area)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM areas WHERE id_area = @id_area";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_area", id_area);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el area: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarCategoriaFinanzas(int id_categoria)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM categorias_finanzas WHERE id_categoria = @id_categoria";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_categoria", id_categoria);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la categoria de finanzas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarProveedor(int id_proveedor)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM proveedores WHERE id_proveedor = @id_proveedor";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_proveedor", id_proveedor);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el proveedor: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarCultivo(int id_cultivo)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM cultivos WHERE id_cultivo = @id_cultivo";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_cultivo", id_cultivo);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el cultivo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarMascota(int id_mascota)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM mascotas WHERE id_mascota = @id_mascota";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_mascota", id_mascota);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la mascota: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarVehiculo(int id_vehiculo)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM vehiculos WHERE id_vehiculo = @id_vehiculo";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_vehiculo", id_vehiculo);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarLista(int id_lista)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM listas WHERE id_lista = @id_lista";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_lista", id_lista);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la lista: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarTarea(int id_tarea)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM tareas WHERE id_tarea = @id_tarea";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_tarea", id_tarea);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la tarea: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarAsignarTarea(int id_tarea, int idMiembro)
        {
            
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM tareas_asignaciones WHERE id_tarea = @id_tarea AND id_miembro = @id_miembro";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_tarea", id_tarea);
                _instruccionSQL.Parameters.AddWithValue("@id_miembro", idMiembro);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la asignacion de tarea: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarEvento(int id_evento)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM eventos WHERE id_evento = @id_evento";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_evento", id_evento);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el evento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarFactura(int id_factura)
        {
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM facturas WHERE id_factura = @id_factura";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_factura", id_factura);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la factura: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarPresupuesto(int idPresupuesto) { 
        
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM presupuestos WHERE id_presupuesto = @id_presupuesto";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_presupuesto", idPresupuesto);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el presupuesto: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarSalario(int idSalario) { 
        
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM salarios WHERE id_salario = @id_salario";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_salario", idSalario);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el salario: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarMovimiento(int idmov) { 
        
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM movimientos WHERE id_mov = @idmov";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@idmov", idmov);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el movimiento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarSiembra(int id_siembra) { 
        
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM siembras WHERE id_siembra = @id_siembra";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_siembra", id_siembra);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la siembra: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarTratamiento(int idTratamiento) { 
        
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM tratamientos WHERE id_tratamiento = @id_tratamiento";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_tratamiento", idTratamiento);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el tratamiento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarInventario(int idItem) { 
        
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM inventario_jardin WHERE id_item = @id_item";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_item", idItem);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el item de inventario: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void EliminarMantenimientoVehiculo(int idMantenimiento) { 
        
            SqlCommand _instruccionSQL;
            string _eliminacion;
            try
            {
                EstablecerConexion();
                _eliminacion = "DELETE FROM vehiculos_mantenimientos WHERE id_mant = @id_mant";
                _instruccionSQL = new SqlCommand(_eliminacion, _conexion);
                //PARAMETROS
                _instruccionSQL.Parameters.AddWithValue("@id_mant", idMantenimiento);
                _instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el mantenimiento de vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        //MOSTRAR DATOS DE LA BASE DE DATOS 

        public void MostrarAreas()
        {

            SqlCommand sql_instruccion;
            string instruccion = "SELECT * FROM areas";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "areas");
                TablaAreas = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar areas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarFinanzas()
        {

            SqlCommand sql_instruccion;
            string instruccion = "SELECT * FROM categorias_finanzas";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "categorias_finanzas");
                TablaCategorias_Finanzas = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar categoria finanzas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarProveedores()
        {

            SqlCommand sql_instruccion;
            string instruccion = "SELECT * FROM proveedores";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "proveedores");
                TablaProveedores = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar categoria proveedores: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarListas()
        {

            SqlCommand sql_instruccion;
            string instruccion = "SELECT * FROM listas";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "listas");
                TablaListas = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar categoria listas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarTareas()
        {

            SqlCommand sql_instruccion;
            string instruccion = "SELECT * FROM tareas";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "tareas");
                TablaTareas = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar categoria tareas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarEventos()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT
                                even.id_evento,
                                even.tipo,
                                even.titulo AS nombreevento,
                                even.fecha_hora AS fecha,
                                even.lugar,
                                even.notas AS notas,
                                even.id_miembro,
                                m.nombre AS nombre_miembro
                                FROM eventos even
                                INNER JOIN miembros m ON even.id_miembro = m.id_miembro
                                ORDER BY even.fecha_hora";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "eventos");
                TablaEventos = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion eventos: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarPresupuesto()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT
                                pre.id_presupuesto AS ID,
                                pre.anio AS Año,
                                cat.nombre AS Categoria,
                                pre.mes AS Mes,
                                pre.monto_planeado AS Monto_Planeado,
                                pre.monto_ejecutado AS Monto_Ejecutado
                                FROM presupuestos pre
                                INNER JOIN categorias_finanzas cat ON pre.id_categoria = cat.id_categoria
                                ORDER BY pre.anio";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "facturas");
                TablaPresupuesto = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion presupuesto: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarSalario()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT
                                sa.id_salario,
                                mi.nombre,
                                sa.monto,
                                sa.periodicidad,
                                sa.deducciones,
                                sa.fecha_inicio 
                                FROM salarios sa
                                INNER JOIN miembros mi ON sa.id_miembro = mi.id_miembro
                                ORDER BY sa.monto";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "salarios");
                TablaSalarios = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion salarios: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarMovimientos()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT
                                mov.id_mov AS id_movimiento,
                                mov.fecha,
                                mov.tipo,
                                cat.nombre,
                                mov.monto,
                                mov.referencia
                                FROM movimientos mov
                                INNER JOIN categorias_finanzas cat ON mov.id_categoria = cat.id_categoria
                                ORDER BY mov.monto";
            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "movimientos");
                TablaMovimiento = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion movimientos: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarCultivos()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT *
                                FROM cultivos";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "cultivos");
                TablaCultivos = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion cultivos: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarMascotas()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT *
                                FROM mascotas";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "cultivos");
                TablaMascotas = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion mascotas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarVehiculo()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT *
                                FROM vehiculos";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "cultivos");
                TablaVehiculos = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarSiembras()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT s.id_siembra, c.nombre, s.fecha_siembra,
                                 s.fecha_estim_cosecha,
                                 s.sector,
                                 s.notas
                                 FROM siembras s
                                 INNER JOIN cultivos c ON s.id_cultivo = c.id_cultivo                                     
                                 ORDER BY s.fecha_siembra";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "siembras");
                TablaSiembra = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion siembra: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarTratamiento()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT t.id_tratamiento,
                                 s.sector,
                                 t.fecha,
                                 t.producto,
                                 t.dosis,
                                 t.notas
                                 FROM tratamientos t
                                 INNER JOIN siembras s ON t.id_siembra = s.id_siembra                                     
                                 ORDER BY t.producto";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "tratamientos");
                TablaTratamiento = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion tratamiento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarInventario()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT *
                                 FROM inventario_jardin";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "inventario_jardin");
                TablaInventarioJardin = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion inventario: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarVeterinaria()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT m.nombre, v.fecha,
                                    v.motivo,
                                    v.costo,
                                    v.notas
                                 FROM vet_visitas v 
                                 INNER JOIN mascotas m ON v.id_mascota = m.id_mascota";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "vet_visitas");
                TablaVeterinaria = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion veterinaria: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarMedicamentos()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT m.nombre, med.nombre_med,
                                    med.dosis,
                                    med.frecuencia,
                                    med.fecha_ini,
                                    med.fecha_fin
                                 FROM mascotas_meds med
                                 INNER JOIN mascotas m ON med.id_mascota = m.id_mascota";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "mascotas_meds");
                TablaMedicamentos = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion medicamentos: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }
        public void MostrarSalud()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT m.nombre,
                                    s.fecha,
                                    s.evento,
                                    s.notas
                                 FROM mascotas_salud s
                                 INNER JOIN mascotas m ON s.id_mascota = m.id_mascota";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "mascotas_salud");
                TablaSalud = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion salud: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }

        }

        //OBTENER LOS ID DESDE LAS VISTAS
        public int ObtenerIDEventoDesdeVista(string titulo, DateTime fecha, string tipo)
        {
            try
            {
                EstablecerConexion();
                string query = "SELECT id_evento FROM eventos WHERE titulo = @titulo AND fecha_hora = @fecha AND tipo = @tipo";

                SqlCommand cmd = new SqlCommand(query, _conexion);
                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("No se encontró el ID del evento");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ID del evento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public int ObtenerIDFacturaDesdeVista(string proveedor, decimal monto, DateTime fechaVenc, string categoria)
        {
            try
            {
                EstablecerConexion();
                string query = @"
                SELECT F.id_factura 
                FROM facturas F
                JOIN proveedores P ON F.id_proveedor = P.id_proveedor
                JOIN categorias_finanzas CF ON F.categoria_id = CF.id_categoria
                WHERE P.nombre = @proveedor 
                AND F.monto = @monto 
                AND F.fecha_venc = @fecha_venc 
                AND CF.nombre = @categoria";

                SqlCommand cmd = new SqlCommand(query, _conexion);
                cmd.Parameters.AddWithValue("@proveedor", proveedor);
                cmd.Parameters.AddWithValue("@monto", monto);
                cmd.Parameters.AddWithValue("@fecha_venc", fechaVenc);
                cmd.Parameters.AddWithValue("@categoria", categoria);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("No se encontro el ID de la factura");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ID de factura: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public int ObtenerIDMantenimiento(string placa, string concepto, DateTime fecha, string tipo)
        {
            try
            {
                EstablecerConexion();
                string query = @"SELECT VM.id_mant 
                        FROM vehiculos_mantenimientos VM
                        INNER JOIN vehiculos V ON VM.id_vehiculo = V.id_vehiculo
                        WHERE V.placa = @placa 
                        AND VM.concepto = @concepto 
                        AND VM.fecha = @fecha 
                        AND VM.tipo = @tipo";

                SqlCommand cmd = new SqlCommand(query, _conexion);
                cmd.Parameters.AddWithValue("@placa", placa);
                cmd.Parameters.AddWithValue("@concepto", concepto);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("No se encontro el ID del mantenimiento");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ID del mantenimiento: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }

        //MOSTRAR LAS VISTAS DEL SQL

        public void MostrarEventosDelMes()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                            titulo,
                            tipo,
                            fecha_hora,
                            lugar,
                            responsable
                          FROM EVENTOSDELMES
                          ORDER BY fecha_hora";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "EVENTOSDELMES");
                TablaEventos = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar eventos del mes: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void MostrarResumenFinanciera()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                           Categoria,
                            tipo,
                            [Total Movimientos],
                            [Cantidad de movimientos]
                          FROM RESUMENFINANCIERO";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "RESUMENFINANCIERO");
                TablaMovimiento = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar resumen financiera: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        
        public List<GastoMensual> ObtenerGastoMensual()
        {
            var resultado = new List<GastoMensual>();

            try
            {
                EstablecerConexion();

                using (var cmd = new SqlCommand("Gasto_Mensual", _conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Add(new GastoMensual
                            {
                                Anio = reader.GetInt32(reader.GetOrdinal("anio")),
                                Mes = reader.GetInt32(reader.GetOrdinal("mes")),
                                Egreso = reader.GetDecimal(reader.GetOrdinal("egreso")),
                                EgresoPrev = reader.IsDBNull(reader.GetOrdinal("egresoPrevio")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("egresoPrevio")),
                                EgresoMoM = reader.IsDBNull(reader.GetOrdinal("egresoMoM")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("egresoMoM")),
                                EgresoForecastNaive = reader.IsDBNull(reader.GetOrdinal("egresoPronostico")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("egresoPronostico"))
                            });
                        }
                    }
                }
            }
            finally
            {
                _conexion.Close();
            }

            return resultado;
        }
        public void MostrarTareasPendientes()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                            Miembro,
                            Tarea,
                            Lista,
                            prioridad,
                            fecha_limite,
                            area
                          FROM TAREASPENDIENTES
                          ORDER BY fecha_limite";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "TAREASPENDIENTES");
                TablaAsignacion_Tareas = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar tareas pendientes: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void MostrarFacturasAVencer()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                           Proveedor,
                            monto,
                            fecha_venc,
                            Categoria,
                            [Dias para vencer],
                            [Estado Pago]
                          FROM FACTURASAVENCER
                          ORDER BY fecha_venc";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "FACTURASAVENCER");
                TablaFacturas = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar facturas a vencer: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void MostrarTotalFacturasPagar()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                           [Cantidad de facturas],
                           [Monto total pendiente]
                          FROM TOTALFACTURASPAGAR";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "TOTALFACTURASPAGAR");
                TablaFacturas = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar total de facturas: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void MostrarManteVehiculo()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT * FROM MANTEVEHICULO ORDER BY fecha DESC";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "MANTEVEHICULO");
                TablaMantenimiento = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar mantenimiento vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void MostrarResumenManteVehiculo()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                           placa,
                           marca,
                           modelo,
                           anio,
                           dekra_fecha,
                           [Dias para DEKRA],
                           [Total Mantenimientos],
                           [Ultimo Mantenimiento]
                          FROM RESUMENVEHICULOS";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "RESUMENVEHICULOS");
                TablaMantenimiento = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar resumen mantenimiento vehiculo: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void MostrarSaludMascota()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                           Mascota,
                           especie,
                           raza,
                           [Ultima vista],
                           motivo,
                           costo,
                           [Ultimo evento],
                           [fecha evento]
                          FROM SALUDMASCOTA";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "SALUDMASCOTA");
                TablaSalud = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperars salud mascpta: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void MostrarMedicamentosActivos()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                           Mascota,
                           Medicamento,
                           dosis,
                           frecuencia,
                           fecha_ini,
                           fecha_fin,
                           [Dias Restantes]
                          FROM MEDICAMENTOSACTIVOS";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "MEDICAMENTOSACTIVOS");
                TablaMedicamentos = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperars medicamentos mascota: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public void MostrarGastosVeterinaria()
        {
            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                           Mascota,
                           especie,
                           [Cantidad Visitas],
                           [Total Gastado]
                          FROM GASTOVETERINARIOPORMASCOTA";

            SqlDataAdapter sqlDA;

            if (dsTablas == null)
            {
                dsTablas = new DataSet();
            }

            if (dsTablas.Tables.Count > 0)
            {
                dsTablas.Tables.Clear();
            }

            EstablecerConexion();

            sql_instruccion = new SqlCommand(instruccion, _conexion);
            sqlDA = new SqlDataAdapter(sql_instruccion);

            try
            {
                sqlDA.Fill(dsTablas, "GASTOVETERINARIOPORMASCOTA");
                TablaVeterinaria = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperars gastos veterinaria: " + ex.Message);
            }
            finally
            {
                _conexion.Close();
            }
        }

        //METOSOS EXTRAS

        public bool ExistePresupuesto(int anio, string mes, int idCategoria)
        {
            try
            {
                EstablecerConexion();
                string query = "SELECT COUNT(*) FROM presupuestos WHERE anio = @anio AND mes = @mes AND id_categoria = @id_categoria";

                SqlCommand cmd = new SqlCommand(query, _conexion);
                cmd.Parameters.AddWithValue("@anio", anio);
                cmd.Parameters.AddWithValue("@mes", mes);
                cmd.Parameters.AddWithValue("@id_categoria", idCategoria);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _conexion.Close();
            }
        }

    }
}

