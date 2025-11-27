using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        DataSet dsTablas = new DataSet();
        SqlConnection _conexion;

        string _StrConexion = @"Server=DESKTOP-HGJUI2M;Database=domus_hogar;Integrated Security=true;TrustServerCertificate=true;";

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
                string query = @"
                DELETE FROM AsignacionesBotones WHERE NumeroBoton = @NumeroBoton;
                INSERT INTO AsignacionesBotones (NumeroBoton, IdMiembro, RutaImagen, NombreImagen) 
                VALUES (@NumeroBoton, @IdMiembro, @RutaImagen, @NombreImagen)";

                SqlCommand command = new SqlCommand(query, connection);
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
            string _insercion;

            try
            {
                EstablecerConexion();

                _insercion = "INSERT INTO areas (nombre, detalle)";
                _insercion += "VALUES (@nombre, @detalle)";

                _instruccionSQL = new SqlCommand(_insercion, _conexion);

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
            string _insercion;
            try
            {
                EstablecerConexion();
                _insercion = "INSERT INTO categorias_finanzas (nombre, tipo)";
                _insercion += "VALUES (@nombre, @tipo)";
                _instruccionSQL = new SqlCommand(_insercion, _conexion);
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
            string _insercion;
            try
            {
                EstablecerConexion();
                _insercion = "INSERT INTO proveedores (nombre, tipo)";
                _insercion += "VALUES (@nombre, @tipo)";
                _instruccionSQL = new SqlCommand(_insercion, _conexion);
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
        public void MostrarAsignacionTareas()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT 
                                ta.id_tarea,
                                t.titulo AS nombre_tarea,
                                ta.id_miembro, 
                                m.nombre AS nombre_miembro
                                FROM tareas_asignaciones ta
                                INNER JOIN tareas t ON ta.id_tarea = t.id_tarea
                                INNER JOIN miembros m ON ta.id_miembro = m.id_miembro";
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
                sqlDA.Fill(dsTablas, "tareas_asignaciones");
                TablaAsignacion_Tareas = dsTablas.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion tareas: " + ex.Message);
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
        public void MostrarFacturas()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT
                                pro.nombre   AS Proveedor,
                                fac.monto AS Monto,
                                cat.nombre AS Categoria,
                                fac.fecha_emision AS Emision,
                                fac.fecha_venc AS Vencimiento,
                                fac.estado AS Estado
                                FROM facturas fac
                                INNER JOIN proveedores pro ON fac.id_proveedor = pro.id_proveedor
                                INNER JOIN categorias_finanzas cat ON fac.categoria_id = cat.id_categoria
                                ORDER BY fac.monto";
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
                TablaFacturas = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion facturas: " + ex.Message);
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
            string instruccion = @"SELECT c.nombre, s.fecha_siembra,
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
            string instruccion = @"SELECT s.sector,
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
        public void MostrarMantenimiento()
        {

            SqlCommand sql_instruccion;
            string instruccion = @"SELECT v.placa, m.tipo, 
                                   m.concepto, m.fecha, m.kilometraje,m.costo,
                                   m.taller, m.notas       
                                 FROM vehiculos_mantenimientos m
                                 INNER JOIN vehiculos v ON m.id_vehiculo = v.id_vehiculo";

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
                sqlDA.Fill(dsTablas, "vehiculos_mantenimientos");
                TablaMantenimiento = dsTablas.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar asignacion mantenimiento: " + ex.Message);
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

    }
}

